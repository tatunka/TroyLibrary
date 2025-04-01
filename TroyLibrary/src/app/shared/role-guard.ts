import { Injectable } from "@angular/core";
import { ActivatedRouteSnapshot, CanActivate, Router } from "@angular/router";
import { NgbModal } from "@ng-bootstrap/ng-bootstrap";
import { LoginModalComponent } from "./components/login-modal/login-modal.component";
import { AuthService } from "./services/auth.service";

@Injectable({
    providedIn: 'root'
})
export class RoleGuard implements CanActivate {

    constructor(
        private modalService: NgbModal, 
        private auth: AuthService,
        private router: Router
    ) {}

    canActivate(route: ActivatedRouteSnapshot): boolean {
        const userRoles = this.auth.getUserRoles();
        const allowedRoles: string[] = route.data['roles'];
        if (allowedRoles === undefined || allowedRoles?.length === 0 || allowedRoles?.some(role => userRoles.includes(role))) {
            return true;
        }
        this.router.navigate(['/error']);
        this.modalService.open(LoginModalComponent, {centered: true, backdrop: 'static' });
        return false;
    }
}