import { Component, OnInit,Input } from '@angular/core';
import {NgForm} from '@angular/forms';
import {AppUserService} from '../services/appUser.service';
import { AppUser } from '../models/appUser.model';
import { Message } from '../models/message.model';
import { Topic } from '../models/topic.model';
import { Comment } from '../models/comment.model';
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
        user.SavedTopics = new Array<Topic>();
        user.SavedComments = new Array<Comment>();
        user.ReceivedMessages = new Array<Message>();
        user.RegistrationDate = new Date(Date.now());
        var us;
        if (user.UserName !== "" && user.Password !== "" && user.Name !== "" && user.Surname !== "" &&
        /^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/.test(user.Email) && 
        /^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$/.test(user.ContactPhone)) {
            this.httpAppUserService.post(user).subscribe(
                data => {
                    alert("Registration successful");
                    this.httpAppUserService.routing("/home");
                },
                error => {
                    alert("Username already exists.");
                });
            
          } 
        else{
            alert("Invalid input!");
        }   
        }
        
          
}