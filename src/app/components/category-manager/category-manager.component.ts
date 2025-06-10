import { Component, OnInit } from '@angular/core';
import { BlogService } from '../../services/blog.service';
import { AuthService } from '../../services/auth.service';
import { Category } from '../../models/category.model.ts';
import { AsyncPipe, NgFor, NgIf } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-category-manager',
  standalone: true,
  imports: [NgFor, NgIf, AsyncPipe, FormsModule],
  templateUrl: './category-manager.component.html',
  styleUrls: ['./category-manager.component.scss']
})
export class CategoryManagerComponent implements OnInit {
  categories: Category[] = [];
  newCategory: { name: string } = { name: '' };
  editingCategory: { id: number, name: string } | null = null;

  constructor(
    private blogService: BlogService,
    public authService: AuthService,
  ) {}

  ngOnInit(): void {
    this.loadCategories();
  }

  loadCategories(): void {
    this.blogService.getCategories().subscribe({
      next: data => this.categories = data,
      error: err => console.error(err)
    });
  }

  createCategory(): void {
    if (this.newCategory.name) {
      this.authService.getToken().subscribe({
        next: token => {
          this.blogService.createCategory(this.newCategory, token)
            .subscribe({
              next: () => {
                console.log('Category created successfully');
                this.newCategory.name = '';
                this.loadCategories();
              },
              error: err => console.error(err)
            });
        },
        error: err => console.error(err)
      });
    } else {
      console.error('Category name is required');
    }
  }

  editCategory(category: Category): void {
    this.editingCategory = { id: category.id, name: category.name };
  }

  updateCategory(): void {
    if (this.editingCategory && this.editingCategory.name) {
      this.authService.getToken().subscribe({
        next: token => {
          this.blogService.updateCategory(this.editingCategory!.id, this.editingCategory!, token)
            .subscribe({
              next: () => {
                console.log('Category updated successfully');
                this.editingCategory = null;
                this.loadCategories();
              },
              error: err => console.error(err)
            });
        },
        error: err => console.error(err)
      });
    } else {
      console.error('Category name is required' + (this.editingCategory ? ` for category ID ${this.editingCategory.id}` : ''));
    }
  }

  deleteCategory(id: number): void {
    if (confirm('Are you sure you want to delete this category?')) {
      this.authService.getToken().subscribe({
        next: token => {
          this.blogService.deleteCategory(id, token)
            .subscribe({
              next: () => {
                console.log('Category deleted successfully');
                this.loadCategories();
              },
              error: err => console.error(err)
            });
        },
        error: err => console.error(err)
      });
    }
  }
}
