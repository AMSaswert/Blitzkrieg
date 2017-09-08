import { Injectable } from "@angular/core"
import { Http, Response, RequestOptions, Headers } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/map';
import 'rxjs/Rx'
import {AppUser} from '../models/appUser.model';
@Injectable()
export class AppUserService{
    
    constructor (private http: Http){

    }

    getDatabyId(): Observable<AppUser> {
        const url = `http://localhost:13124//api/AppUsers/`;
        return this.http.get(url).map(this.extractData);        
    }

    private extractData(res: Response) {
        let body = res.json();
        return body || [];
    }


    post(data: AppUser): Promise<any> {
        const headers: Headers = new Headers();
        
        headers.append('Content-type', 'application/json');
        return this.http
        .post(`http://localhost:13124//api/AppUsers/`,
        JSON.stringify(data),{headers:headers})
        .toPromise()
        .then(res => res.json() as AppUser);
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

}