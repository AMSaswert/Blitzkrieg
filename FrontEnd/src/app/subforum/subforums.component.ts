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
        
        subforum.Id = this.getRandomInt(1,9999999);
        subforum.Moderators = new Array<string>();
        subforum.Topics = new Array<Topic>();
        subforum.LeadModeratorUsername = localStorage.getItem("username");
        this.httpSubforumService.post(subforum);
        form.reset();
        
        this.httpSubforumService.getData().subscribe(
            (prod: any) => {this.subforums = prod;});
        

       // window.location.reload();
        
      }

      edit(subforum: Subforum, form: NgForm) {
        
         this.httpSubforumService.put(subforum.Id,subforum);        
         form.reset();
       //  window.location.reload();
       }

       delete(subforum: Subforum, form: NgForm) {
        
         this.httpSubforumService.delete(subforum.Id);
         form.reset();
       //  window.location.reload();
       }

       

       isLoggedIn() : boolean
       {
         return this.httpAppUserService.isLoggedIn();
       }

       getRandomInt(min, max) {
        return Math.floor(Math.random() * (max - min + 1)) + min;
    }

    authRole() : boolean
      {
        if("Admin" == sessionStorage.getItem("role") || "Moderator" == sessionStorage.getItem("role"))
          return true;
        return false;
      }
}