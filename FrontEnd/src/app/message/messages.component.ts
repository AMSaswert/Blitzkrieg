import { Component, OnInit,Input } from '@angular/core';
import {NgForm} from '@angular/forms';
import {MessageService} from '../services/message.service';
import { Message } from '../models/message.model';
@Component({
    selector: 'app-messages',
    templateUrl: './messages.component.html',
    providers: [MessageService]
  })

export class MessagesComponent implements OnInit{

    messages: Message[];

    constructor(private httpMessageService: MessageService) {
    }


    ngOnInit() {
        
        
        this.httpMessageService.getDatabyId(1956481846).subscribe(
            (prod: any) => {this.messages = prod; console.log(this.messages)},//You can set the type to Product.
             error => {alert("Unsuccessful fetch operation!"); console.log(error);});
    }

    onSubmit(msg: Message, form: NgForm) {
        this.httpMessageService.create(1956481846,msg);
        
        form.reset();
        window.location.reload();
        
      }

      edit(msg: Message, form: NgForm) {
       
        msg.Seen = true;
        this.httpMessageService.update(1956481846,msg);
                
        form.reset();
        window.location.reload();
      }

      delete(msg: Message, form: NgForm) {
        this.httpMessageService.delete(msg);
        
        form.reset();
        window.location.reload();
      }


}