import { Component, OnInit,Input } from '@angular/core';
import {NgForm} from '@angular/forms';
import {TopicService} from '../services/topic.service';
import { AppUser } from '../models/appUser.model';
import { Comment } from '../models/comment.model';
import { Topic,TopicType } from '../models/topic.model';
@Component({
    selector: 'app-topics',
    templateUrl: './topics.component.html',
    providers: [TopicService]
  })

export class TopicsComponent implements OnInit{

    topics: Topic[];
    

    constructor(private httpTopicService: TopicService ) {
    }


    ngOnInit() {
                
        this.httpTopicService.getData().subscribe(
            (prod: any) => {this.topics = prod; console.log(this.topics)},//You can set the type to Product.
             error => {alert("Unsuccessful fetch operation!"); console.log(error);});
             
    }

    onSubmit(topic: Topic, form: NgForm) {
        
        
        topic.CreationDate = new Date(Date.now());
        topic.TopicType = TopicType.Text;
        topic.UsersWhoVoted = new Array<AppUser>();
        this.httpTopicService.put(topic.SubforumId,topic);
        form.reset();
        window.location.reload();
        
      }

      edit(topic: Topic, form: NgForm) {
        
        this.httpTopicService.put(topic.SubforumId,topic);
                 
         form.reset();
         window.location.reload();
       }

       delete(topic: Topic, form: NgForm) {
       this.httpTopicService.delete(topic.Id);
                 
         form.reset();
         window.location.reload();
       }

       
    getRandomInt(min, max) {
      return Math.floor(Math.random() * (max - min + 1)) + min;
  }
}