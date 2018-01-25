import { Component, OnInit,Input } from '@angular/core';
import {NgForm} from '@angular/forms';
import { NgZone } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import {AppUserService} from '../services/appUser.service';
import { Subforum } from '../models/subforum.model';
import { Topic } from '../models/topic.model';
import { AppUser } from '../models/appUser.model';

@Component({
    selector: 'user-page',
    templateUrl: './user-page.component.html',
    providers: [AppUserService],
  })

export class UserPage implements OnInit{

    user : AppUser = new AppUser();
    dislikesAndLikes : number[] = [];
    constructor(private httpAppUserService : AppUserService ) {

    }

    ngOnInit() {

            this.httpAppUserService.getDataById(sessionStorage.getItem("username")).subscribe(
                (prod: any) => {this.user = prod; console.log(this.user)},
                  error => {alert("Unsuccessful fetch operation!"); console.log(error);});

            this.httpAppUserService.getDislikesAndLikes(sessionStorage.getItem("username")).subscribe(
                (prod: any) => {this.dislikesAndLikes = prod; console.log(this.dislikesAndLikes)},
                  error => {alert("Unsuccessful fetch operation!"); console.log(error);}); 
            
    }
}