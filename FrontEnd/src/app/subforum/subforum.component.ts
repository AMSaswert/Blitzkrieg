import { Component, OnInit,Input} from '@angular/core';
import {NgForm} from '@angular/forms';
import { NgZone } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import {SubforumService} from '../services/subforum.service';
import {ComplaintService} from '../services/complaint.service';
import {AppUserService} from '../services/appUser.service';
import {TopicService} from '../services/topic.service';
import { Subforum } from '../models/subforum.model';
import { Topic } from '../models/topic.model';
import { Complaint,EntityType } from '../models/complaint.model';
import { ActivatedRoute, Params } from '@angular/router';
@Component({
    selector: 'app-subforum',
    templateUrl: './subforum.component.html',
    providers: [SubforumService,ComplaintService],
  })

export class SubforumComponent implements OnInit{

    topics : Topic[] = [];
    subforumId : number;
    sub:any;
    complaintType : string = "Topic";
    entityType : EntityType = EntityType.Topic;
    nameOfTopic:  string = "";
    constructor(private httpSubforumService: SubforumService,private httpComplaintService : ComplaintService
        ,private httpAppUserService : AppUserService ,private httpTopicService : TopicService ,private route: ActivatedRoute) {

    }

    ngOnInit() {
        this.route.params.subscribe(params => {
            this.subforumId = +params['id'] ;  });
            
            this.httpSubforumService.getDatabyId(this.subforumId).subscribe(
                (prod: any) => {this.topics=prod.Topics; console.log(this.topics)},
                 error => {alert("Unsuccessful fetch operation!"); console.log(error);}); 
            
    }

    isLoggedIn() : boolean
    {
      return this.httpAppUserService.isLoggedIn();
    }

    getRandomInt(min, max) {
     return Math.floor(Math.random() * (max - min + 1)) + min;
 }


    Like(topic:Topic) : void
    {
        topic.LikesNum +=1;
        topic.UsersWhoVoted.push(sessionStorage.getItem("user"));
        this.httpTopicService.put(this.subforumId,topic);
    }

    Dislike(topic:Topic) : void
    {
        topic.DislikesNum +=1;
        topic.UsersWhoVoted.push(sessionStorage.getItem("user"));
        this.httpTopicService.put(this.subforumId,topic);
    }

    voted(topic:Topic) : boolean
    {
    var x = -1;
    x = topic.UsersWhoVoted.indexOf(sessionStorage.getItem("user"));
    if(x != -1)
        return true;
    return false;
    }

    routing(topic: Topic) : void
    {
        this.httpAppUserService.routing("/topic/"+topic.Id.toString());
    }
   
}