import { Component, OnInit,Input } from '@angular/core';
import {NgForm} from '@angular/forms';
import {ComplaintService} from '../services/complaint.service';
import {ComplaintsHelpService} from '../services/complaints-help.service';
import { Complaint,EntityType } from '../models/complaint.model';
import { ActivatedRoute, Params } from '@angular/router';
@Component({
    selector: 'app-complaint',
    templateUrl: './complaint.component.html',
    providers: [ComplaintService,ComplaintsHelpService]
  })

export class ComplaintComponent implements OnInit{

    complaint: Complaint;
    complaintId : number;
    deleteComplaint : boolean = false;
    warningComplaint : boolean = false;
    refuseComplaint : boolean = false;

    constructor(private httpComplaintService: ComplaintService,private httpComplaintsHelpService: ComplaintsHelpService,
        private route: ActivatedRoute ) {
    }


    ngOnInit() {
        
        this.route.params.subscribe(params => {
            this.complaintId = +params['id'] ;  });
        

        this.httpComplaintService.getDatabyId(this.complaintId).subscribe(
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

    }

    warn() : void
    {
        
    }
    
    refuse() : void
    {

    }

   
}