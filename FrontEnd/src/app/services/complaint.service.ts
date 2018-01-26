import { Injectable } from "@angular/core"
import { Http, Response, RequestOptions, Headers } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/map';
import 'rxjs/Rx'
import {Complaint} from '../models/complaint.model';
@Injectable()
export class ComplaintService{
    
    constructor (private http: Http){

    }

    getData(): Observable<Complaint> {
        const url = `http://localhost:13124//api/Complaints/`;
        return this.http.get(url).map(this.extractData);        
    }

    getDatabyId(useRole: string): Observable<Complaint> {
        const url = `http://localhost:13124//api/Complaints?useRole=`+useRole;
        return this.http.get(url).map(this.extractData);        
    }

    private extractData(res: Response) {
        let body = res.json();
        return body || [];
    }


    post(data: Complaint): Promise<any> {
        const headers: Headers = new Headers();
        
        headers.append('Content-type', 'application/json');
        return this.http
        .post(`http://localhost:13124//api/Complaints/`,
        JSON.stringify(data),{headers:headers})
        .toPromise()
        .then(res => res.json() as Complaint);
    }

    put(id:number,data: Complaint): Promise<any> {
        const headers: Headers = new Headers();
        
        headers.append('Content-type', 'application/json');
        return this.http
        .put(`http://localhost:13124//api/Complaints/${id}`,
        JSON.stringify(data),{headers:headers})
        .toPromise()
        .then(res => res.json() as Complaint);
    }

    delete(id:number): Promise<any> {
        const headers: Headers = new Headers();
        
        headers.append('Content-type', 'application/json');
        return this.http
        .delete(`http://localhost:13124//api/Complaints/${id}`,{headers:headers})
        .toPromise();
    }

}