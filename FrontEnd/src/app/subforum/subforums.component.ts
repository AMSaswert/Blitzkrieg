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

complaint : Complaint = new Complaint(this.getRandomInt(1,9999999));
complaintText: string = "";


    constructor(private httpSubforumService: SubforumService,private httpComplaintService: ComplaintService
    ,private httpAppUserService : AppUserService ) {

    }

    ngOnInit() {
        
            this.httpSubforumService.getData().subscribe(
                (prod: any) => {this.subforums = prod;});
    }

    onSubmit(subforum: Subforum, form: NgForm) {
        
        subforum.Moderators = new Array<string>();
        subforum.Topics = new Array<Topic>();
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

       complaintSend() : void
       {
           
            this.complaint.EntityType = EntityType.Subforum;
            this.complaint.AuthorUsername = sessionStorage.getItem("username");
            this.complaint.CreationDate = new Date(Date.now());
            this.complaint.EntityId = Number.parseInt((<HTMLInputElement>document.getElementById("complaintsub")).value);
            this.complaint.Text = this.complaintText;
            
            this.httpComplaintService.post(this.complaint);
       }

       isLoggedIn() : boolean
       {
         return this.httpAppUserService.isLoggedIn();
       }

       getRandomInt(min, max) {
        return Math.floor(Math.random() * (max - min + 1)) + min;
    }
}