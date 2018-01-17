import { Component, OnInit,Input } from '@angular/core';
import {NgForm} from '@angular/forms';
import {MessageService} from '../services/message.service';
import {AppUserService} from '../services/appUser.service';
import { Message } from '../models/message.model';
import { AppUser } from '../models/appUser.model';
@Component({
    selector: 'app-message-received',
    templateUrl: './message-received.component.html',
    providers: [MessageService,AppUserService]
  })

export class MessageReceivedComponent implements OnInit{

    messages: Message[] = [];

    constructor(private httpMessageService: MessageService,private httpAppUserService : AppUserService) {
    }

    ngOnInit() {
        
        if(this.httpAppUserService.isLoggedIn())
            {
                
                this.httpMessageService.getDatabyId(sessionStorage.getItem("username")).subscribe(
                    (prod: any) => {this.messages = prod; console.log(this.messages)},//You can set the type to Product.
                     error => {alert("Unsuccessful fetch operation!"); console.log(error);});
            }
        
    }
}