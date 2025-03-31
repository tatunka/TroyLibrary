import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BookComponent } from './book.component';
import { BookService } from '../shared/services/book.service';
import { HttpClient } from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { BookDetailComponent } from './book-detail/book-detail.component';
import { NgbDropdownModule } from '@ng-bootstrap/ng-bootstrap'
import { AppRoutingModule } from '../app-routing.module';



@NgModule({
  declarations: [
    BookComponent,
    BookDetailComponent
  ],
  imports: [
    AppRoutingModule,
    CommonModule,
    FormsModule,
    NgbDropdownModule,
    ReactiveFormsModule
  ],
  providers: [
    HttpClient,
    BookService
  ]
})
export class BookModule { }
