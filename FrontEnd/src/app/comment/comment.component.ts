import { Component, OnInit,Input } from '@angular/core';
import {NgForm} from '@angular/forms';
import {CommentService} from '../services/comment.service';
import {AppUserService} from '../services/appUser.service';
import { AppUser } from '../models/appUser.model';
import { Comment } from '../models/comment.model';
@Component({
    selector: 'app-comment',
    templateUrl: './comment.component.html',
  })

export class CommentComponent implements OnInit{

 @Input() comments: Comment[] = [];
    

    constructor(private httpAppUserService : AppUserService,private httpCommentService : CommentService ) {
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

    getRandomInt(min, max) {
        return Math.floor(Math.random() * (max - min + 1)) + min;
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

    /*
    onSubmit(comment: Comment, form: NgForm) {

        comment.CreationDate = new Date(Date.now());
        comment.ChildrenComments = new Array<Comment>();
        comment.UsersWhoVoted = new Array<string>();

        this.httpCommentService.put(231651,comment);
        form.reset();
        window.location.reload();
        
      }

      edit(user: AppUser, form: NgForm) {
        
    
                 
         form.reset();
         window.location.reload();
       }

       delete(comment: Comment, form: NgForm) {
         
         this.httpCommentService.delete(comment.Id);
         form.reset();
         window.location.reload();
       }
       */
}