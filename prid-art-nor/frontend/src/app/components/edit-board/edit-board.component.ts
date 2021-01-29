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
import { AuthenticationService } from '../../services/authentication.service';


@Component({
    selector: 'app-edit-board-mat',
    templateUrl: './edit-board.component.html',
    styleUrls: ['./edit-board.component.css']
})
export class EditBoardComponent{
    public formBoard: FormGroup;
    public ctlName: FormControl;
    public ctlCollabs: FormControl;
    public isNew: boolean;

    public listUsers: User[] = [];

    constructor(
        public dialogRef: MatDialogRef<EditBoardComponent>,
        @Inject(MAT_DIALOG_DATA) public data: { board: Board; isNew: boolean; },
        private fb: FormBuilder,
        private auth: AuthenticationService,
        private boardService: BoardService,
        private userService: UserService
    ) {
        this.isNew = data.isNew;
        this.ctlName = this.fb.control('', [Validators.required,Validators.minLength(3),Validators.maxLength(20)]);
        this.ctlCollabs = this.fb.control([]);
    
        this.userService.getAll().subscribe(e => {
            e.forEach(async val => {
                if(this.auth.currentUser.id !== val.id)
                    this.listUsers.push(val);
            })
        });

        this.formBoard = this.fb.group({
            name: this.ctlName,
            users: this.ctlCollabs
        });

        if (!this.isNew) {
            this.boardService.getCollaboraters(data.board.id).subscribe(listUserDTO => {
                var listId: number[] = []; 
                listUserDTO.forEach(async user => {
                    listId.push(user.id);
                })
                this.ctlCollabs.setValue(listId);
            });       
        }
        this.formBoard.patchValue(data.board);
    }
    
    update() {
        var data;
        if(!this.isNew){
             data = {
                id: this.data.board.id,
                name: this.formBoard.get('name').value,
                collaborations: this.formBoard.get('users').value
            }
        }
        else{
             data = {
                name: this.formBoard.get('name').value,
                collaborations: this.formBoard.get('users').value
            }
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