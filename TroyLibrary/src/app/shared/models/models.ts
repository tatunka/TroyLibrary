export enum Category {
    NonFiction = 1,
    ScienceFiction,
    Fantasy,
    Mystery,
    Romance,
    Horror,
    Thriller,
    Biography,
    History,
    SelfHelp
}

export enum Lookups {
    Category = 'Category'
}

export interface CrudResponse {
    completedAt: Date;
}

export interface CreateReviewRequest {
    userId: string;
    bookId: number;
    rating: number;
    text: string;
}

export interface GetReviewsResponse {
    reviews: Review[];
}

export interface Review {
    reviewId: number;
    userName: string;
    rating: number;
    text: string;
}