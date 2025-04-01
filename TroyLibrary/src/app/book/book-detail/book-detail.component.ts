import { Component, ElementRef, OnChanges,OnDestroy,OnInit, SimpleChanges, ViewChild } from '@angular/core';
import { BookService } from '../../shared/services/book.service';
import { BookData, BookDetail, BookRequest, GetBookResponse } from '../models';
import { ActivatedRoute, Params, Router } from '@angular/router';
import { LookupService } from '../../shared/services/lookup.service';
import { LookupItem, LookupResponse } from '../../shared/models/lookup-models';
import { BooleanResponse, CrudResponse, GetReviewsResponse, Lookups, Review } from '../../shared/models/models';
import { AuthService } from '../../shared/services/auth.service';
import { Role } from '../../shared/models/auth-models';
import { ToastService } from '../../shared/components/toast/toast-service';
import { DatePipe } from '@angular/common';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ReviewService } from '../../shared/services/review.service';
import { NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { ReviewModalComponent } from '../../shared/components/review-modal/review-modal.component';
import { Subscription } from 'rxjs';

const datePipe = new DatePipe('en-US');

@Component({
  selector: 'app-book-detail',
  standalone: false,
  templateUrl: './book-detail.component.html',
  styleUrl: './book-detail.component.css'
})
export class BookDetailComponent implements OnInit, OnChanges, OnDestroy {

  constructor(
    private bookService: BookService, 
    private reviewService: ReviewService,
    private lookupService: LookupService,
    private toast: ToastService,
    private modalService: NgbModal,
    protected auth: AuthService,
    private route: ActivatedRoute,
    private router: Router
  ) {
    this.paramSubscription = this.route.params.subscribe({
      next: (params: Params) => {
        this.bookId = +params['bookId'];
        this.reloadPage();
      },
    });
  }

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

  private paramSubscription: Subscription;

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
    //this.reloadPage();
    //get categories for dropdown
    this.lookupService.lookup(Lookups.Category).subscribe({
      next: (response: LookupResponse) => this.categories = response.items,
      error: (error) => {
        this.toast.showError('Unable to load categories');
        console.log(error.message);
      }
    });
  }

  private reloadPage() {
    //get books
    this.bookService.getBook(this.bookId).subscribe({
      next: (response: GetBookResponse) => {
        this.bookDetail = response.bookDetail;
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
          //auto-size description text area
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
        this.router.navigate(['/error'])
      }
    });
    //get reviews
    this.refreshReviews(this.bookId);
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes && changes['bookId']) {
      this.reloadPage();
    }
  }

  ngOnDestroy(): void {
    if (this.paramSubscription) {
      this.paramSubscription.unsubscribe();
    }
  }

  updateBook(event: MouseEvent) {
    event.preventDefault();
    const request: BookRequest = {
      bookData: this.form.value as BookData,
    }
    request.bookData.bookId = this.bookId;
    request.bookData.category = Number(request.bookData.category);
    this.bookService.updateBook(request).subscribe({
      next: (response: CrudResponse) => {
        if (response.completedAt) {
          this.toast.showSuccess('Changes saved successfully!');
          this.pubDate = datePipe.transform(request.bookData.publicationDate, 'M/d/yyyy');
          this.bookDetail.coverImage = request.bookData.coverImage;
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

  refreshReviews(bookId: number) {
  //get reviews
    this.reviewService.GetReviews(bookId).subscribe({
      next: (response: GetReviewsResponse) => {
        this.reviews = response.reviews;
        const sum = this.reviews
          .map(r => r.rating)
          .reduce((a, v) => a + v);
          this.bookDetail.rating = this.reviews.length != 0 ? sum / this.reviews.length : 0;
      },
      error: (error) => {
        this.toast.showError('Unable to retrieve reviews for this book');
        console.log(error.message);
      }
    });
  }

  checkOutBook(event: MouseEvent) {
    event.preventDefault();
    this.bookService.checkoutBook(this.bookId).subscribe({
      next: (response: BooleanResponse) => {
        if (response.success) {
          this.toast.showSuccess('Successfully check out book!');
          (this.bookDetail as BookDetail).isAvailable = false;
        }
        else {
          this.toast.showError('Unable to check out book');
        }
      },
      error: (error) => {
        this.toast.showError('Unable to check out book');
        console.log(error.message);
      }
    });
  }

  returnBook(event: MouseEvent) {
    event.preventDefault();
    this.bookService.returnBook(this.bookId).subscribe({
      next: (response: BooleanResponse) => {
        if (response.success) {
          this.toast.showSuccess('Book successfully returned!');
          this.bookDetail.isAvailable = true;
          this.bookDetail.isOverdue = false;
          this.bookDetail.checkoutDate = undefined;
        }
        else {
          this.toast.showError('Unable to return book');
        }
      },
      error: (error) => {
        this.toast.showError('Unable to return book');
        console.log(error.message);
      }
    });
  }

  removeBook(event: MouseEvent) {
    event.preventDefault();
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

  reviewBook(event: MouseEvent) {
    event.preventDefault();
    const modalRef: NgbModalRef = this.modalService.open(ReviewModalComponent, {});
    modalRef.componentInstance.bookId = this.bookId;
    modalRef.componentInstance.callback = () => {
      modalRef.close();
      this.refreshReviews(this.bookId);
    };
  }

  numSequence(n?: number): Array<number> {
    if (n) {
      return Array(Math.floor(n));
    }
    return [];
  }
}
