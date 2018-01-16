import { Component, OnInit,Input } from '@angular/core';
import {NgForm} from '@angular/forms';
import {CommentService} from '../services/comment.service';
import {AppUserService} from '../services/appUser.service';
import {ComplaintService} from '../services/complaint.service';
import { AppUser } from '../models/appUser.model';
import { Comment } from '../models/comment.model';
import { Subforum } from '../models/subforum.model';
import { Topic } from '../models/topic.model';
import { Complaint,EntityType } from '../models/complaint.model';
@Component({
    selector: 'complaint-send',
    templateUrl: './complaint-send.component.html',
  })

export class ComplaintSend {

    constructor(private httpComplaintService: ComplaintService,private httpAppUserService: AppUserService) {
    
        }

    @Input() complaintType : String;
    @Input() complaningTo: any; 
    @Input() entityType : EntityType;
    complaint : Complaint = new Complaint();
    complaintText: string = "";


    complaintSend() : void
       {
            this.complaint.Id = this.getRandomInt(1,9999999);
            this.complaint.EntityType = this.entityType;
            this.complaint.AuthorUsername = sessionStorage.getItem("username").toString();
            this.complaint.CreationDate = new Date(Date.now());
            this.complaint.EntityId = this.complaningTo.Id;
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