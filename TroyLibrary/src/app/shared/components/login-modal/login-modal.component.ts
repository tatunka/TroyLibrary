import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { AuthService } from '../../services/auth.service';
import { Credentials, LoginRequest, LoginResponse, RegisterRequest, RegisterResponse, Role } from '../../models/auth-models';
import { ToastService } from '../toast/toast-service';
import { Router } from '@angular/router';

enum Tab {
  Login,
  Register
}

@Component({
  selector: 'app-login-modal',
  standalone: false,
  templateUrl: './login-modal.component.html',
  styleUrl: './login-modal.component.css'
})
export class LoginModalComponent {
  
  constructor(
    private activeModal: NgbActiveModal, 
    private auth: AuthService, 
    private toastService: ToastService,
    private router: Router
  ) {}

  //form
  form = new FormGroup({
    username: new FormControl('', Validators.required),
    password: new FormControl('', Validators.required)
  });

  roleChoice: Role = Role.Customer;
  activeTab: Tab = Tab.Login;

  //expose enum to template
  public tab = Tab;
  public role = Role;

  selectLoginTab() {
    this.activeTab = Tab.Login;
  }

  selectRegisterTab() {
    this.activeTab = Tab.Register;
  }

  close() {
    this.activeModal.close();
  }

  submit() {
    const credentials: Credentials = {
      username: this.form.value.username as string,
      password: this.form.value.password as string
    }
    if (this.activeTab === Tab.Login) {
      const request: LoginRequest = {
        credentials: credentials,
      }
      this.auth.login(request).subscribe({
        next: (value: LoginResponse) => {
          if (value.token) {
            this.toastService.showSuccess('Log in successful!');
            this.activeModal.close();
            this.router.navigate(['/book']);
          }
          else {
            this.toastService.showError('Incorrect User Name / Password');
          }
        }, 
        error: (error) => this.toastService.showError(error.message)
      });
    }
    else if (this.activeTab === Tab.Register) {
      const request: RegisterRequest = {
        credentials: credentials,
        role: this.roleChoice,
      }
      this.auth.register(request).subscribe({
        next: (value: RegisterResponse) => {
          localStorage.setItem('token', value.token);
          this.toastService.showSuccess('Registration succesful!');
          this.activeModal.close();
          this.router.navigate(['/book']);
        },
        error: (error) => this.toastService.showError(error.message)
      });
    }
  }
}
