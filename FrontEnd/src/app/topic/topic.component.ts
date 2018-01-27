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
import {SubforumService} from '../services/subforum.service';
import {Location} from '@angular/common'
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
    rTopics : Topic[]=[];
    topics : Topic[] = [];
    @Input()  subforums: Subforum[];
    recommendedTitle: string = "";

    constructor(private httpSubforumService: SubforumService
        ,private httpService: TopicService,private httpCommentService : CommentService
        ,private httpAppUserService : AppUserService,private route: ActivatedRoute,
        private _location: Location) {

    }

    ngOnInit() {
        this.route.params.subscribe(params => {
            this.topicId = +params['id'] ; this.subforumId = +params['subId'];});
            
            this.httpService.getDatabyId(this.topicId).subscribe(
                (prod: any) => {this.topic = prod;this.comments = prod.Comments; this.getAllSubforums();
                 console.log(this.topic)},
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

    getAllSubforums()
    {
        this.httpSubforumService.getData().subscribe(
            (prod: any) => {this.subforums = prod;  this.getAllTopics();});
    }

    getAllTopics()
    {
        for(var subforum of this.subforums)
            {
                this.topics.push.apply(this.topics, subforum.Topics);
            } 
        this.getRecommendations();
    }
    getRecommendations()
    {
        this.rTopics.splice(0);
        for(var topic of this.topics)
            {
                if (topic.Id !== this.topicId) {
                    for(var user of this.topic.UsersWhoVoted)
                    {
                        if (topic.UsersWhoVoted.indexOf(user) !== -1) {
                            this.rTopics.push(topic);
                            break;
                        }
                    }
                }
            }
        if (this.rTopics.length === 0) {
            this.recommendedTitle = "Most popular topics";
            this.rTopics.push.apply(this.rTopics, this.topics);
        }
        else {
            this.recommendedTitle = "People who liked this also liked";
        }
        this.rTopics.sort((a: Topic, b: Topic) => {
                if (a.LikesNum > b.LikesNum) {
                  return -1;
                } else if (a.LikesNum < b.LikesNum) {
                  return 1;
                } else {
                  return 0;
                }
              });
              this.rTopics.splice(6);
    }
    
    routing(topic: Topic) : void
    {
        sessionStorage.setItem("topicRoute","/topic/"+topic.Id.toString()+"/"+this.subforumId.toString());
        this._location.back();
    }
}