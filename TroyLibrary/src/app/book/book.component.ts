import { Component, OnInit } from '@angular/core';
import { BookService } from '../shared/services/book.service';
import { Book, GetBooksResponse } from './models';
import { Helper } from '../shared/helper';

@Component({
  selector: 'app-book',
  standalone: false,
  templateUrl: './book.component.html',
  styleUrl: './book.component.css'
})
export class BookComponent implements OnInit {

  constructor(private bookService: BookService, private helper: Helper){}

  pageTitle: string = "Featured Books";
  searchTitle: string = '';
  books: Book[] = [];
  filteredBooks: Book[] = [];
  displayedBooks: Book[] = [];
  titles: { title: string, isActive: boolean }[] = [];
  authors: { author: string, isActive: boolean }[] = [];
  availability: {availability: string, isActive: boolean}[] = [
    { availability: "In Stock",  isActive: false },
    { availability: "Out of Stock", isActive: false }
  ];

  //sort buttons
  sortTitle: boolean = false;
  sortAuthor: boolean = false;
  sortAvailability: boolean = false;
  //filter buttons
  filterTitle: boolean = false;
  filterAuthor: boolean = false;
  filterAvailability: boolean = false;
  filterTitleTerms: string[] = [];
  filterAuthorTerms: string[] = [];
  filterAvailabilityTerms: string[] = [];

  ngOnInit(): void {
    this.bookService.getFeaturedBooks().subscribe(
      (value: GetBooksResponse) => {
        this.books = value.books;
        this.displayedBooks = [...this.books];
        this.titles = this.books.map(b => {return{title: b.title, isActive: false}}).sort((a, b) => this.helper.compare(a, b));
        this.authors = this.books.map(b => {return{author:b.author, isActive: false}}).sort((a,b) => this.helper.compare(a, b));
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

  sortByTitle() {
    this.sortTitle = !this.sortTitle;
    this.sortAuthor = false;
    this.sortAvailability = false;
    this.sort();
  }

  sortByAuthor() {
    this.sortAuthor = !this.sortAuthor;
    this.sortTitle = false;
    this.sortAvailability = false;
    this.sort();
  }

  sortByAvailability() {
    this.sortAvailability = !this.sortAvailability;
    this.sortTitle = false;
    this.sortAuthor = false;
    this.sort();
  }

  filterByTitle(title: { title: string, isActive: boolean }) {
    title.isActive = !title.isActive;
    if (this.filterTitleTerms.includes(title.title)) {
      this.filterTitleTerms = this.filterTitleTerms.filter(f => f !== title.title);
    }
    else {
      this.filterTitleTerms.push(title.title);
    }
    this.filterTitle = this.filterTitleTerms.length > 0;
    this.filter();
  }

  filterByAuthor(author: { author: string, isActive: boolean }) {
    author.isActive = !author.isActive;
    if (this.filterAuthorTerms.includes(author.author)) {
      this.filterAuthorTerms = this.filterAuthorTerms.filter(f => f !== author.author);
    }
    else {
      this.filterAuthorTerms.push(author.author);
    }
    this.filterAuthor = this.filterAuthorTerms.length > 0;
    this.filter();
  }

  filterByAvailability(availability: { availability: string, isActive: boolean }) {
    availability.isActive = !availability.isActive;
    if (this.filterAvailabilityTerms.includes(availability.availability)) {
      this.filterAvailabilityTerms = this.filterAvailabilityTerms.filter(f => f != availability.availability);
    }
    else {
      this.filterAvailabilityTerms.push(availability.availability);
    }
    this.filterAvailability = this.filterAvailabilityTerms.length > 0;
    this.filter();
  }

  private sort() {
    if (this.sortTitle) {
      this.displayedBooks.sort((a, b) => this.helper.compare(a.title, b.title));
    }
    else if (this.sortAuthor) {
      this.displayedBooks.sort((a, b) => this.helper.compare(a.author, b.author));
    }
    else if (this.sortAvailability) {
      this.displayedBooks.sort((a,b) => this.helper.compare(a.isAvailable, b.isAvailable));
    }
  }

  private filter() {
    this.displayedBooks = this.books.filter(d => (this.filterTitleTerms.length == 0 || this.filterTitleTerms.includes(d.title)) && 
                                                          (this.filterAuthorTerms.length == 0 || this.filterAuthorTerms.includes(d.author)) && 
                                                          (this.filterAvailabilityTerms.length == 0 || this.filterAvailabilityTerms.includes("In Stock") && d.isAvailable || this.filterAvailabilityTerms.includes("Out of Stock") && !d.isAvailable));
  }

  clearTitleFilter() {
    this.filterTitleTerms = [];
    this.titles.forEach(t => t.isActive = false);
    this.filterTitle = false;
    this.filter();
  }

  clearAuthorFilter() {
    this.filterAuthorTerms = [];
    this.authors.forEach(a => a.isActive = false);
    this.filterAuthor = false;
    this.filter();
  }

  clearAvailibilityFilter() {
    this.filterAvailabilityTerms = [];
    this.availability.forEach(a => a.isActive = false);
    this.filterAvailability = false;
    this.filter();
  }

}
