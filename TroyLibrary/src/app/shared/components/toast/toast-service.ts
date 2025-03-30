import { Injectable } from "@angular/core";

export interface ToastInfo {
    body: string;
    delay?: number;
    classname: string
}

@Injectable({ providedIn: 'root' })
export class ToastService {
    
    toasts: ToastInfo[] = [];

    show(body: string, classname: string){
        this.toasts.push({ body: body, classname: classname })
    }

    remove(toast: ToastInfo) {
        this.toasts = this.toasts.filter(t => t != toast);
    }
}