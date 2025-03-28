import { Component, OnInit } from '@angular/core';
import { BookService } from '../../shared/services/book.service';
import { BookDetail, GetBookResponse } from '../models';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-book-detail',
  standalone: false,
  templateUrl: './book-detail.component.html',
  styleUrl: './book-detail.component.css'
})
export class BookDetailComponent implements OnInit {

  constructor(private bookService: BookService, private route: ActivatedRoute) {}

  bookId?: number;
  bookDetail?: BookDetail;

  ngOnInit(): void {
    this.bookId = +(this.route.snapshot.paramMap.get('bookId') ?? 0);

    this.bookService.getBook(this.bookId).subscribe(
      (value: GetBookResponse) => this.bookDetail = value.bookDetail,
    );
  }
}
