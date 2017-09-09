import { Component, OnInit,Input } from '@angular/core';
import {NgForm} from '@angular/forms';
import {CommentService} from '../services/comment.service';
import { AppUser } from '../models/appUser.model';
import { Comment } from '../models/comment.model';
@Component({
    selector: 'app-comment',
    templateUrl: './comment.component.html',
    providers: [CommentService]
  })

export class CommentComponent implements OnInit{

  comments: Comment[];
    

    constructor(private httpCommentService: CommentService ) {
    }


    ngOnInit() {
                
        this.httpCommentService.getDatabyId(1622326492).subscribe(
            (prod: any) => {this.comments = prod; console.log(this.comments)},//You can set the type to Product.
             error => {alert("Unsuccessful fetch operation!"); console.log(error);});
             
    }

    onSubmit(comment: Comment, form: NgForm) {

        comment.CreationDate = new Date(Date.now());
        comment.ChildrenComments = new Array<Comment>();
        comment.UsersWhoVoted = new Array<AppUser>();

        this.httpCommentService.put(1622326492,comment);
        form.reset();
        window.location.reload();
        
      }

      edit(user: AppUser, form: NgForm) {
        
    
                 
         form.reset();
         window.location.reload();
       }

       delete(comment: Comment, form: NgForm) {
         
         this.httpCommentService.delete(1722859085,703184974);
       }
}