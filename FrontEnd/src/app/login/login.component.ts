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

export class LoginComponent implements OnInit{

    user: AppUser = null;
    logg : boolean = false;

    constructor(private httpAppUserService: AppUserService,private router : Router ) {
    }


    ngOnInit() {
             
    }

    onSubmit(user: AppUser, form: NgForm) {

        this.httpAppUserService.login(user.UserName,user.Password).subscribe(
            (prod: any) => {this.user = prod; console.log(this.user)},//You can set the type to Product.
             error => {alert("Invalid username or password!"); console.log(error);});
        
        if(this.user != null)
            {
                
                sessionStorage.setItem("user", JSON.stringify(this.user.Id));
                sessionStorage.setItem("username",this.user.UserName);
                sessionStorage.setItem("role",this.user.Role);
                this.router.navigate(['/home']);
                
            }
            else
            {
                alert("Invalid username or password!");
                form.reset();
            }

           
            
            //window.location.reload();
        }

    isLoggedIn(): Boolean{
        
        return this.httpAppUserService.isLoggedIn();

    }

       
        
      
}