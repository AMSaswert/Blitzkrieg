import { Component, OnInit,Input } from '@angular/core';
import {NgForm} from '@angular/forms';
import {ComplaintService} from '../services/complaint.service';
import {ComplaintsHelpService} from '../services/complaints-help.service';
import { Complaint,EntityType } from '../models/complaint.model';
import { ActivatedRoute, Params, Router } from '@angular/router';
import {MessageService} from '../services/message.service';
import {TopicService} from '../services/topic.service';
import {SubforumService} from '../services/subforum.service';
import {CommentService} from '../services/comment.service';
import { Message } from '../models/message.model';
import { Subforum } from '../models/subforum.model';

@Component({
    selector: 'app-complaint',
    templateUrl: './complaint.component.html',
    providers: [ComplaintService,ComplaintsHelpService,
    SubforumService,TopicService,CommentService,MessageService]
  })

export class ComplaintComponent implements OnInit{

    complaint: Complaint;
    complaintId : number;
    deleteComplaint : boolean = false;
    warningComplaint : boolean = false;
    refuseComplaint : boolean = false;
    textForAuthor : string = "";
    textForComplainer : string = "";
    messageForAuthor : Message = new Message();
    messageForComplainer : Message = new Message();
    AuthorId : number;
    ComplainerId: number;

    constructor(private httpComplaintService: ComplaintService,private httpComplaintsHelpService: ComplaintsHelpService,
        private httpMessageService: MessageService,private route: ActivatedRoute,
        private httpSubforumService: SubforumService,private httpTopicService: TopicService,
        private httpCommentService: CommentService, private router : Router ) {
    }


    ngOnInit() {
        
        this.route.params.subscribe(params => {
            this.complaintId = +params['id'] ;  });
        

        this.httpComplaintsHelpService.getDatabyId(this.complaintId).subscribe(
            (prod: any) => {this.complaint=prod; console.log(this.complaint)},//You can set the type to Product.
            error => {alert("Unsuccessful fetch operation!"); console.log(error);});        
    }


    deleteBool() : void
    {
        this.deleteComplaint = true;
        this.warningComplaint = false;
        this.refuseComplaint = false;
    }

    warningBool() : void
    {
        this.deleteComplaint = false;
        this.warningComplaint = true;
        this.refuseComplaint = false;
    }

    refuseBool() : void
    {
        this.deleteComplaint = false;
        this.warningComplaint = false;
        this.refuseComplaint = true;
    }

    delete() : void
    {      
        this.messageConstructorForAuthor()
        this.messageConstructorForComplaier();
        if(this.complaint.EntityType == EntityType.Subforum)
        {
            this.httpSubforumService.delete(this.complaint.EntityId);
        }
        else if(this.complaint.EntityType == EntityType.Topic)
        {
            this.httpTopicService.delete(this.complaint.EntityId);
        }
        else
        {
            this.httpCommentService.delete(this.complaint.EntityId);
        }
        this.httpComplaintService.delete(this.complaint.Id);
        this.router.navigate(['/complaints']);

    }

    warn() : void
    { 
        this.messageConstructorForAuthor()
        this.messageConstructorForComplaier();
        this.httpComplaintService.delete(this.complaint.Id);
        this.router.navigate(['/complaints']);

    }
    
    refuse() : void
    {
        this.messageConstructorForComplaier();
        this.httpComplaintService.delete(this.complaint.Id);
        this.router.navigate(['/complaints']);

    }

    getRandomInt(min, max) {
        return Math.floor(Math.random() * (max - min + 1)) + min;
    }

    messageConstructorForAuthor()
    {
        this.messageForAuthor.Id = this.getRandomInt(1,9999999);
        this.messageForAuthor.Content = this.textForAuthor;
        this.messageForAuthor.SenderUsername = sessionStorage.getItem("username");
        this.messageForAuthor.RecipientUsername = this.complaint.EntityAuthor;
        this.httpMessageService.create(this.complaint.EntityAuthor,this.messageForAuthor);
    }

    messageConstructorForComplaier()
    {
        this.messageForComplainer.Id = this.getRandomInt(1,9999999);      
        this.messageForComplainer.Content = this.textForComplainer;        
        this.messageForComplainer.SenderUsername = sessionStorage.getItem("username");
        this.messageForComplainer.RecipientUsername = this.complaint.AuthorUsername;
        this.httpMessageService.create(this.complaint.AuthorUsername,this.messageForComplainer);
    }
   
}