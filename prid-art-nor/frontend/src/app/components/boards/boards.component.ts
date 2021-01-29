import { AfterViewInit, Component, OnInit, ViewChild, OnDestroy  } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Inject } from '@angular/core';
import { UserService } from '../../services/user.service';
import { FormGroup } from '@angular/forms';
import { FormControl } from '@angular/forms';
import { FormBuilder } from '@angular/forms';
import { Validators } from '@angular/forms';
import * as _ from 'lodash-es';
import { Board } from 'src/app/models/board';
import { BoardService } from '../../services/board.service';
import { StateService } from 'src/app/services/state.service';
import { Router } from '@angular/router';
import { MatTableState } from 'src/app/helpers/mattable.state';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatDialog } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { EditBoardComponent } from '../edit-board/edit-board.component';
import { AuthenticationService } from '../../services/authentication.service';
import { Role, User } from 'src/app/models/user';


@Component({
    selector: 'app-boards',
    templateUrl: './boards.component.html',
    styleUrls: ['./boards.component.css']
})
export class BoardsComponent implements AfterViewInit{
   
    dataOwn: Board[] = [];
    dataCollabs: Board[] =[];
    currentUser: User;

    @ViewChild(MatPaginator, { static: false }) paginator: MatPaginator;
    @ViewChild(MatSort, { static: false }) sort: MatSort;

    constructor(
        private boardService: BoardService,
        private stateService: StateService,
        public dialog: MatDialog,
        public snackBar: MatSnackBar,
        public auth: AuthenticationService
    ) {

    }

     ngAfterViewInit(): void {
        this.refresh();
    }

    refresh() {
        this.currentUser = this.auth.currentUser;
        if(this.currentUser.role == 2){         
            this.boardService.getBoards().subscribe(boards => {
                this.dataOwn = boards;
            });
        }
        else
            {
                this.boardService.getBoardsByOwner(this.currentUser.id).subscribe(boardsOwn => {
                    this.dataOwn = boardsOwn;
                });
                this.boardService.getBoardsByCollabs(this.currentUser.id).subscribe(boardsCollabs => {
                    this.dataCollabs = boardsCollabs;
                });
            }
    }

    dontHide(boards: Board[]){
           return boards.length >= 1;           
    }

    createBoard() {
        const board = new Board({});
        const dlg = this.dialog.open(EditBoardComponent, { data: { board, isNew: true } });
        dlg.beforeClosed().subscribe(res => {
            if (res) {
                this.dataOwn = [...this.dataOwn, new Board(res)];
                this.boardService.add(res).subscribe(res => {
                    if (!res) {
                        this.snackBar.open('There was an error at the server. The board has not been created! Please try again.', 'Dismiss', { duration: 10000 });
                    }
                    this.refresh();
                });
            }
        });
    }


    delete(board: Board) {
        const backup = this.dataOwn;
        this.dataOwn = _.filter(this.dataOwn, b => b.id !== board.id);
        const snackBarRef = this.snackBar.open(`Board '${board.id}' will be deleted`, 'Undo', { duration: 10000 });
        snackBarRef.afterDismissed().subscribe(res => {
            if (!res.dismissedByAction)
                this.boardService.delete(board).subscribe(del =>{
                    this.refresh();
                });
            else
                this.dataOwn = backup;
        });
    }
    
    edit(board: Board) {
        const dlg = this.dialog.open(EditBoardComponent, { data: { board, isNew: false } });
        dlg.beforeClosed().subscribe(res => {
            if (res) {
                _.assign(board, res);
                this.boardService.update(res).subscribe(res => {
                    if (!res) {
                        this.snackBar.open(`There was an error at the server. The update has not been done! Please try again.`, 'Dismiss', { duration: 10000 });
                        this.refresh();
                    }
                });
            }
        });
    }
            
}