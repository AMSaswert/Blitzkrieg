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
    usernameAndRole : string = sessionStorage.getItem("username") +"-"+ sessionStorage.getItem("role");
    constructor(private httpComplaintService: ComplaintService,private httpComplaintsHelpService: ComplaintsHelpService ) {
    }


    ngOnInit() {
                
        this.httpComplaintService.getDatabyId(this.usernameAndRole).subscribe(
            (prod: any) => {this.complaints = prod; console.log(this.complaints)},//You can set the type to Product.
             error => {alert("Unsuccessful fetch operation!"); console.log(error);});
    }


}