import { Component, OnInit,Input } from '@angular/core';
import {NgForm} from '@angular/forms';
import {AppUserService} from './services/appUser.service';
import { Router } from '@angular/router';
@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  
      constructor(private httpAppUserService: AppUserService,private router: Router) {}
  
  
      ngOnInit() {
  
      }

      isLoggedIn() : boolean
      {
        return this.httpAppUserService.isLoggedIn();
      }

      logOut() : void
      {
        this.httpAppUserService.logOut();
        this.router.navigate(['/home']);
      }

      authRole() : boolean
      {
        if("Admin" == sessionStorage.getItem("role") )
          return true;
        return false;
      }
  
}
