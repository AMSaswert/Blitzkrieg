import { Injectable } from "@angular/core"
import { Http, Response, RequestOptions, Headers } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/map';
import 'rxjs/Rx'
import {Subforum} from '../models/subforum.model';
@Injectable()
export class SubforumService{
    
    constructor (private http: Http){

    }

    getData(): Observable<Subforum> {
        const url = `http://localhost:13124//api/Subforums/`;
        return this.http.get(url).map(this.extractData);        
    }

    getDatabyId(id: number): Observable<Subforum> {
        const url = `http://localhost:13124//api/Subforums/${id}`;
        return this.http.get(url).map(this.extractData);        
    }

    private extractData(res: Response) {
        let body = res.json();
        return body || [];
    }


    post(data: Subforum): Promise<any> {
        const headers: Headers = new Headers();
        
        headers.append('Content-type', 'application/json');
        return this.http
        .post(`http://localhost:13124//api/Subforums/`,
        JSON.stringify(data),{headers:headers})
        .toPromise()
        .then(res => res.json() as Subforum);
    }

    put(id:number,data: Subforum): Promise<any> {
        const headers: Headers = new Headers();
        
        headers.append('Content-type', 'application/json');
        return this.http
        .put(`http://localhost:13124//api/Subforums/${id}`,
        JSON.stringify(data),{headers:headers})
        .toPromise()
        .then(res => res.json() as Subforum);
    }

    delete(id:number): Observable<any> {
        const headers: Headers = new Headers();
        
        headers.append('Content-type', 'application/json');
        return this.http
        .delete(`http://localhost:13124//api/Subforums/${id}`
        ,{headers:headers})
        .map((response: Response) => response.json());
    }

}