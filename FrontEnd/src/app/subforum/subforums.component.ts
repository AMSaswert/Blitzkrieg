import { Component, OnInit,Input } from '@angular/core';
import {NgForm} from '@angular/forms';
import { NgZone } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import {SubforumService} from '../services/subforum.service';
import {ComplaintService} from '../services/complaint.service';
import {AppUserService} from '../services/appUser.service';
import { Subforum } from '../models/subforum.model';
import { Topic } from '../models/topic.model';
import { Complaint,EntityType } from '../models/complaint.model';
import { ChangeDetectionStrategy } from '@angular/core';
import { Router } from '@angular/router';
import { AppUser } from '../models/appUser.model';
@Component({
    selector: 'app-subforums',
    templateUrl: './subforums.component.html',
    providers: [SubforumService],
  })

export class SubforumsComponent implements OnInit{

@Input()  subforums: Subforum[];

    complaintType : string = "Subforum";
    entityType : EntityType = EntityType.Subforum;
    subIcon : string = "";
    user : AppUser = new AppUser();

    constructor(private httpSubforumService: SubforumService,private httpComplaintService: ComplaintService
    ,private httpAppUserService : AppUserService ) {

    }

    ngOnInit() {

            this.httpSubforumService.getData().subscribe(
                (prod: any) => {this.subforums = prod;});

            if(this.isLoggedIn())
            {

            this.httpAppUserService.getDataById(sessionStorage.getItem("username")).subscribe(
                (prod: any) => {this.user = prod; console.log(this.user)},
                  error => {alert("Unsuccessful fetch operation!"); console.log(error);}); 
            }
    }

    onSubmit(subforum: Subforum, form: NgForm) {
        

        for(var sub of this.subforums)
        {
          if(sub.Name == subforum.Name)
          {
            alert("Subforum with that name already exists!");
            subforum.Name = "";
            return;
          }
        }

        subforum.Id = this.httpAppUserService.getRandomInt(1,9999999);
        subforum.Moderators = new Array<string>();
        subforum.Topics = new Array<Topic>();
        subforum.LeadModeratorUsername = sessionStorage.getItem("username");
        subforum.IconURL = this.subIcon;
        this.httpSubforumService.post(subforum);
        form.reset();
        
        this.subforums.push(subforum);
        
      }
      
      deleteSubforum(subforumId: number) : void
      {
        this.httpSubforumService.delete(subforumId);
        this.subforums.splice(this.subforums.findIndex(x=> x.Id == subforumId),1);
      }

       isLoggedIn() : boolean
       {
         return this.httpAppUserService.isLoggedIn();
       }

      authRole() : boolean
      {
        if("Admin" == sessionStorage.getItem("role") || "Moderator" == sessionStorage.getItem("role"))
          return true;
        return false;
      }

      isCreatorOrAdmin(username: string) : boolean
      {
        if(sessionStorage.getItem("role") == "Admin")
        {
          return true;
        }
        else if(username == sessionStorage.getItem("username"))
        {
          return true;
        }

        return false;
      }

      routing(subforum: Subforum) : void
      {
        this.httpAppUserService.routing("/subforum/"+ subforum.Id.toString());
      }

      imageUploaded(event: Event)
    {
        var response = event["serverResponse"].json();
        this.subIcon = response["path"];
    }

    bookmarked(subforumId : number) : boolean
    {
      for(var bookmarked of this.user.BookmarkedSubforums)
      {
        if(subforumId == bookmarked)
        {
          return true;
        }
      }
      
      return false;
    }

    bookmarkSubforum(subforumId : number) : void
    {
      this.user.BookmarkedSubforums.push(subforumId);
      this.httpAppUserService.put(this.user.Id,this.user);
    }
}