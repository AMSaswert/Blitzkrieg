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

    user : AppUser = new AppUser();
    username : string = "";
    password : string = "";

    constructor(private httpAppUserService: AppUserService,private router : Router ) {
    }

    login() : void
    {
        this.httpAppUserService.login(this.username+"-"+this.password).subscribe(
            (prod: any) => {this.user = prod; console.log(this.user);

                if(prod.length != 0)
                {
                    sessionStorage.setItem("username",this.user.UserName);
                    sessionStorage.setItem("role",this.user.Role);
                    sessionStorage.setItem("route","/home");
                    this.router.navigate(['/home']);
                }
                else
                {
                    alert("Invalid username or password!");
                }          
            }   
        );
    }
        
}

    
