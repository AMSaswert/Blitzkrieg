import { Component, OnInit,Input } from '@angular/core';
import {NgForm} from '@angular/forms';
import { Comment } from '../models/comment.model';
@Component({
    selector: 'comment-saved',
    templateUrl: './comment-saved.component.html',
  })

export class CommentSaved{

  @Input()comments : Comment[] = [];

    constructor() {
    }

    forCheck() : boolean
    {
      this.checkComment(this.comments);
      return true;
    }


    checkComment(comments : Array<Comment>)
    {
      for(var item of comments)
      {
          if (item.Removed == true)
          {
              comments.splice(comments.findIndex(x=>x.Id==item.Id),1);
          }
          else
          {
              this.checkComment(item.ChildrenComments);             
          }
      }       
    }
}