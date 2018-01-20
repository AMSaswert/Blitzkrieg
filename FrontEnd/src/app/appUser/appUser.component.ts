import { Component, OnInit,Input } from '@angular/core';
import {NgForm} from '@angular/forms';
import {AppUserService} from '../services/appUser.service';
import { AppUser } from '../models/appUser.model';
import { Message } from '../models/message.model';
@Component({
    selector: 'app-appuser',
    templateUrl: './appUser.component.html',
    providers: [AppUserService]
  })

export class AppUserComponent implements OnInit{

    appUsers: AppUser[];
    

    constructor(private httpAppUserService: AppUserService ) {
    }
    
    ngOnInit() {
                
        this.httpAppUserService.getData().subscribe(
            (prod: any) => {this.appUsers = prod; console.log(this.appUsers)},
             error => {alert("Unsuccessful fetch operation!"); console.log(error);});
             
    }

    changeTypeOfUser(user : AppUser) : void
    {
      if(user.Role == "AppUser")
      {
          user.Role = "Moderator";
          this.httpAppUserService.put(user.Id,user);
          this.appUsers[this.appUsers.findIndex(x=> x.UserName == user.UserName)].Role = "Moderator";
      }
      else if(user.Role == "Moderator")
      {
          user.Role = "Admin";
          this.httpAppUserService.put(user.Id,user);
          this.appUsers[this.appUsers.findIndex(x=> x.UserName == user.UserName)].Role = "Admin";
      }
    }
}