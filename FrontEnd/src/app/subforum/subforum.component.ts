import { Component, OnInit,Input } from '@angular/core';
import {NgForm} from '@angular/forms';
import {SubforumService} from '../services/subforum.service';
import { Subforum } from '../models/subforum.model';
import { Topic } from '../models/topic.model';
@Component({
    selector: 'app-subforum',
    templateUrl: './subforum.component.html',
    providers: [SubforumService]
  })

export class SubforumComponent implements OnInit{

    subforums: Subforum[];
    

    constructor(private httpSubforumService: SubforumService ) {
    }


    ngOnInit() {
                
        this.httpSubforumService.getData().subscribe(
            (prod: any) => {this.subforums = prod; console.log(this.subforums)},//You can set the type to Product.
             error => {alert("Unsuccessful fetch operation!"); console.log(error);});
             
    }

    onSubmit(subforum: Subforum, form: NgForm) {
        
        subforum.Moderators = new Array<string>();
        subforum.Topics = new Array<Topic>();
        this.httpSubforumService.post(subforum);
        form.reset();
        window.location.reload();
        
      }

      edit(subforum: Subforum, form: NgForm) {
        
         this.httpSubforumService.put(subforum.Id,subforum);        
         form.reset();
         window.location.reload();
       }

       delete(subforum: Subforum, form: NgForm) {
        
         this.httpSubforumService.delete(subforum.Id);
         form.reset();
         window.location.reload();
       }
}