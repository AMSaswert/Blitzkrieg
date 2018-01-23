import { Component, OnInit,Input,Output } from '@angular/core';
import {NgForm} from '@angular/forms';
import { NgZone } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import {TopicService} from '../services/topic.service';
import {CommentService} from '../services/comment.service';
import {AppUserService} from '../services/appUser.service';
import { Subforum } from '../models/subforum.model';
import { Topic } from '../models/topic.model';
import { Comment } from '../models/comment.model';
import { ActivatedRoute, Params } from '@angular/router';
import { ReferenceAst } from '@angular/compiler';
@Component({
    selector: 'app-topic',
    templateUrl: './topic.component.html',
    providers: [TopicService,CommentService],
  })

export class TopicComponent implements OnInit{
    
    topic: Topic;
    topicId : number;
    subforumId : number;
    comments : Comment[]=[];
    comment: Comment = new Comment();

    constructor(private httpService: TopicService,private httpCommentService : CommentService
        ,private httpAppUserService : AppUserService,private route: ActivatedRoute) {

    }

    ngOnInit() {
        this.route.params.subscribe(params => {
            this.topicId = +params['id'] ; this.subforumId = +params['subId'];  });
            
            this.httpService.getDatabyId(this.topicId).subscribe(
                (prod: any) => {this.topic = prod;this.comments = prod.Comments; console.log(this.topic)},
                 error => {alert("Unsuccessful fetch operation!"); console.log(error);}); 
            
    }

    isLoggedIn() : boolean
    {
      return this.httpAppUserService.isLoggedIn();
    }

    onSubmit(comment: Comment,form: NgForm)
    {
      
      comment.Id = this.httpAppUserService.getRandomInt(1,9999999);
      comment.AuthorUsername = sessionStorage.getItem("username");
      comment.ChildrenComments = new Array<Comment>();
      comment.CreationDate = new Date(Date.now());
      comment.DislikesNo = 0;
      comment.Edited = false;
      comment.LikesNo = 0;
      comment.ParentCommentId = null;
      comment.Removed = false;
      comment.TopicId = this.topicId;
      comment.UsersWhoVoted = new Array<string>();
      
      this.httpCommentService.put(this.topicId,comment);
      this.comments.push(comment);
      form.reset();
    }
}