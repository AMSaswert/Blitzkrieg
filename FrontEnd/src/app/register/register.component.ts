import { Component, OnInit,Input } from '@angular/core';
import {NgForm} from '@angular/forms';
import {AppUserService} from '../services/appUser.service';
import { AppUser } from '../models/appUser.model';
import { Message } from '../models/message.model';
import { Router } from '@angular/router';
@Component({
    selector: 'app-register',
    templateUrl: './register.component.html',
    providers: [AppUserService]
  })

export class RegisterComponent{

    appUsers: AppUser[];
    
    constructor(private httpAppUserService: AppUserService,private router: Router ) {
    }

    onSubmit(user: AppUser, form: NgForm) {

        user.Id = this.httpAppUserService.getRandomInt(1,9999999);
        user.Role = "AppUser";
        user.BookmarkedSubforums = new Array<number>();
        user.SavedTopics = new Array<number>();
        user.SavedComments = new Array<number>();
        user.ReceivedMessages = new Array<Message>();
        user.RegistrationDate = new Date(Date.now());
        var us;
        this.httpAppUserService.post(user).subscribe(
            data => {
                alert("Registration successful");
                this.router.navigate(['/login']);
            },
            error => {
                alert("Username already exists.");
            });
        
      }      
}