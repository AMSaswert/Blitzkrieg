import { Component, OnInit,Input } from '@angular/core';
import {NgForm} from '@angular/forms';
import {MessageService} from '../services/message.service';
import {AppUserService} from '../services/appUser.service';
import { Message } from '../models/message.model';
import { AppUser } from '../models/appUser.model';
import { ActivatedRoute, Params, Router } from '@angular/router';
@Component({
    selector: 'app-message-send',
    templateUrl: './message-send.component.html',
    providers: [MessageService,AppUserService]
  })

export class MessageSendComponent implements OnInit{

    message : Message = new Message();
    appusers : AppUser[];
    messageText : string = "";
    currentUser : string = sessionStorage.getItem("username");
    constructor(private httpMessageService: MessageService,private httpAppUserService : AppUserService,
      private router : Router) {
    }


    ngOnInit() {

      this.httpAppUserService.getData().subscribe(
        (prod: any) => {this.appusers = prod; 
            for(var user of this.appusers)
            {
              if(user.UserName == sessionStorage.getItem("username"))
              {
                this.appusers.splice(this.appusers.findIndex(x=>x.UserName == user.UserName),1);
                break;
              }
            }         
          console.log(this.appusers)},
         error => {alert("Unsuccessful fetch operation!"); console.log(error);});
        
    }

    sendMessage() : void
    {
      if (this.messageText !== "") {
        this.message.Id = this.httpAppUserService.getRandomInt(1,9999999);
        this.message.Content = this.messageText;
        this.message.SenderUsername = sessionStorage.getItem("username");
        this.message.Seen = false;
        var username = (<HTMLInputElement>document.getElementById("recipient")).value;
        this.httpMessageService.create(username,this.message);
        this.httpAppUserService.routing("/home");;
      } else {
        alert("Field cannot be empty!")
      }
    }
}