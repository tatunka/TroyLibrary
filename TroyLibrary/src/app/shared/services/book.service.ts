import { HttpClient } from "@angular/common/http";
import { environment } from "../../../environments/environment";
import { Injectable } from "@angular/core";
import { BookRequest, GetBookResponse, GetBooksResponse } from "../../book/models";
import { Observable } from "rxjs";
import { BooleanResponse, CrudResponse } from "../models/models";

const endpoint = environment.apiUrl + 'api/book';

@Injectable({
    providedIn: 'root'
})
export class BookService {

    constructor(private http: HttpClient) {}

    public getBook(bookId: number): Observable<GetBookResponse> {
        return this.http.get<GetBookResponse>(`${endpoint}/book?bookId=${bookId}`);
    }

    public getFeaturedBooks(): Observable<GetBooksResponse> {
        return this.http.get<GetBooksResponse>(`${endpoint}/featured`);
    }

    public searchBooks(title: string): Observable<GetBooksResponse> {
        return this.http.get<GetBooksResponse>(`${endpoint}/search?title=${title}`);
    }

    public createBook(request: BookRequest): Observable<GetBookResponse> {
        return this.http.post<GetBookResponse>(`${endpoint}`, request);
    }

    public updateBook(request: BookRequest): Observable<CrudResponse> {
        return this.http.patch<CrudResponse>(`${endpoint}/update`, request);
    }

    public removeBook(bookId: number): Observable<CrudResponse> {
        return this.http.delete<CrudResponse>(`${endpoint}?bookid=${bookId}`);
    }

    public checkoutBook(bookId: number): Observable<BooleanResponse> {
        return this.http.patch<BooleanResponse>(`${endpoint}/checkout?bookid=${bookId}`, null);
    }

    public returnBook(bookId: number): Observable<BooleanResponse> {
        return this.http.patch<BooleanResponse>(`${endpoint}/return?bookid=${bookId}`, null);
    }
}
