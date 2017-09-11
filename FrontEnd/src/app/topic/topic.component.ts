import { Component, OnInit,Input } from '@angular/core';
import {NgForm} from '@angular/forms';
import { NgZone } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import {TopicService} from '../services/topic.service';
import { Subforum } from '../models/subforum.model';
import { Topic } from '../models/topic.model';
import { ActivatedRoute, Params } from '@angular/router';
@Component({
    selector: 'app-topic',
    templateUrl: './topic.component.html',
    providers: [TopicService],
  })

export class TopicComponent implements OnInit{

    topic: Topic;
    topicId : number;
    sub:any;
    constructor(private httpService: TopicService,private route: ActivatedRoute) {

    }

    ngOnInit() {
        this.route.params.subscribe(params => {
            this.topicId = +params['id'] ;  });
            
            this.httpService.getDatabyId(this.topicId).subscribe(
                (prod: any) => {this.topic = prod; console.log(this.topic)},//You can set the type to Product.
                 error => {alert("Unsuccessful fetch operation!"); console.log(error);}); 
            
    }

   
}