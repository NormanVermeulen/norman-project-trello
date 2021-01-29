import { Component, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Inject } from '@angular/core';
import { UserService } from '../../services/user.service';
import { FormGroup } from '@angular/forms';
import { FormControl } from '@angular/forms';
import { FormBuilder } from '@angular/forms';
import { Validators } from '@angular/forms';
import * as _ from 'lodash-es';
import { User, Role } from 'src/app/models/user';

@Component({
    selector: 'app-edit-user-mat',
    templateUrl: './edit-user.component.html',
    styleUrls: ['./edit-user.component.css']
})
export class EditUserComponent {
    public frm: FormGroup;
    public frmPhone: FormGroup; 
    public ctlPseudo: FormControl;
    public ctlFirstName: FormControl;
    public ctlLastName: FormControl;
    public ctlEmail: FormControl;
    public ctlPassword: FormControl;
    public ctlBirthDate: FormControl;
    public ctlRole: FormControl;
    public ctlPhoneType: FormControl; 
    public ctlPhoneNumber: FormControl; 
    public isNew: boolean;
    public phones;

    constructor(public dialogRef: MatDialogRef<EditUserComponent>,
        @Inject(MAT_DIALOG_DATA) public data: { user: User; isNew: boolean; },
        private fb: FormBuilder,
        private userService: UserService
    ) {
        this.ctlPseudo = this.fb.control('', [
            Validators.required,
            Validators.minLength(3),
            this.forbiddenValue('abc')
        ], [this.pseudoUsed()]);
        this.ctlPassword = this.fb.control('', data.isNew ? [Validators.required, Validators.minLength(3)] : []);
        this.ctlFirstName = this.fb.control('', []);
        this.ctlLastName = this.fb.control('', []);
        this.ctlEmail = this.fb.control('', [Validators.required]);
        this.ctlBirthDate = this.fb.control('', [this.validateBirthDate()]);
        this.ctlRole = this.fb.control(Role.Member, []);
        this.frm = this.fb.group({
            pseudo: this.ctlPseudo,
            password: this.ctlPassword,
            firstName: this.ctlFirstName,
            lastName: this.ctlLastName,
            email: this.ctlEmail,
            birthDate: this.ctlBirthDate,
            role: this.ctlRole
        });

        this.ctlPhoneType = this.fb.control('', []); 
        this.ctlPhoneNumber = this.fb.control('', []); 
        this.frmPhone = this.fb.group({ 
            type: this.ctlPhoneType, 
            number: this.ctlPhoneNumber 
        }); 
        
        this.isNew = data.isNew;
        this.frm.patchValue(data.user);
    }


    forbiddenValue(val: string): any {
        return (ctl: FormControl) => {
            if (ctl.value === val) {
                return { forbiddenValue: { currentValue: ctl.value, forbiddenValue: val } };
            }
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

    onNoClick(): void {
        this.dialogRef.close();
    }

    update() {
        const data = this.frm.value; 
        data.phones = this.phones; 
        this.dialogRef.close(data); 
    }

    cancel() {
        this.dialogRef.close();
    }
    
    phoneAdd() { 
        if (!this.phones) { 
            this.phones = []; 
        } 
        this.phones.push(this.frmPhone.value); 
        this.frmPhone.reset(); 
        this.frm.markAsDirty(); 
    } 

    phoneDelete(phone) { 
        _.remove(this.phones, phone); 
        this.frm.markAsDirty(); 
    } 
}