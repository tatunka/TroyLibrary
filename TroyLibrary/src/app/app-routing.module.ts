import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { BookComponent } from './book/book.component';
import { BookDetailComponent } from './book/book-detail/book-detail.component';
import { RoleGuard } from './shared/role-guard';

const routes: Routes = [
  { path: 'book', component: BookComponent, canActivate: [RoleGuard], data: { roles: ['Librarian', 'Customer'] } },
  { path: 'detail/:bookId', component: BookDetailComponent, canActivate: [RoleGuard] },
  { path: '', redirectTo: 'book', pathMatch: 'full' },
  { path: '**', redirectTo: '/404' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
