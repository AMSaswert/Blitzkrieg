import { Component, OnInit,Input } from '@angular/core';
import {NgForm} from '@angular/forms';
@Component({
    selector: 'comment-saved',
    templateUrl: './comment-saved.component.html',
  })

export class CommentSaved{

  @Input()comments : Comment[] = [];

    constructor() {
    }
}