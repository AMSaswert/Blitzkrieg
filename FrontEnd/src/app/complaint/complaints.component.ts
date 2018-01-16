import { Component, OnInit,Input } from '@angular/core';
import {NgForm} from '@angular/forms';
import {ComplaintService} from '../services/complaint.service';
import {ComplaintsHelpService} from '../services/complaints-help.service';
import { Complaint,EntityType } from '../models/complaint.model';
import { debug } from 'util';
import { debounce } from 'rxjs/operator/debounce';
@Component({
    selector: 'app-complaints',
    templateUrl: './complaints.component.html',
    providers: [ComplaintService,ComplaintsHelpService]
  })

export class ComplaintsComponent implements OnInit{

    complaints: Complaint[] = [];
    liableComplaints: Complaint[];
    liableUsers : Array<String>;
    types :string[];
    collection : boolean = false;
    //entityType : typeof EntityType = EntityType;
    constructor(private httpComplaintService: ComplaintService,private httpComplaintsHelpService: ComplaintsHelpService ) {
    }


    ngOnInit() {
                
        this.httpComplaintService.getDatabyId(sessionStorage.getItem("username")).subscribe(
            (prod: any) => {this.complaints = prod; console.log(this.complaints)},//You can set the type to Product.
             error => {alert("Unsuccessful fetch operation!"); console.log(error);});
        
        
        
        /*
        var liableUsers = Array<String>();
        var tempComplaints = this.complaints;
        for(var complaint of this.complaints)
          {
            
            this.httpComplaintsHelpService.getDatabyId(complaint.EntityId).subscribe(
              (prod: any) => {liableUsers = prod; console.log(liableUsers)},//You can set the type to Product.
               error => {alert("Unsuccessful fetch operation!"); console.log(error);});
            
            for(var user of liableUsers)
            {
            
                if(user == sessionStorage.getItem("username"))
                {
                  this.liableComplaints.push(complaint);
                  break;
                }
            }
          }
          */
        
    }

    getComplaints() : void
    {
      /*
      for(var complaint of this.complaints)
        {
          
          
          for(var user of this.liableUsers)
          {
          
              if(user == sessionStorage.getItem("username"))
              {
                this.liableComplaints.push(complaint);
                break;
              }
          }
        }
        this.collection = true;
        */
    }



}