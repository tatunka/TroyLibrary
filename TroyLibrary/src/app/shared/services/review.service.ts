import { Injectable } from "@angular/core";
import { environment } from "../../../environments/environment";
import { HttpClient } from "@angular/common/http";
import { CreateReviewRequest, GetReviewsResponse } from "../models/models";
import { Observable } from "rxjs";

const endpoint = environment.apiUrl + 'api/review';

@Injectable({
  providedIn: 'root'
})
export class ReviewService {

    constructor(private http: HttpClient) {}

    GetReviews(bookId: number): Observable<GetReviewsResponse> {
        return this.http.get<GetReviewsResponse>(`${endpoint}?bookId=${bookId}`);
    }

    CreateReview(request: CreateReviewRequest): Observable<GetReviewsResponse> {
        return this.http.post<GetReviewsResponse>(`${endpoint}`, request);
    }
}