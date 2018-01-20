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
@Component({
    selector: 'app-subforums',
    templateUrl: './subforums.component.html',
    providers: [SubforumService],
  })

export class SubforumsComponent implements OnInit{

@Input()  subforums: Subforum[];

    complaintType : string = "Subforum";
    entityType : EntityType = EntityType.Subforum;

    constructor(private httpSubforumService: SubforumService,private httpComplaintService: ComplaintService
    ,private httpAppUserService : AppUserService ) {

    }

    ngOnInit() {

            this.httpSubforumService.getData().subscribe(
                (prod: any) => {this.subforums = prod;});
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



}