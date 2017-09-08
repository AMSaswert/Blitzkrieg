import { Component, OnInit,Input } from '@angular/core';
import {NgForm} from '@angular/forms';
import {MessageService} from '../services/message.service';
import { Message } from '../models/message.model';
@Component({
    selector: 'app-message',
    templateUrl: './message.component.html',
    providers: [MessageService]
  })

export class MessageComponent implements OnInit{

    messages: Message[];

    constructor(private httpMessageService: MessageService) {
    }


    ngOnInit() {
        
        
       /* this.httpMessageService.getDatabyId(1861725323).subscribe(
            (prod: any) => {this.messages = prod; console.log(this.messages)},//You can set the type to Product.
             error => {alert("Unsuccessful fetch operation!"); console.log(error);});*/
    }

    onSubmit(msg: Message, form: NgForm) {
        this.httpMessageService.create(1861725323,msg);
        
        form.reset();
        window.location.reload();
        
      }

      edit(msg: Message, form: NgForm) {
       
        msg.Seen = true;
        this.httpMessageService.update(1861725323,msg);
                
        form.reset();
        window.location.reload();
      }

      delete(msg: Message, form: NgForm) {
        this.httpMessageService.delete(msg);
        
        form.reset();
        window.location.reload();
      }


}