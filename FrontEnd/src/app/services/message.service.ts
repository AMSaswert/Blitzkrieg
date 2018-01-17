import { Injectable } from "@angular/core"
import { Http, Response, RequestOptions, Headers } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/map';
import 'rxjs/Rx'
import {Message} from '../models/message.model';
@Injectable()
export class MessageService{
    
    constructor (private http: Http){

    }

    getDatabyId(username:string): Observable<Message> {
        const url = `http://localhost:13124//api/Messages?username=`+username;
        return this.http.get(url).map(this.extractData);        
    }

    private extractData(res: Response) {
        let body = res.json();
        return body || [];
    }

    create(username:string,data: Message): Promise<any> {
        const headers: Headers = new Headers();
        
        headers.append('Content-type', 'application/json');
        return this.http
        .put(`http://localhost:13124//api/Messages?username=`+username,
        JSON.stringify(data),{headers:headers})
        .toPromise()
        .then(res => res.json() as Message);
    }

    update(id:number,data: Message): Promise<any> {
        const headers: Headers = new Headers();
        
        headers.append('Content-type', 'application/json');
        return this.http
        .put(`http://localhost:13124//api/Messages/${id}`,
        JSON.stringify(data),{headers:headers})
        .toPromise()
        .then(res => res.json() as Message);
    }

    delete(data: Message): Promise<any> {
        const headers: Headers = new Headers();
        
        headers.append('Content-type', 'application/json');
        return this.http
        .delete(`http://localhost:13124//api/Messages/${data.Id}`,{headers:headers})
        .toPromise();
    }

    private handleError(error: any): Promise<any> {
        console.error('An error occurred', error); // for demo purposes only
        return Promise.reject(error.message || error);
    }
        
}

