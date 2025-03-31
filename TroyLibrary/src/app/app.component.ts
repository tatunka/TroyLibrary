import { Component } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { LoginModalComponent } from './shared/components/login-modal/login-modal.component';
import { AuthService } from './shared/services/auth.service';
import { Router } from '@angular/router';
import { Role } from './shared/models/auth-models';
import { AddbookModalComponent } from './shared/components/addbook-modal/addbook-modal.component';

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

  login(event: MouseEvent) {
    event.preventDefault();
    this.modalService.open(LoginModalComponent, { centered: true })
  }

  logout(event: MouseEvent) {
    event?.preventDefault()
    this.auth.logout();
    window.location.reload();
  }

  addBook(event: MouseEvent) {
    event.preventDefault();
    this.modalService.open(AddbookModalComponent, { centered: true })
  }
}
