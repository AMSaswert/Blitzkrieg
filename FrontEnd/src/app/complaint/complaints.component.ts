import { Component, OnInit,Input } from '@angular/core';
import {NgForm} from '@angular/forms';
import {ComplaintService} from '../services/complaint.service';
import {ComplaintsHelpService} from '../services/complaints-help.service';
import { Complaint,EntityType } from '../models/complaint.model';
import { debug } from 'util';
import { debounce } from 'rxjs/operator/debounce';
import {AppUserService} from '../services/appUser.service';
import {MessageService} from '../services/message.service';
import {TopicService} from '../services/topic.service';
import {CommentService} from '../services/comment.service';
import {SubforumService} from '../services/subforum.service';
import { Message } from '../models/message.model';

@Component({
    selector: 'app-complaints',
    templateUrl: './complaints.component.html',
    providers: [ComplaintService,ComplaintsHelpService,
        SubforumService,TopicService,CommentService,MessageService]
  })

export class ComplaintsComponent implements OnInit{


    complaints: Complaint[] = [];
    entityType :string[];
    usernameAndRole : string = sessionStorage.getItem("username") +"-"+ sessionStorage.getItem("role");
    complaintType : string = "";
    complaintId : number = -1;
    textForAuthor : string = "";
    textForComplainer : string = "";
    messageForAuthor : Message = new Message();
    messageForComplainer : Message = new Message();
    complaint : Complaint = new Complaint();
    counter : number = 0;
    
    constructor(private httpComplaintService: ComplaintService,private httpComplaintsHelpService: ComplaintsHelpService,
        private httpMessageService: MessageService,  private httpSubforumService: SubforumService,
        private httpTopicService: TopicService,
        private httpCommentService: CommentService,private httpAppUserService : AppUserService) {
    }


    ngOnInit() {
                
        this.httpComplaintService.getDatabyId(this.usernameAndRole).subscribe(
            (prod: any) => {this.complaints = prod; console.log(this.complaints)},
             error => {alert("Unsuccessful fetch operation!"); console.log(error);});


        var options = Object.keys(EntityType);
        this.entityType = options.slice(options.length/2);
    }

    getComplaint(complaint : Complaint,complaintType: string) : void
    {
        this.complaintId = complaint.Id;
        this.complaint = complaint;
        this.complaintType = complaintType;
    }


    delete() : void
    {   if (this.textForAuthor !== "" || this.textForComplainer !== "") {
           
            if(this.complaint.EntityType == EntityType.Subforum)
            {
                this.httpSubforumService.delete(this.complaint.EntityId).subscribe(
                    data => {
                      alert("Subforum is deleted.");
                      this.messagesAndDeleteComplaint();
                  },
                  error => {
                      alert("Subforum is already deleted.");
                      
                  });
            }
            else if(this.complaint.EntityType == EntityType.Topic)
            {
                this.httpTopicService.delete(this.complaint.EntityId).subscribe(
                    data => {
                      alert("Topic is deleted.");
                      this.messagesAndDeleteComplaint();
                  },
                  error => {
                      alert("Topic is already deleted.");
                      
                  });
            }
            else
            {
                this.httpCommentService.delete(this.complaint.EntityId).subscribe(
                    data => {
                      alert("Comment is deleted.");
                      this.messagesAndDeleteComplaint();
                  },
                  error => {
                      alert("Comment is already deleted.");
                  });
            }
           
        } else {
            alert("Field cannot be empty!");
        }
    }

    warn() : void
    { 
        if (this.textForAuthor !== "" || this.textForComplainer !== "") {
            this.messageConstructorForAuthor()
            this.messageConstructorForComplainer();
            this.deleteComplaint();
        } else {
            alert("Field cannot be empty!");
        }
    }
    
    refuse() : void
    {
        if (this.textForComplainer !== "") {
            this.messageConstructorForComplainer();
            this.deleteComplaint();
        } else {
            alert("Field cannot be empty!");
        }
    }

    messageConstructorForAuthor()
    {
        this.messageForAuthor.Id = this.httpAppUserService.getRandomInt(1,9999999);
        this.messageForAuthor.Content = this.textForAuthor;
        this.messageForAuthor.SenderUsername = sessionStorage.getItem("username");
        this.messageForAuthor.RecipientUsername = this.complaint.EntityAuthor;
        this.httpMessageService.create(this.complaint.EntityAuthor,this.messageForAuthor);
    }

    messageConstructorForComplainer()
    {
        this.messageForComplainer.Id = this.httpAppUserService.getRandomInt(1,9999999);      
        this.messageForComplainer.Content = this.textForComplainer;        
        this.messageForComplainer.SenderUsername = sessionStorage.getItem("username");
        this.messageForComplainer.RecipientUsername = this.complaint.AuthorUsername;
        this.httpMessageService.create(this.complaint.AuthorUsername,this.messageForComplainer);
    }

    deleteComplaint()
    {
        this.httpComplaintService.delete(this.complaint.Id);
        this.complaints.splice(this.complaints.findIndex(x=>x.Id==this.complaint.Id),1);
        this.textForAuthor = "";
        this.textForComplainer = "";
        this.complaintId = -1;
    }

    messagesAndDeleteComplaint()
    {
        this.messageConstructorForAuthor();
        this.messageConstructorForComplainer();
        this.deleteComplaint();
    }

}