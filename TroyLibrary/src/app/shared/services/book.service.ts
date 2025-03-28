import { HttpClient, HttpHeaders } from "@angular/common/http";
import { environment } from "../../../environments/environment";
import { Injectable } from "@angular/core";
import { GetBookResponse, GetBooksResponse } from "../../book/models";

const endpoint = environment.apiUrl + 'api/book';
const httpOptions = {
    headers: new HttpHeaders ({
        'Content-Type': 'application/json'
    })
};

@Injectable({
    providedIn: 'root'
})
export class BookService {

    constructor(private http: HttpClient) {}

    public getBook(bookId: number) {
        return this.http.get<GetBookResponse>(`${endpoint}/book?bookId=${bookId}`);
    }

    public getFeaturedBooks() {
        return this.http.get<GetBooksResponse>(`${endpoint}/featured`);
    }

    public searchBooks(title: string) {
        return this.http.get<GetBooksResponse>(`${endpoint}/search?title=${title}`);
    }
}
