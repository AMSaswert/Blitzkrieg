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
@Component({
    selector: 'app-topic',
    templateUrl: './topic.component.html',
    providers: [TopicService],
  })

export class TopicComponent implements OnInit{
    
    topic: Topic;
    topicId : number;
    sub:any;
    comments : Comment[]=[];
    content: string = "";
    comment: Comment = new Comment();
    constructor(private httpService: TopicService,private httpCommentService : CommentService
        ,private httpAppUserService : AppUserService,private route: ActivatedRoute) {

    }

    ngOnInit() {
        this.route.params.subscribe(params => {
            this.topicId = +params['id'] ;  });
            
            this.httpService.getDatabyId(this.topicId).subscribe(
                (prod: any) => {this.topic = prod;this.comments = prod.Comments;
                    /*
                    for(var comment of this.comments)
                    {
                        if(comment.Removed == true)
                        {
                        this.comments.splice(this.comments.findIndex(x=>x.Id==comment.Id),1);
                        }
                    }
                    */
                  ; console.log(this.topic)},
                 error => {alert("Unsuccessful fetch operation!"); console.log(error);}); 
            
    }

    isLoggedIn() : boolean
    {
      return this.httpAppUserService.isLoggedIn();
    }

    sendComment() : void
    {
        this.comment.Text = this.content;
        this.comment.Id = this.httpAppUserService.getRandomInt(1,9999999);
        this.comment.Removed = false;
        this.comment.UsersWhoVoted = new Array<string>();
        this.comment.AuthorUsername = sessionStorage.getItem("username");
        this.comment.TopicId = this.topicId;
        this.comment.CreationDate = new Date(Date.now());
        this.comment.ChildrenComments = new Array<Comment>();
        this.httpCommentService.put(this.topicId,this.comment);
        this.comments.push(this.comment);
        this.content = "";
    }
   
}