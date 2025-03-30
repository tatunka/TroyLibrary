export interface RegisterRequest {
    credentials: Credentials
    email?: string;
    role: Role;
}

export interface LoginRequest {
    credentials: Credentials;
}

export interface LoginResponse {
    token: string;
}

export interface RegisterResponse {
    token: string;
    errors: IdentityError[];
}

export interface IdentityError {
    code: string;
    description: string;
}

export interface Credentials {
    username: string;
    password: string;
}

export enum Role {
    Librarian = 1, 
    Customer
}