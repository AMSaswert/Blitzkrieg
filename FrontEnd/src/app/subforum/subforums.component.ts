import { Component, OnInit,Input } from '@angular/core';
import {NgForm} from '@angular/forms';
import { NgZone } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import {SubforumService} from '../services/subforum.service';
import { Subforum } from '../models/subforum.model';
import { Topic } from '../models/topic.model';
import { Complaint,EntityType } from '../models/complaint.model';
import { ChangeDetectionStrategy } from '@angular/core';
import { Router } from '@angular/router';
@Component({
    selector: 'app-subforums',
    templateUrl: './subforums.component.html',
    //template: `<app-subforum> [subforums]="subforums"</app-subforum>`,
    providers: [SubforumService],
    //changeDetection: ChangeDetectionStrategy.OnPush
  })

export class SubforumsComponent implements OnInit{

@Input()  subforums: Subforum[];

complaint : Complaint;
authorUsername: string;
entityId: number;


    constructor(private httpSubforumService: SubforumService,private router:Router) {

    }

    ngOnInit() {
        
            this.httpSubforumService.getData().subscribe(
                (prod: any) => {this.subforums = prod;});
           /* this.pushArray();
            var promise = new Promise((resolve)=>{
                resolve();
                console.log("promise hit");
            });
            
            promise.then(x=> {
                this.pushArray();
                this.subforums = this.subforums.slice();
            });

            this.httpSubforumService.getData().subscribe(
                (prod: any) => {this.subforums = prod; console.log(this.subforums)},//You can set the type to Product.
                 error => {alert("Unsuccessful fetch operation!"); console.log(error);}); 
                 */
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
       /*
       redirect()
       {
        var id = Number.parseInt((<HTMLInputElement>document.getElementById("subforumId")).value);
        
        this.router.navigate(['/subforum-topics',id]);
       }*/

       sendComplaint()
       {
        
       }
}