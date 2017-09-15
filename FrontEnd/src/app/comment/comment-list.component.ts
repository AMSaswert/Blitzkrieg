import { Component, OnInit,Input } from '@angular/core';
import {NgForm} from '@angular/forms';
import {CommentService} from '../services/comment.service';
import {AppUserService} from '../services/appUser.service';
import { AppUser } from '../models/appUser.model';
import { Comment } from '../models/comment.model';
@Component({
    selector: 'app-comment-list',
    templateUrl: './comment-list.component.html',
    providers: [CommentService]
  })

export class CommentListComponent implements OnInit{

  @Input()childrenComments : Comment[] = [];

    constructor(private httpCommentService: CommentService,private httpAppUserService : AppUserService ) {
    }


    ngOnInit() {
                
             
    }


    isLoggedIn() : boolean
    {
      return this.httpAppUserService.isLoggedIn();
    }

    like(comment:Comment) : void
    {
        comment.LikesNo +=1;
        comment.UsersWhoVoted.push(sessionStorage.getItem("user"));
        this.httpCommentService.put(comment.Id,comment);
    }

    dislike(comment:Comment) : void
    {
        comment.DislikesNo +=1;
        comment.UsersWhoVoted.push(sessionStorage.getItem("user"));
        this.httpCommentService.put(comment.Id,comment);
    }

    voted(comment:Comment) : boolean
    {
      var x = -1;
      x = comment.UsersWhoVoted.indexOf(sessionStorage.getItem("user"));
      if(x != -1)
        return true;
      return false;
    }

    deleteAuth(comment : Comment): boolean
    {
      if(comment.AuthorUsername == sessionStorage.getItem("username") || "Admin" == sessionStorage.getItem("role") || 
        "Moderator" == sessionStorage.getItem("role"))
        return true;
      return false;
    }

    delete(commentId: number) : void
    {
      this.httpCommentService.delete(commentId);
    }

}