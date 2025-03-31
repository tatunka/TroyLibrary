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
    completedAt: Date
}