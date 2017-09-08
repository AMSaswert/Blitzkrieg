import { Injectable } from "@angular/core"
import { Http, Response, RequestOptions, Headers } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/map';
import 'rxjs/Rx'
import {Topic} from '../models/topic.model';
@Injectable()
export class TopicService{
    
    constructor (private http: Http){

    }

    getData(): Observable<Topic> {
        const url = `http://localhost:13124//api/Topics/`;
        return this.http.get(url).map(this.extractData);        
    }

    getDatabyId(id: number): Observable<Topic> {
        const url = `http://localhost:13124//api/Topics/${id}`;
        return this.http.get(url).map(this.extractData);        
    }

    private extractData(res: Response) {
        let body = res.json();
        return body || [];
    }


    post(data: Topic): Promise<any> {
        const headers: Headers = new Headers();
        
        headers.append('Content-type', 'application/json');
        return this.http
        .post(`http://localhost:13124//api/Topics/`,
        JSON.stringify(data),{headers:headers})
        .toPromise()
        .then(res => res.json() as Topic);
    }

    put(id:number,data: Topic): Promise<any> {
        const headers: Headers = new Headers();
        
        headers.append('Content-type', 'application/json');
        return this.http
        .put(`http://localhost:13124//api/Topics/${id}`,
        JSON.stringify(data),{headers:headers})
        .toPromise()
        .then(res => res.json() as Topic);
    }

    delete(id:number): Promise<any> {
        const headers: Headers = new Headers();
        
        headers.append('Content-type', 'application/json');
        return this.http
        .delete(`http://localhost:13124//api/Topics/${id}`,{headers:headers})
        .toPromise();
    }

}