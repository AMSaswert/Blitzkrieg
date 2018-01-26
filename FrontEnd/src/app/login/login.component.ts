import { Component, OnInit,Input } from '@angular/core';
import {NgForm} from '@angular/forms';
import {AppUserService} from '../services/appUser.service';
import { AppUser } from '../models/appUser.model';
import { Router, ActivatedRoute } from '@angular/router';
@Component({
    selector: 'app-login',
    templateUrl: './login.component.html',
    styleUrls: ['./login.component.css'],
    providers: [AppUserService]
  })

export class LoginComponent{

    username : string = "";
    password : string = "";

    constructor(private httpAppUserService: AppUserService,private router : Router ) {
    }

    login() : void
    {
        this.httpAppUserService.login(this.username+"-"+this.password).subscribe(
            (prod: any) => { console.log(prod);

                if(prod.length != 0)
                {
                    sessionStorage.setItem("username",prod.UserName);
                    sessionStorage.setItem("role",prod.Role);
                    this.httpAppUserService.routing("/user-page");
                }
                else
                {
                    alert("Invalid username or password!");
                }          
            }   
        );
    }
        
}

    
