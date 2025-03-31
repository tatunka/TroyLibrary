import { Component, OnInit } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { ToastService } from '../toast/toast-service';
import { Router } from '@angular/router';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { LookupService } from '../../services/lookup.service';
import { LookupItem, LookupResponse } from '../../models/lookup-models';
import { Lookups } from '../../models/models';
import { BookService } from '../../services/book.service';
import { BookData, BookRequest, GetBookResponse } from '../../../book/models';

@Component({
  selector: 'app-addbook-modal',
  standalone: false,
  templateUrl: './addbook-modal.component.html',
  styleUrl: './addbook-modal.component.css'
})
export class AddbookModalComponent implements OnInit {

  constructor(
    private activeModal: NgbActiveModal,
    private toastService: ToastService,
    private lookupService: LookupService,
    private bookService: BookService,
    private router: Router
  ) {}

  categories: LookupItem[] = [];

  form = new FormGroup({
    title: new FormControl('', Validators.required),
    author: new FormControl('', Validators.required),
    description: new FormControl('', Validators.required),
    coverImage: new FormControl('', Validators.required),
    publisher: new FormControl('', Validators.required),
    publicationDate: new FormControl<Date | null>(null, Validators.required),
    category: new FormControl<number>(1, Validators.required),
    isbn: new FormControl('', Validators.required),
    pageCount: new FormControl<number>(0, Validators.required),
  });

  ngOnInit(): void {
    this.lookupService.lookup(Lookups.Category).subscribe({
      next: (response: LookupResponse) => this.categories = response.items
    })
  }

  close() {
    this.activeModal.close();
  }

  addBook() {
    const request: BookRequest = {
      bookData: this.form.value as BookData,
    }
    request.bookData.category = Number(request.bookData.category);
    this.bookService.createBook(request).subscribe({
      next: (response: GetBookResponse) => {
        this.toastService.showSuccess('Successfully added book to library!');
        this.activeModal.close();
        this.router.navigate(['/detail', response.bookDetail.bookId]);
      },
      error: (error) => {
        this.toastService.showError('Unable to add book to library');
        console.log(error.message);
      }
    })
  }
}
