import { AfterViewInit, Component, ElementRef, OnChanges, OnInit, SimpleChanges, ViewChild } from '@angular/core';
import { BookService } from '../../shared/services/book.service';
import { BookDetail, GetBookResponse, Review } from '../models';
import { ActivatedRoute } from '@angular/router';
import { LookupService } from '../../shared/services/lookup.service';
import { LookupItem, LookupResponse } from '../../shared/models/lookup-models';
import { Lookups } from '../../shared/models/models';

@Component({
  selector: 'app-book-detail',
  standalone: false,
  templateUrl: './book-detail.component.html',
  styleUrl: './book-detail.component.css'
})
export class BookDetailComponent implements OnInit {

  constructor(
    private bookService: BookService, 
    private lookupService: LookupService,
    private route: ActivatedRoute
  ) {}

  @ViewChild('description') descriptionTextArea!: ElementRef<HTMLTextAreaElement>;

  bookId?: number;
  bookDetail?: BookDetail;
  reviews: Review[] = [];
  categories: LookupItem[] = [];

  ngOnInit(): void {
    //get books
    this.bookId = +(this.route.snapshot.paramMap.get('bookId') ?? 0);
    this.bookService.getBook(this.bookId).subscribe(
      (value: GetBookResponse) => {
        this.bookDetail = value.bookDetail;
        this.reviews = this.bookDetail?.reviews?.length > 0 ? this.bookDetail.reviews : [];
        setTimeout(() => {
          if (this.descriptionTextArea) {
            var textarea = this.descriptionTextArea.nativeElement;
            textarea.style.height = 'auto';
            textarea.style.height = (textarea.scrollHeight + 2) + 'px';
          }
        }, 100);
      });
    this.lookupService.lookup(Lookups.Category).subscribe({
      next: (response: LookupResponse) => this.categories = response.items
    }
    )
  }

  checkOutBook() {

  }
}
