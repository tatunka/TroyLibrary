import { Component, ElementRef, OnInit, ViewChild, viewChild } from '@angular/core';
import { BookService } from '../shared/services/book.service';
import { Book, GetBooksResponse } from './models';

@Component({
  selector: 'app-book',
  standalone: false,
  templateUrl: './book.component.html',
  styleUrl: './book.component.css'
})
export class BookComponent implements OnInit {

  constructor(private bookService: BookService){}

  pageTitle: string = "Featured Books";
  searchTitle: string = '';

  books: Book[] = [];

  ngOnInit(): void {
    this.bookService.getFeaturedBooks().subscribe(
      (value: GetBooksResponse) => {
        this.books = value.books;
      }
    )
  }

  searchBooks() {
    this.pageTitle = `Search: ${this.searchTitle}`
    this.bookService.searchBooks(this.searchTitle).subscribe(
      (value: GetBooksResponse) => {
        this.books = value.books;
      }
    )
  }

}
