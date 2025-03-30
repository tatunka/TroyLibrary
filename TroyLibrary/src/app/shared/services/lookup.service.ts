import { Injectable } from "@angular/core";
import { environment } from "../../../environments/environment";
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";
import { LookupResponse } from "../models/lookup-models";

const endpoint = environment.apiUrl + 'api/lookup';

@Injectable({
  providedIn: 'root'
})
export class LookupService {

    constructor(private http: HttpClient) {}

    lookup(lookupName: string): Observable<LookupResponse> {
        return this.http.get<LookupResponse>(`${endpoint}?name=${lookupName}`);
    }
}