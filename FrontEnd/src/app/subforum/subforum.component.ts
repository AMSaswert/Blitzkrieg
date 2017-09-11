import { Component, OnInit,Input } from '@angular/core';
import {NgForm} from '@angular/forms';
import { NgZone } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import {SubforumService} from '../services/subforum.service';
import { Subforum } from '../models/subforum.model';
import { Topic } from '../models/topic.model';
import { ActivatedRoute, Params } from '@angular/router';
@Component({
    selector: 'app-subforum',
    templateUrl: './subforum.component.html',
    providers: [SubforumService],
  })

export class SubforumComponent implements OnInit{

    subforum : Subforum;
    topics : Topic[] = [];
    subforumId : number;
    sub:any;
    constructor(private httpSubforumService: SubforumService,private route: ActivatedRoute) {

    }

    ngOnInit() {
        this.route.params.subscribe(params => {
            this.subforumId = +params['id'] ;  });
            
            this.httpSubforumService.getDatabyId(this.subforumId).subscribe(
                (prod: any) => {this.subforum = prod; console.log(this.subforum)},//You can set the type to Product.
                 error => {alert("Unsuccessful fetch operation!"); console.log(error);}); 
            
    }

   
}