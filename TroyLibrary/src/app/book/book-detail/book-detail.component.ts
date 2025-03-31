import { AfterViewInit, Component, ElementRef, Inject, LOCALE_ID, OnChanges, OnInit, SimpleChanges, ViewChild } from '@angular/core';
import { BookService } from '../../shared/services/book.service';
import { BookData, BookDetail, BookRequest, GetBookResponse, Review } from '../models';
import { ActivatedRoute, Router } from '@angular/router';
import { LookupService } from '../../shared/services/lookup.service';
import { LookupItem, LookupResponse } from '../../shared/models/lookup-models';
import { Category, CrudResponse, Lookups } from '../../shared/models/models';
import { AuthService } from '../../shared/services/auth.service';
import { Role } from '../../shared/models/auth-models';
import { ToastService } from '../../shared/components/toast/toast-service';
import { DatePipe, formatDate } from '@angular/common';
import { FormControl, FormGroup, Validators } from '@angular/forms';

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
    private toast: ToastService,
    protected auth: AuthService,
    private route: ActivatedRoute,
    private router: Router
  ) {}

  @ViewChild('description') descriptionTextArea!: ElementRef<HTMLTextAreaElement>;

  
  form = new FormGroup({
    title: new FormControl('', Validators.required),
    author: new FormControl('', Validators.required),
    description: new FormControl('', Validators.required),
    coverImage: new FormControl('', Validators.required),
    publisher: new FormControl('', Validators.required),
    publicationDate: new FormControl(new Date(), Validators.required),
    category: new FormControl(1, Validators.required),
    isbn: new FormControl('', Validators.required),
    pageCount: new FormControl<number>(0, Validators.required),
  });

  bookId!: number;
  bookDetail!: BookDetail;
  reviews: Review[] = [];
  categories: LookupItem[] = [];
  isLibrarian!: boolean;
  pubDate: string | null = '';
  formControlClass: string = '';

  //expose enum to template
  public role = Role;

  ngOnInit(): void {
    this.isLibrarian = this.auth.isInRole(Role.Librarian);
    if (this.isLibrarian) {
      this.formControlClass = 'form-control';
    }
    else {
      this.formControlClass = 'form-control-plaintext';
    }
    //get books
    this.bookId = +(this.route.snapshot.paramMap.get('bookId') ?? 0);
    this.bookService.getBook(this.bookId).subscribe({
      next: (response: GetBookResponse) => {
        this.bookDetail = response.bookDetail;
        this.reviews = this.bookDetail?.reviews?.length > 0 ? this.bookDetail.reviews : [];
        var datePipe = new DatePipe('en-US');
        this.pubDate = datePipe.transform(this.bookDetail.publicationDate);
        this.form = new FormGroup({
          title: new FormControl(response.bookDetail.title, Validators.required),
          author: new FormControl(response.bookDetail.author, Validators.required),
          description: new FormControl(response.bookDetail.description, Validators.required),
          coverImage: new FormControl(response.bookDetail.coverImage, Validators.required),
          publisher: new FormControl(response.bookDetail.publisher, Validators.required),
          publicationDate: new FormControl(response.bookDetail.publicationDate, Validators.required),
          category: new FormControl(response.bookDetail.category, Validators.required),
          isbn: new FormControl(response.bookDetail.isbn, Validators.required),
          pageCount: new FormControl<number>(response.bookDetail.pageCount, Validators.required),
        });
        setTimeout(() => {
          if (this.descriptionTextArea) {
            var textarea = this.descriptionTextArea.nativeElement;
            textarea.style.height = 'auto';
            textarea.style.height = (textarea.scrollHeight + 2) + 'px';
          }
        }, 100);
      },
      error: (error) => {
        this.toast.showError('Unable to retrieve book details');
        console.log(error.message);
      }
    });
    //get categories for dropdown
    this.lookupService.lookup(Lookups.Category).subscribe({
      next: (response: LookupResponse) => this.categories = response.items,
      error: (error) => {
        this.toast.showError('Unable to load categories');
        console.log(error.message);
      }
    });
    //get reviews
    //TODO: get reviews
  }

  updateBook() {
    const request: BookRequest = {
      bookData: this.form.value as BookData,
    }
    request.bookData.bookId = this.bookId;
    request.bookData.category = Number(request.bookData.category);
    this.bookService.updateBook(request).subscribe({
      next: (response: CrudResponse) => {
        if (response.completedAt) {
          this.toast.showSuccess('Changes saved successfully!');
          this.bookDetail.publicationDate = request.bookData.publicationDate;
          this.form.markAsPristine();
        }
        else {
          this.toast.showError('Unable to save changes');
        }
      },
      error: (error) => {
        this.toast.showError('Unable to save changes');
        console.log(error.message);
      }
    })
  }

  checkOutBook() {

  }

  returnBook() {
    
  }

  removeBook() {
    this.bookService.removeBook(this.bookId).subscribe({
      next: (response: CrudResponse) => {
        if (response.completedAt) {
          this.toast.showSuccess('Book successfully removed from library!');
          this.router.navigate(['/book']);
        }
      },
      error: (error) => {
        this.toast.showError('Unable to remove book from library');
        console.log(error.message);
      }
    });
  }

  reviewBook() {
    
  }
}
