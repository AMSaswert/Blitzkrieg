import { Injectable } from "@angular/core"
import { Http, Response, RequestOptions, Headers } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/map';
import 'rxjs/Rx'
import { ComplaintService } from "./complaint.service";
import {Complaint} from '../models/complaint.model';
@Injectable()
export class ComplaintsHelpService{
    
    constructor (private http: Http){

    }

    getDatabyId(id: number): Observable<Complaint> {
        const url = `http://localhost:13124//api/ComplaintsHelp/${id}`;
        return this.http.get(url).map(this.extractData);        
    }

    private extractData(res: Response) {
        let body = res.json();
        return body || [];
    }
}