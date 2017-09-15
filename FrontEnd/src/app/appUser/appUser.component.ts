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
            (prod: any) => {this.appUsers = prod; console.log(this.appUsers)},//You can set the type to Product.
             error => {alert("Unsuccessful fetch operation!"); console.log(error);});
             
    }

    onSubmit(user: AppUser, form: NgForm) {

        user.BookmarkedSubforums = new Array<number>();
        user.SavedTopics = new Array<number>();
        user.SavedComments = new Array<number>();
        user.ReceivedMessages = new Array<Message>();
        user.RegistrationDate = new Date(Date.now());
        this.httpAppUserService.post(user);
        
        form.reset();
        window.location.reload();
        
      }

      edit(user: AppUser, form: NgForm) {
        
         this.httpAppUserService.put(user.Id,user);
                 
         form.reset();
         window.location.reload();
       }

       delete(user: AppUser, form: NgForm) {
        
         this.httpAppUserService.delete(user.Id);
       }
}