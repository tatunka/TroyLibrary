import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { BookComponent } from './book/book.component';
import { BookDetailComponent } from './book/book-detail/book-detail.component';
import { RoleGuard } from './shared/role-guard';
import { ErrorPageComponent } from './error-page/error-page.component';

const routes: Routes = [
  { path: 'book', component: BookComponent, canActivate: [RoleGuard], data: { roles: ['Librarian', 'Customer'] } },
  { path: 'detail/:bookId', component: BookDetailComponent, canActivate: [RoleGuard] },
  { path: 'error', component: ErrorPageComponent },
  { path: '', redirectTo: 'book', pathMatch: 'full' },
  { path: '**', redirectTo: 'error' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
