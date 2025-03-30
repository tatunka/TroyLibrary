import { Component } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { LoginModalComponent } from './shared/components/login-modal/login-modal.component';
import { AuthService } from './shared/services/auth.service';
import { Router } from '@angular/router';
import { Role } from './shared/models/auth-models';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  standalone: false,
  styleUrl: './app.component.css'
})
export class AppComponent {
  title = 'TroyLibrary';

  constructor(private modalService: NgbModal, protected auth: AuthService, private router: Router) {}

  //expose enum to template
  public role = Role;

  login() {
    this.modalService.open(LoginModalComponent, {centered: true })
  }

  logout() {
    this.auth.logout();
    this.router.navigate(['/']);
  }
}
