import { Injectable } from "@angular/core"
import { Http, Response, RequestOptions, Headers } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/map';
import 'rxjs/Rx'
import {Comment} from '../models/comment.model';
@Injectable()
export class CommentService{
    
    constructor (private http: Http){

    }

    getDatabyId(topicId:number): Observable<Comment> {
        const url = `http://localhost:13124//api/Comments/${topicId}`;
        return this.http.get(url).map(this.extractData);        
    }

    private extractData(res: Response) {
        let body = res.json();
        return body || [];
    }


    post(data: Comment): Promise<any> {
        const headers: Headers = new Headers();
        
        headers.append('Content-type', 'application/json');
        return this.http
        .post(`http://localhost:13124//api/Comments/`,
        JSON.stringify(data),{headers:headers})
        .toPromise()
        .then(res => res.json() as Comment);
    }

    put(id:number,data: Comment): Promise<any> {
        const headers: Headers = new Headers();
        
        headers.append('Content-type', 'application/json');
        return this.http
        .put(`http://localhost:13124//api/Comments?id=`+id,
        JSON.stringify(data),{headers:headers})
        .toPromise()
        .then(res => res.json() as Comment);
    }

    delete(commentAndTopicIds : string): Observable<any> {
        const headers: Headers = new Headers();
        
        headers.append('Content-type', 'application/json');
        return this.http
        .delete(`http://localhost:13124//api/Comments?commentAndTopicIds=`+commentAndTopicIds
        ,{headers:headers})
        .map((response: Response) => response.json());
    }

}