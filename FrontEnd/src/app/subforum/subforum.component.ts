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
import { AppUser } from '../models/appUser.model';
import { AppComponent } from '../app.component';
import { Button } from 'selenium-webdriver';
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
    topicTypeForEdit : string = "";
    topicContent: string = "";
    topicName : string = "";
    topicNameForEdit : string = "";
    topicContentForEdit : string = "";
    topic : Topic = new Topic();
    topicForEdit : Topic = new Topic();
    editBool : boolean = false;
    user : AppUser = new AppUser();
    constructor(private httpSubforumService: SubforumService,private httpComplaintService : ComplaintService
        ,private httpAppUserService : AppUserService ,private httpTopicService : TopicService ,private route: ActivatedRoute) {

    }

    ngOnInit() {
        if (sessionStorage.getItem("topicRoute") !== null) {
            if (sessionStorage.getItem("topicRoute").length > 1) {
                let quickRoute = sessionStorage.getItem("topicRoute");
                sessionStorage.setItem("topicRoute","");
                this.httpAppUserService.routing(quickRoute);
            }
        }
        this.route.params.subscribe(params => {
            this.subforumId = +params['id'] ;  });
            
            this.httpSubforumService.getDatabyId(this.subforumId).subscribe(
                (prod: any) => {this.topics=prod.Topics;this.subforum = prod; console.log(this.topics)},
                 error => {alert("Unsuccessful fetch operation!"); console.log(error);}); 

            if(this.isLoggedIn())
            {

            this.httpAppUserService.getDataById(sessionStorage.getItem("username")).subscribe(
                (prod: any) => {this.user = prod; console.log(this.user)},
                 error => {alert("Unsuccessful fetch operation!"); console.log(error);}); 
            }
            
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
        this.httpAppUserService.routing("/topic/"+topic.Id.toString()+"/"+topic.SubforumId.toString());
    }

    imageUploaded(event: Event)
    {
        var response = event["serverResponse"].json();
        this.topicContent = response["path"];
    }

    create()
    {
        if(this.topicType == "")
        {
            alert("Chose type of topic!");
            return;
        }

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

        this.topic.Name = this.topicName;
        this.topic.Content = this.topicContent;

        this.topic.Id = this.httpAppUserService.getRandomInt(1,9999999);
        this.topic.AuthorUsername = sessionStorage.getItem("username");
        this.topic.SubforumId = this.subforumId;
        this.topic.Comments = new Array<Comment>();
        this.topic.CreationDate = new Date(Date.now());
        this.topic.DislikesNum = 0;
        this.topic.LikesNum = 0;
        this.topic.UsersWhoVoted = new Array<string>();
        
        this.httpTopicService.put(this.subforumId,this.topic);
        this.topics.push(this.topic);
        this.topicName = "";
        this.topicContent = "";
    }

    edit()
    {
        if(this.topicTypeForEdit == "")
        {
            alert("Chose type of topic!");
            return;
        }

        if(this.topicNameForEdit == "" || this.topicContentForEdit == "")
        {
            alert("Topic name and content must be filled!");
            return;
        }

        for(var top of this.topics)
        {
            if(this.topicNameForEdit == top.Name)
            {
                alert("Topic with that name already exists!");
                return;
            }
        }
        if(this.topicTypeForEdit == "Text")
        {
            this.topicForEdit.TopicType = TopicType.Text;
            
        }
        else if(this.topicTypeForEdit == "Link")
        {
            this.topicForEdit.TopicType = TopicType.Link;
        }
        else if(this.topicTypeForEdit == "Picture")
        {
            this.topicForEdit.TopicType = TopicType.Picture;
        }

        this.topicForEdit.Name = this.topicNameForEdit;
        this.topicForEdit.Content = this.topicContentForEdit;
        this.topics[this.topics.findIndex(x=>x.Id == this.topicForEdit.Id)] = this.topicForEdit;
        this.httpTopicService.put(this.subforumId,this.topic);
        this.topicNameForEdit = "";
        this.topicContentForEdit = "";
        
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

    forEditTopic(topic : Topic) : void
    {
        if(this.editBool == false)
        {
            this.editBool = true;
            this.topicNameForEdit = topic.Name;
            this.topicContentForEdit = topic.Content;
            this.topicForEdit = topic;
            this.topicType = "";
        }
        else
        {
            this.editBool = false;
            this.topicTypeForEdit = "";
        }    

        
    }

    deleteTopic(topic : Topic) : void
    {
        this.httpTopicService.delete(topic.Id).subscribe(
            data => {
              alert("Topic is deleted.");
          },
          error => {
              alert("Topic is already deleted.");
          });
        this.topics.splice(this.topics.findIndex(x=>x.Id==topic.Id),1);
        
    }

    saveTopic(topic : Topic) : void
    {
        this.user.SavedTopics.push(topic);
        this.httpAppUserService.put(this.user.Id,this.user);
    }
    resetContent() : void
    {
        this.topicContent = "";
        this.topicContentForEdit = "";
    }
}