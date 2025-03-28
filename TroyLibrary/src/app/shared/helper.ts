import { Injectable } from "@angular/core";

@Injectable({
    providedIn: 'root',
})
export class Helper {

    public compare(a: any, b: any): number {
        if (a < b) {
            return -1;
        }
        if (a > b) {
            return 1;
        }
        return 0;
    }
}