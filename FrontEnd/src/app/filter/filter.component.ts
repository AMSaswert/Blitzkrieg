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
@Component({
    selector: 'app-filter',
    templateUrl: './filter.component.html',
    providers: [SubforumService],
  })

export class FilterComponent implements OnInit{

    @Input()  subforums: Subforum[];
    @Input()topics : Topic[] = [];
    subforumId : number;
    //subforum : Subforum = new Subforum();
    complaintType : string = "Topic";
    entityType : EntityType = EntityType.Topic;
    topicType : string = "";
    topicContent: string = "";
    topicName : string = "";
    topic : Topic = new Topic();
    editBool : boolean = false;
    filterEntity : string = "";
    fName : string = "";
    fDescOrCont : string = "";
    fAuthor : string = "";
    fSubforum : string = "";
    appUsers: AppUser[];

    constructor(private httpSubforumService: SubforumService
        ,private httpAppUserService : AppUserService 
        ,private httpTopicService : TopicService ,private route: ActivatedRoute) {

    }

    ngOnInit() {
        if (sessionStorage.getItem("topicRoute") !== null) {
            if (sessionStorage.getItem("topicRoute").length > 1) {
                let quickRoute = sessionStorage.getItem("topicRoute");
                sessionStorage.setItem("topicRoute","");
                this.httpAppUserService.routing(quickRoute);
            }
        }
        this.httpSubforumService.getData().subscribe(
            (prod: any) => {this.subforums = prod;  this.getAllTopics();});

            this.httpAppUserService.getData().subscribe(
                (prod: any) => {this.appUsers = prod; console.log(this.appUsers)},
                 error => {alert("Unsuccessful fetch operation!"); console.log(error);});

    }

    isLoggedIn() : boolean
    {
      return this.httpAppUserService.isLoggedIn();
    }

    routingT(topic: Topic) : void
    {
        this.httpAppUserService.routing("/topic/"+topic.Id.toString()+"/"+topic.SubforumId.toString());
    }

    routingS(subforum: Subforum) : void
    {
        this.httpAppUserService.routing("/subforum/"+ subforum.Id.toString());
    }

    getAllTopics()
    {
        for(var subforum of this.subforums)
            {
                this.topics.push.apply(this.topics, subforum.Topics);
            } 
    }

    getSubforumId(name : string) : number
    {
        for(var subforum of this.subforums)
        {
            if(subforum.Name.indexOf(name) !== -1)
            {
                return subforum.Id;
            }
        } 
    }

    isFilteredS(subforum: Subforum) : boolean
    {
        if ((subforum.Name.toLowerCase().indexOf(this.fName.toLowerCase()) !== -1 || this.fName === "")
        &&(subforum.Description.toLowerCase().indexOf(this.fDescOrCont.toLowerCase()) !== -1 || this.fDescOrCont === "")
        && (subforum.LeadModeratorUsername.toLowerCase().indexOf(this.fAuthor.toLowerCase()) !== -1 || this.fAuthor === ""))
        {
            return true;
        }
        return false;
    }

    isFilteredT(topic: Topic) : boolean
    {
        if ((topic.Name.toLowerCase().indexOf(this.fName.toLowerCase()) !== -1 || this.fName === "")
        &&(topic.Content.toLowerCase().indexOf(this.fDescOrCont.toLowerCase()) !== -1 || this.fDescOrCont === "")
        && (topic.AuthorUsername.toLowerCase().indexOf(this.fAuthor.toLowerCase()) !== -1 || this.fAuthor === "")
        && (topic.SubforumId === this.getSubforumId(this.fSubforum) || this.fSubforum === ""))
        {
            return true;
        }
        return false;
    }

    isFilteredU(appUser: AppUser) : boolean
    {
        if (appUser.UserName.toLowerCase().indexOf(this.fName.toLowerCase()) !== -1 || this.fName === "")
        {
            return true;
        }
        return false;
    }
}