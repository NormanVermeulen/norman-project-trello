import { Component } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { FormBuilder, FormGroup, Validators, FormControl, AsyncValidatorFn, ValidationErrors } from '@angular/forms';
import { AuthenticationService } from '../../services/authentication.service';
import { Inject } from '@angular/core';
import { UserService } from '../../services/user.service';
import * as _ from 'lodash-es';
import { User, Role } from 'src/app/models/user';
import { MatTableDataSource } from '@angular/material/table';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
    templateUrl: 'signup.component.html',
    styleUrls: ['signup.component.css']
})
export class SignupComponent {
    dataSource: MatTableDataSource<User> = new MatTableDataSource();
    signupForm: FormGroup;
    loading = false;    // utilisé en HTML pour désactiver le bouton pendant la requête de login
    submitted = false;  // retient si le formulaire a été soumis ; utilisé pour n'afficher les 
    // erreurs que dans ce cas-là (voir template)
    returnUrl: string;
    ctlPseudo: FormControl;
    ctlPassword: FormControl;
    ctlPasswordConfirm: FormControl;
    ctlFirstName: FormControl;
    ctlLastName: FormControl;
    ctlEmail: FormControl;
    ctlBirthDate: FormControl;

    // dataSource: MatTableDataSource<User> = new MatTableDataSource();

    constructor(
        private fb: FormBuilder,
        private route: ActivatedRoute,
        private router: Router,
        private authService: AuthenticationService,
        private userService: UserService,
        public snackBar: MatSnackBar
    ) {
        this.ctlPseudo = this.fb.control('', [
            Validators.required,
            Validators.minLength(3),
            this.forbiddenValue('abc')
        ], [this.pseudoUsed()]);
        this.ctlPassword = this.fb.control('', [Validators.required, Validators.minLength(3)]);
        this.ctlPasswordConfirm = this.fb.control('', [Validators.required, Validators.minLength(3)]);

        this.ctlFirstName = this.fb.control('', [Validators.required, this.ValidationsFirstName()]);
        this.ctlLastName = this.fb.control('', [Validators.required, this.ValidationsLastName()]);
        this.ctlEmail = this.fb.control('', [Validators.required, this.checkEmail()]);
        this.ctlBirthDate = this.fb.control('', [this.validateBirthDate()]);
        this.signupForm = this.fb.group({
            pseudo: this.ctlPseudo,
            password: this.ctlPassword,
            passwordConfirm: this.ctlPasswordConfirm,
            firstName: this.ctlFirstName,
            lastName: this.ctlLastName,
            email: this.ctlEmail,
            birthDate: this.ctlBirthDate
        }, { validators: this.crossValidations });
    }

    forbiddenValue(val: string): any {
        return (ctl: FormControl) => {
            if (ctl.value === val) {
                return { forbiddenValue: { currentValue: ctl.value, forbiddenValue: val } };
            }
            return null;
        };
    }

    ValidationsFirstName(): any {
        return (ctl: FormControl) => {
            if (ctl.value !== '') {
                return this.ctlLastName?.value === '' ? { lastNameNull: true } : null;
            }
            return null;
        };

    }

    ValidationsLastName(): any {
        return (ctl: FormControl) => {
            if (ctl.value !== '') {
                return this.ctlFirstName?.value === '' ? { firstNameNull: true } : null;
            }
            return null;
        };
    }

    checkEmail(): any {
        var regexp = new RegExp(/^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/);
        return (ctl: FormControl) => {
            if (!regexp.test(ctl.value))
                return { emailFormat: true };
            return null;
        };
    }

    validateBirthDate(): any {
        return (ctl: FormControl) => {
            const date = new Date(ctl.value);
            const diff = Date.now() - date.getTime();
            if (diff < 0)
                return { futureBorn: true }
            var age = new Date(diff).getUTCFullYear() - 1970;
            if (age < 18)
                return { tooYoung: true };
            return null;
        };
    }

    // Validateur asynchrone qui vérifie si le pseudo n'est pas déjà utilisé par un autre membre
    pseudoUsed(): any {
        let timeout: NodeJS.Timer;
        return (ctl: FormControl) => {
            clearTimeout(timeout);
            const pseudo = ctl.value;
            return new Promise(resolve => {
                timeout = setTimeout(() => {
                    if (ctl.pristine) {
                        resolve(null);
                    } else {
                        this.userService.getByPseudo(pseudo).subscribe(user => {
                            resolve(user ? { pseudoUsed: true } : null);

                        });
                    }
                }, 300);
            });
        };
    }

    crossValidationsFirstLastName(group: FormGroup): ValidationErrors {
        if (group.value) { return null; }
    }

    crossValidations(group: FormGroup): ValidationErrors {
        if (!group.value) { return null; }
        return group.value.password === group.value.passwordConfirm ? null : { passwordNotConfirmed: true };
    }

    signup() {
        this.authService.signup(
            this.ctlPseudo.value,
            this.ctlPassword.value,
            this.ctlFirstName.value,
            this.ctlLastName.value,
            this.ctlEmail.value,
            this.ctlBirthDate.value
        ).subscribe(() => {
            if (this.authService.currentUser) {
                this.router.navigate(['/']);
            }
        });
    }

}
