import { Component, OnInit,Input} from '@angular/core';
import {NgForm} from '@angular/forms';
import { NgZone } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import {SubforumService} from '../services/subforum.service';
import {ComplaintService} from '../services/complaint.service';
import {AppUserService} from '../services/appUser.service';
import {TopicService} from '../services/topic.service';
import { Subforum } from '../models/subforum.model';
import { Topic, TopicType } from '../models/topic.model';
import { Complaint,EntityType } from '../models/complaint.model';
import { ActivatedRoute, Params } from '@angular/router';
import { NgFor } from '@angular/common/src/directives';
import { Comment } from '../models/comment.model';
@Component({
    selector: 'app-subforum',
    templateUrl: './subforum.component.html',
    providers: [SubforumService,ComplaintService],
  })

export class SubforumComponent implements OnInit{
    topics : Topic[] = [];
    subforumId : number;
    subforum : Subforum = new Subforum();
    complaintType : string = "Topic";
    entityType : EntityType = EntityType.Topic;
    topicType : string = "";
    topicContent: string = "";
    topicName : string = "";
    topic : Topic = new Topic();
    editBool : boolean = false;
    topicForEdit : Topic = new Topic();
    nameForEdit : string = "";
    contentForEdit : string = "";
    topicTypeForEdit : string = "";

    constructor(private httpSubforumService: SubforumService,private httpComplaintService : ComplaintService
        ,private httpAppUserService : AppUserService ,private httpTopicService : TopicService ,private route: ActivatedRoute) {

    }

    ngOnInit() {
        this.route.params.subscribe(params => {
            this.subforumId = +params['id'] ;  });
            
            this.httpSubforumService.getDatabyId(this.subforumId).subscribe(
                (prod: any) => {this.topics=prod.Topics;this.subforum = prod; console.log(this.topics)},
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

    imageUploaded(event: Event)
    {
        var response = event["serverResponse"].json();
        this.topicContent = response["path"];
        this.contentForEdit = response["path"];
    }

    create()
    {
        if(this.topicName == "" || this.topicContent == "")
        {
            alert("Topic name and content must be filled!");
            return;
        }

        for(var top of this.topics)
        {
            if(this.topicName == top.Name)
            {
                alert("Topic with that name already exists!");
                return;
            }
        }
        this.topic.Name = this.topicName;
        this.topic.Id = this.httpAppUserService.getRandomInt(1,9999999);
        this.topic.AuthorUsername = sessionStorage.getItem("username");
        this.topic.SubforumId = this.subforumId;
        this.topic.Comments = new Array<Comment>();
        this.topic.CreationDate = new Date(Date.now());
        this.topic.DislikesNum = 0;
        this.topic.LikesNum = 0;
        this.topic.UsersWhoVoted = new Array<string>();
        this.topic.Content = this.topicContent;
        if(this.topicType == "Text")
        {
            this.topic.TopicType = TopicType.Text;
            
        }
        else if(this.topicType == "Link")
        {
            this.topic.TopicType = TopicType.Link;
        }
        else if(this.topicType == "Picture")
        {
            this.topic.TopicType = TopicType.Picture;
        }

        this.httpTopicService.put(this.subforumId,this.topic);
        this.topics.push(this.topic);
        this.topicName = "";
        this.topicContent = "";

    }


    Authorized(topic: Topic) : boolean
    {
        if(topic.AuthorUsername == sessionStorage.getItem("username") || "Admin" == sessionStorage.getItem("role")
        || this.subforum.LeadModeratorUsername == sessionStorage.getItem("username"))
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

    forEditTopic(topic : Topic)
    {
        if(this.editBool == false)
            this.editBool = true;
        else
            this.editBool = false;
        
        this.topicForEdit = topic;
        this.nameForEdit = topic.Name;
        this.contentForEdit = topic.Content;
    }

    edit()
    {
        if(this.nameForEdit == "" || this.contentForEdit == "")
        {
            alert("Topic name and content must be filled!");
            return;
        }

        for(var top of this.topics)
        {
            if(this.nameForEdit == top.Name)
            {
                alert("Topic with that name already exists!");
                return;
            }
        }
        
        this.topicForEdit.Name = this.nameForEdit;
        this.topicForEdit.Content = this.contentForEdit;
        if(this.topicType == "Text")
        {
            this.topic.TopicType = TopicType.Text;
            
        }
        else if(this.topicType == "Link")
        {
            this.topic.TopicType = TopicType.Link;
        }
        else if(this.topicType == "Picture")
        {
            this.topic.TopicType = TopicType.Picture;
        }

        this.topics[this.topics.findIndex(x=>x.Id == this.topicForEdit.Id)] = this.topicForEdit;
        this.httpTopicService.put(this.subforumId,this.topicForEdit);
    }

    deleteTopic(topic : Topic)
    {
        this.topics.splice(this.topics.findIndex(x=>x.Id==topic.Id),1);
        this.httpTopicService.delete(topic.Id);
    }
}