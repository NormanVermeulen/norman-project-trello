import { Component, OnInit, ViewChild, AfterViewInit, ElementRef, Renderer2 } from '@angular/core';
import { Board } from 'src/app/models/board';
import { User } from 'src/app/models/user';
import { List } from 'src/app/models/list';
import { Card } from 'src/app/models/card';
import { BoardService } from '../../services/board.service';
import { UserService } from '../../services/user.service';
import { EditCardComponent } from '../edit-card/edit-card.component';
import { MatDialog } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ActivatedRoute } from '@angular/router';
import * as _ from 'lodash-es';

import {
  CdkDragDrop, moveItemInArray, transferArrayItem,
  CdkDragStart, CdkDragEnd, CdkDragEnter, CdkDragExit
} from '@angular/cdk/drag-drop';
import { cpuUsage } from 'process';
import { FormControl } from '@angular/forms';


@Component({
  selector: 'app-board',
  templateUrl: './board.component.html',
  styleUrls: ['./board.component.css']
})

export class BoardComponent implements OnInit {

  displayedColumns: string[] = ['name'];

  board: Board;
  owner: User;
  lists: (List)[];
  showCard: boolean[] = [];
  showList: boolean;

  constructor(
    private boardService: BoardService,
    private userService: UserService,
    public dialog: MatDialog,
    public route: ActivatedRoute,
    public snackBar: MatSnackBar
  ) {
  }

  ngOnInit() {

    let id = this.route.snapshot.params['id'];
    this.boardService.getBoardById(+id).subscribe(b => {
      
      this.board = b;
      this.lists = b.lists;
      this.userService.getById(b.ownerId).subscribe(u => {
        this.owner = u;
      });
      
    });
    
  }

  drop(event: CdkDragDrop<Card[]>, l: List) {
    if (event.previousContainer === event.container) {
      moveItemInArray(event.container.data, event.previousIndex, event.currentIndex);
      for (let i = 0; i < l.cards.length; i++) {
        l.cards[i].pos = i;
        this.updateCard(l.cards[i]);
      }
    } else {
      transferArrayItem(event.previousContainer.data,
        event.container.data,
        event.previousIndex,
        event.currentIndex);
        event.item.data.listId = l.id;
      for(let i = 0; i < event.previousContainer.data.length; i++){
        event.previousContainer.data[i].pos = i;
        this.updateCard(event.previousContainer.data[i]);
        
      }
      for(let i = 0; i < event.container.data.length; i++){
        event.container.data[i].pos = i;
        this.updateCard(event.container.data[i]);
      }
    }
  }

  isFirst(l: List){
    return l.pos == 0;
  }

  isLast(l: List){
    return l.pos == this.board.lists.length - 1;
  }

  moveList(l1: List, l2: List){
    this.updatePosList(l1, l2);
  }

  createList(listName: string) {
    if (listName)
      var list = new List({ name: listName });
    
    this.lists.push(list);
    this.boardService.addList(list, this.board.id).subscribe(b => this.ngOnInit());
  }

  updateListName(event, list: List) {
    const name = list.name;

    event.target.addEventListener("keydown", (e: KeyboardEvent) => {
      if (e.key === "Enter") {
        e.preventDefault();
        list.name = event.target.textContent;
        if (event.target.textContent == "") {
          event.target.textContent = name;
        }
        else {
          this.boardService.updateList(list).subscribe();
        }
      }
    });
  }

  deleteList(list: List) {
    this.boardService.deleteList(list).subscribe(b => this.ngOnInit());
  }

  updatePosList(list1: List, list2: List){
    this.boardService.updatePosList(list1, list1.id, list2.id).subscribe(l => this.ngOnInit());
  }

  createCard(cardName: string, listId: number) {
    if (cardName.length > 2) {
      if (this.checkCardName(cardName)) {
        var card = new Card({ name: cardName, listId: listId });
        var list = this.lists.find(l => l.id === listId);
        list.cards.push(card);
        this.boardService.addCard(card, this.board.id, list.id).subscribe(c => {
          this.ngOnInit();
        });
      }
      else
        this.snackBar.open("This card's name is already use on board", 'Dismiss', { duration: 5000 });
    }
    else
      this.snackBar.open("Minimum 3 characters and max 20 for card's name", 'Dismiss', { duration: 5000 });
  }

  checkCardName(cardName: string) {
    var ok: Boolean;
    ok = true;
    this.lists.forEach(l => {
      l.cards.forEach(c => {
        if (c.name == cardName)
          ok = false;
      });
    });
    return ok;
  }

  updateCard(c: Card) {
    this.boardService.updatePosCard(c, c.id).subscribe(b => this.ngOnInit());
  }

  deleteCard(card: Card) {
    this.boardService.deleteCard(card).subscribe(b => this.ngOnInit());
  }

  editCard(card: Card) {
    const board = this.board;
    const dlg = this.dialog.open(EditCardComponent, { data: { board, card, isNew: false } });
    dlg.beforeClosed().subscribe(res => {
      if (res) {
        _.assign(card, res);
        this.boardService.updateCard(res).subscribe(res => {
          if (!res) {
            this.snackBar.open(`There was an error at the server. The update has not been done! Please try again.`, 'Dismiss', { duration: 10000 });
            this.ngOnInit();
          }
        });
      }
    });
  }

}
