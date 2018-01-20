import { Injectable } from "@angular/core"
import { Http, Response, RequestOptions, Headers } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/map';
import 'rxjs/Rx'
import {AppUser} from '../models/appUser.model';
import { Router, ActivatedRoute } from '@angular/router';
@Injectable()
export class AppUserService{
    

    //logged : boolean;
    user: AppUser;
    constructor (private http: Http,private router: Router){

    }

    getData(): Observable<AppUser> {
        const url = `http://localhost:13124//api/AppUsers/`;
        return this.http.get(url).map(this.extractData);        
    }

    private extractData(res: Response) {
        let body = res.json();
        return body || [];
    }

    login(usernameAndPassword: string): Observable<any> {
        const url = `http://localhost:13124//api/AppUsers?usernameAndPassword=` +usernameAndPassword;
        return this.http.get(url).map(this.extractData);        
    }

    post(data: AppUser): Observable<any> {
        const headers: Headers = new Headers();
        
        headers.append('Content-type', 'application/json');
        return this.http
        .post(`http://localhost:13124//api/AppUsers/`,
        JSON.stringify(data),{headers:headers})
        .map((response: Response) => response.json());
    }

    put(id:number,data: AppUser): Promise<any> {
        const headers: Headers = new Headers();
        
        headers.append('Content-type', 'application/json');
        return this.http
        .put(`http://localhost:13124//api/AppUsers/${id}`,
        JSON.stringify(data),{headers:headers})
        .toPromise()
        .then(res => res.json() as AppUser);
    }

    delete(id:number): Promise<any> {
        const headers: Headers = new Headers();
        
        headers.append('Content-type', 'application/json');
        return this.http
        .delete(`http://localhost:13124//api/AppUsers/${id}`,{headers:headers})
        .toPromise();
    }

    getRandomInt(min, max) {
        return Math.floor(Math.random() * (max - min + 1)) + min;
    }

    isLoggedIn() : boolean{

        if(sessionStorage.getItem("username") != null)
            return true;
        else return false;
    }

    logOut()
    {
        sessionStorage.clear();
    }

    role() : string
    {
        return sessionStorage.getItem("role");
    }

    routing(route:string): void{

        if(sessionStorage.getItem("route") != route)
        {
            sessionStorage.setItem("route",route);
            this.router.navigate([route]);
        }     
    }

}