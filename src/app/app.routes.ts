import { Routes } from '@angular/router';
import { BlogListComponent } from './components/blog-list/blog-list.component';
import { BlogDetailComponent } from './components/blog-detail/blog-detail.component';
import { BlogFormComponent } from './components/blog-form/blog-form.component';
import { CategoryManagerComponent } from './components/category-manager/category-manager.component';
import { LoginComponent } from './components/login/login.component';

export const routes: Routes = [
  { path: '', component: BlogListComponent },
  { path: 'blog/:id', component: BlogDetailComponent },
  { path: 'create', component: BlogFormComponent },
  { path: 'edit/:id', component: BlogFormComponent },
  { path: 'categories', component: CategoryManagerComponent },
  { path: 'login', component: LoginComponent },
  { path: '**', redirectTo: '' },
];
