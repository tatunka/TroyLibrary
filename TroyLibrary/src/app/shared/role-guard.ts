import { Injectable } from "@angular/core";
import { ActivatedRouteSnapshot, CanActivate } from "@angular/router";
import { NgbModal } from "@ng-bootstrap/ng-bootstrap";
import { LoginModalComponent } from "./components/login-modal/login-modal.component";
import { AuthService } from "./services/auth.service";

@Injectable({
    providedIn: 'root'
})
export class RoleGuard implements CanActivate {

    constructor(private modalService: NgbModal, private auth: AuthService) {}

    canActivate(route: ActivatedRouteSnapshot): boolean {
        const userRoles = this.auth.getUserRoles();
        const allowedRoles: string[] = route.data['roles'];
        if (allowedRoles === undefined || allowedRoles?.length === 0 || allowedRoles?.some(role => userRoles.includes(role))) {
            return true;
        }
        this.modalService.open(LoginModalComponent, {centered: true });
        return false;
    }
}