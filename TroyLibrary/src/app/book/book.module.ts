import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BookComponent } from './book.component';
import { BookService } from '../shared/services/book.service';
import { HttpClient } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { BookDetailComponent } from './book-detail/book-detail.component';
import { NgbDropdownModule } from '@ng-bootstrap/ng-bootstrap'



@NgModule({
  declarations: [
    BookComponent,
    BookDetailComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    NgbDropdownModule
  ],
  providers: [
    HttpClient,
    BookService
  ]
})
export class BookModule { }
