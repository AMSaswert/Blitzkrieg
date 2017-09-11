import { Component, OnInit,Input } from '@angular/core';
import {NgForm} from '@angular/forms';
import {MessageService} from '../services/message.service';
import {AppUserService} from '../services/appUser.service';
import { Message } from '../models/message.model';
import { AppUser } from '../models/appUser.model';
@Component({
    selector: 'app-message-send',
    templateUrl: './message-send.component.html',
    providers: [MessageService,AppUserService]
  })

export class MessageComponent implements OnInit{

    messages: Message[];
    appusers : AppUser[];

    constructor(private httpMessageService: MessageService,private httpAppUserService : AppUserService) {
    }


    ngOnInit() {

      this.httpAppUserService.getDatabyId().subscribe(
        (prod: any) => {this.appusers = prod; console.log(this.appusers)},//You can set the type to Product.
         error => {alert("Unsuccessful fetch operation!"); console.log(error);});
        
    }

    onSubmit(msg: Message, form: NgForm) {
        msg.Id = this.getRandomInt(1,9999999);
        var id = Number.parseInt((<HTMLInputElement>document.getElementById("recipient")).value);
        this.httpMessageService.create(id,msg);

        form.reset();
        window.location.reload();
        
      }

    getRandomInt(min, max) {
        return Math.floor(Math.random() * (max - min + 1)) + min;
    }
}