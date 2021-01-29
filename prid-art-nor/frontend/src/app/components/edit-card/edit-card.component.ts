import { Component, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Inject } from '@angular/core';
import { UserService } from '../../services/user.service';
import { BoardService } from '../../services/board.service';
import { FormGroup } from '@angular/forms';
import { FormControl } from '@angular/forms';
import { FormBuilder } from '@angular/forms';
import { Validators } from '@angular/forms';
import * as _ from 'lodash-es';
import { User, Role } from 'src/app/models/user';
import { Board } from 'src/app/models/board';
import { Card } from 'src/app/models/card';
import { AuthenticationService } from '../../services/authentication.service';


@Component({
    selector: 'app-edit-card-mat',
    templateUrl: './edit-card.component.html',
    styleUrls: ['./edit-card.component.css']
})
export class EditCardComponent {
    public formCard: FormGroup;
    public ctlName: FormControl;
    public ctlParts: FormControl;

    public listUsers: User[] = [];

    constructor(
        public dialogRef: MatDialogRef<EditCardComponent>,
        @Inject(MAT_DIALOG_DATA) public data: { card: Card; board: Board },
        private fb: FormBuilder,
        private auth: AuthenticationService,
        private boardService: BoardService
    ) {

        this.ctlName = this.fb.control('', [Validators.required, Validators.minLength(3), Validators.maxLength(20)], [this.checkCardNameUsed()]);
        this.ctlParts = this.fb.control([]);

        this.boardService.getCollaboraters(data.board.id).subscribe(e => {
            e.forEach(async val => {
                    this.listUsers.push(val);
            })
        });

        this.formCard = this.fb.group({
            name: this.ctlName,
            UserParticipations: this.ctlParts
        });

        this.boardService.getParticipaters(data.card.id).subscribe(e => {
            var listId: number[] = [];
            e.forEach(async user => {
                listId.push(user.id);
            })
            this.ctlParts.setValue(listId);
        });

        this.formCard.patchValue(data.card);
    }

    checkCardNameUsed(): any {
        let timeout: NodeJS.Timer;
        return (ctl: FormControl) => {
            clearTimeout(timeout);
            const cardName = ctl.value;
            return new Promise(resolve => {
                timeout = setTimeout(() => {
                    if (ctl.pristine) {
                        resolve(null);
                    } else {
                        this.boardService.getCardByName(this.data.board.id,cardName).subscribe(card => {
                            resolve(card ? { checkCardNameUsed: true } : null);
                        });
                    }
                }, 300);
            });
        };
    }

    update() {
        var data;
        data = {
            id: this.data.card.id,
            name: this.formCard.get('name').value,
            UserParticipations: this.formCard.get('UserParticipations').value
        }
        this.dialogRef.close(data);
    }

    cancel() {
        this.dialogRef.close();
    }

    onNoClick(): void {
        this.dialogRef.close();
    }


}