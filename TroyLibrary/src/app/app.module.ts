import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { BookModule } from './book/book.module';
import { LoginModalComponent } from './shared/components/login-modal/login-modal.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgbModalModule, NgbToastModule } from '@ng-bootstrap/ng-bootstrap';
import { BasicToastComponent } from './shared/components/toast/basic-toast/basic-toast.component';
import { ToastService } from './shared/components/toast/toast-service';
import { JwtModule } from '@auth0/angular-jwt';
import { authInterceptor } from './shared/interceptors/auth-interceptor';
import { AddbookModalComponent } from './shared/components/addbook-modal/addbook-modal.component';
import { ReviewModalComponent } from './shared/components/review-modal/review-modal.component';
import { environment } from '../environments/environment';

export function tokenGetter() {
  return localStorage.getItem("token");
}

@NgModule({
  declarations: [
    AppComponent,
    LoginModalComponent,
    BasicToastComponent,
    AddbookModalComponent,
    ReviewModalComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    BookModule,
    FormsModule,
    NgbModalModule,
    NgbToastModule,
    ReactiveFormsModule,
    JwtModule.forRoot({
      config: {
        tokenGetter: tokenGetter,
        allowedDomains: [environment.JwtIssuer],
        disallowedRoutes: [`https://${environment.JwtIssuer}/api/auth/login`]
      }
    })
  ],
  providers: [
    provideHttpClient(withInterceptors([authInterceptor])),
    ToastService,
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
