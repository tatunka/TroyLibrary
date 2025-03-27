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
    books: []
}