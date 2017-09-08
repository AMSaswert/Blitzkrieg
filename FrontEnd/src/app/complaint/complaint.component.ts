import { Component, OnInit,Input } from '@angular/core';
import {NgForm} from '@angular/forms';
import {ComplaintService} from '../services/complaint.service';
import { Complaint,EntityType } from '../models/complaint.model';
@Component({
    selector: 'app-complaint',
    templateUrl: './complaint.component.html',
    providers: [ComplaintService]
  })

export class ComplaintComponent implements OnInit{

    complaints: Complaint[];

    constructor(private httpComplaintService: ComplaintService ) {
    }


    ngOnInit() {
                
        this.httpComplaintService.getData().subscribe(
            (prod: any) => {this.complaints = prod; console.log(this.complaints)},//You can set the type to Product.
             error => {alert("Unsuccessful fetch operation!"); console.log(error);});
             
    }

    onSubmit(complaint: Complaint, form: NgForm) {

        complaint.CreationDate = new Date(Date.now());
        complaint.EntityType = EntityType.Comment;
        this.httpComplaintService.post(complaint);
        form.reset();
        window.location.reload();
        
      }

      edit(complaint: Complaint, form: NgForm) {
        
        complaint.EntityType = EntityType.Comment;
        this.httpComplaintService.put(complaint.Id,complaint);
        form.reset();
        window.location.reload();
       }

       delete(complaint: Complaint, form: NgForm) {
        this.httpComplaintService.delete(complaint.Id);
        form.reset();
        window.location.reload();
       }
}