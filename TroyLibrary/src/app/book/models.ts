import { Category } from "../shared/models/models";

export interface BookDetail extends Book {
    publisher: string;
    publicationDate: Date;
    isbn: string;
    pageCount: number;
    reviews: Review[];
    category: Category;
    categoryName: string;
    CheckoutDate: Date;
}

export interface BookData {
    bookId?: number;
    title: string;
    author: string;
    description: string;
    coverImage: string;
    publisher: string;
    publicationDate: Date;
    category: Category;
    isbn: string;
    pageCount: number;
}

export interface Review {
    reviewId: number;
    userName: string;
    rating: number;
    text: string;
}

export interface Book {
    bookId: number;
    title: string;
    author: string;
    description: string;
    coverImage: string;
    rating: number;
    isAvailable: boolean
}

export interface GetBooksResponse {
    books: Book[];
}

export interface GetBookResponse {
    bookDetail: BookDetail;
}

export interface BookRequest {
    bookData: BookData
}