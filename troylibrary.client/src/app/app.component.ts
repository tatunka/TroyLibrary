import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

interface WeatherForecast {
  date: string;
  temperatureC: number;
  temperatureF: number;
  summary: string;
}

interface Book {
  bookId: number;
  title: string;
  author: string;
  description: string;
  coverImage: string;
  rating: number;
  isAvailable: boolean
}

interface GetBookResponse {
  Books: []
}

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  standalone: false,
  styleUrl: './app.component.css'
})
export class AppComponent implements OnInit {
  public forecasts: WeatherForecast[] = [];
  public books: Book[] = [];

  constructor(private http: HttpClient) {}

  ngOnInit() {
    this.getForecasts();
    this.getFeaturedBooks();
  }

  getForecasts() {
    this.http.get<WeatherForecast[]>('/weatherforecast').subscribe(
      (result) => {
        this.forecasts = result;
      },
      (error) => {
        console.error(error);
      }
    );
  }

  getFeaturedBooks() {
    this.http.get<GetBookResponse>('/api/book/featured').subscribe(
      (value: GetBookResponse) => {
        this.books = value.Books
      }
    )
  }

  title = 'troylibrary.client';
}
