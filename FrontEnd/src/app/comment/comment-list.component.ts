import { Component, OnInit,Input } from '@angular/core';
import {NgForm} from '@angular/forms';
import {CommentService} from '../services/comment.service';
import {AppUserService} from '../services/appUser.service';
import {SubforumService} from '../services/subforum.service';
import { AppUser } from '../models/appUser.model';
import { Comment } from '../models/comment.model';
import { Complaint,EntityType } from '../models/complaint.model';
import { Subforum } from '../models/subforum.model';
@Component({
    selector: 'app-comment-list',
    templateUrl: './comment-list.component.html',
    providers: [CommentService]
  })

export class CommentListComponent implements OnInit{

  @Input()comments : Comment[] = [];
  @Input()topicId : number;
  @Input()subforumId : number;
  complaintType : string = "Comment";
  entityType : EntityType = EntityType.Comment;
  subforum : Subforum = new Subforum();
  editId : number;
  @Input() user : AppUser;

    constructor(private httpCommentService: CommentService,private httpAppUserService : AppUserService,
      private httpSubforumService : SubforumService ) {
    }


    ngOnInit() {      
      this.httpSubforumService.getDatabyId(this.subforumId).subscribe(
        (prod: any) => {this.subforum = prod;});
    }


    isLoggedIn() : boolean
    {
      this.checkComment(this.comments);
      return this.httpAppUserService.isLoggedIn();
    }

    like(comment:Comment) : void
    {
        comment.LikesNo +=1;
        comment.UsersWhoVoted.push(sessionStorage.getItem("username"));
        this.httpCommentService.put(comment.TopicId,comment);
    }

    dislike(comment:Comment) : void
    {
        comment.DislikesNo +=1;
        comment.UsersWhoVoted.push(sessionStorage.getItem("username"));
        this.httpCommentService.put(comment.TopicId,comment);
    }

    voted(comment:Comment) : boolean
    {
      var x = -1;
      x = comment.UsersWhoVoted.findIndex(x=> x == sessionStorage.getItem("username"));
      if(x != -1)
        return true;
      return false;
    }

    deleteAuth(comment : Comment): boolean
    {
      if(comment.AuthorUsername == sessionStorage.getItem("username") || "Admin" == sessionStorage.getItem("role") || 
        this.subforum.LeadModeratorUsername == sessionStorage.getItem("username"))
        return true;
      return false;
    }

    editAuth(comment : Comment) : boolean
    {
      if(comment.AuthorUsername == sessionStorage.getItem("username") ||
         this.subforum.LeadModeratorUsername == sessionStorage.getItem("username"))
         {
           return true;
         }

        for(var moderator of this.subforum.Moderators)
        {
          if(moderator == sessionStorage.getItem("username"))
          {
            return true;
          }       
        }

      return false;
    }

    delete(comment: Comment) : void
    {
      this.httpCommentService.delete(comment.Id.toString()+"-"+comment.TopicId.toString()).subscribe(
        data => {
          alert("Comment is deleted.");
      },
      error => {
          alert("Comment is already deleted.");
      });
      this.comments.splice(this.comments.findIndex(x=>x.Id==comment.Id),1);
    }

    forEditing(commentId: number) : void
    {
       this.editId = commentId;
    }

    edit(tempComment: Comment,form : NgForm,comment: Comment)
    {
      comment.Text = tempComment.Text;
      if(comment.AuthorUsername == sessionStorage.getItem("username"))
      {
        comment.Edited = true;
      }

      this.httpCommentService.put(this.topicId,comment);
      this.comments.splice(this.comments.findIndex(x=>x.Id==comment.Id),1,comment);
      this.editId = -1;
      form.reset();
    }

    onSubmit(comment: Comment,form: NgForm,parentCommentId: number)
    {
      
      comment.Id = this.httpAppUserService.getRandomInt(1,9999999);
      comment.AuthorUsername = sessionStorage.getItem("username");
      comment.ChildrenComments = new Array<Comment>();
      comment.CreationDate = new Date(Date.now());
      comment.DislikesNo = 0;
      comment.Edited = false;
      comment.LikesNo = 0;
      comment.ParentCommentId = parentCommentId;
      comment.Removed = false;
      comment.TopicId = this.topicId;
      comment.UsersWhoVoted = new Array<string>();
      
      this.httpCommentService.put(this.topicId,comment);
      this.comments.splice(this.comments.findIndex(x=>x.Id==parentCommentId)+1,0,comment);
      form.reset();
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

    saveComment(comment : Comment) : void
    {
      this.user.SavedComments.push(comment);
      this.httpAppUserService.put(this.user.Id,this.user);
    }

}