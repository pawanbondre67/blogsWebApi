import { Component, OnInit } from '@angular/core';
import { BlogService } from '../../services/blog.service';
import { AuthService } from '../../services/auth.service';

import { BlogPost } from '../../models/blog-post.model';
import { Category } from '../../models/category.model.ts';
import { AsyncPipe, CommonModule, DatePipe, NgFor, NgIf, SlicePipe } from '@angular/common';
import { RouterLink } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { MaterialModule } from '../../Material/Material.module';

@Component({
  selector: 'app-blog-list',
  standalone: true,
  imports: [ RouterLink, FormsModule , MaterialModule , CommonModule],
  templateUrl: './blog-list.component.html',
  styleUrls: ['./blog-list.component.scss']
})
export class BlogListComponent implements OnInit {
  posts: BlogPost[] = [];
  categories: Category[] = [];
  currentPage = 1;
  pageSize = 10;
  selectedCategoryId: number | null = null;
  currentUserId: string | null = null;


  constructor(
    private blogService: BlogService,
    public authService: AuthService,

  ) {}

  ngOnInit(): void {
    this.loadPosts();
    this.loadCategories();
    this.authService.getUserId().subscribe(id => this.currentUserId = id);

  }

  loadPosts(): void {
    const observable = this.selectedCategoryId
      ? this.blogService.getByCategory(this.selectedCategoryId)
      : this.blogService.getPosts(this.currentPage, this.pageSize);
    observable.subscribe({
      next: data => {
          this.posts = data;
          console.log('BlogListComponent initialized', this.posts);
      },
      error: err => console.error(err)
    });
  }

  loadCategories(): void {
    this.blogService.getCategories().subscribe({
      next: data => this.categories = data,
      error: err => console.error(err)
    });
  }

  filterByCategory(categoryId: string): void {
    this.selectedCategoryId = categoryId ? +categoryId : null;
    this.currentPage = 1;
    this.loadPosts();
  }

  nextPage(): void {
    this.currentPage++;
    this.loadPosts();
  }

  previousPage(): void {
    if (this.currentPage > 1) {
      this.currentPage--;
      this.loadPosts();
    }
  }

  deletePost(id: number): void {
    if (confirm('Are you sure you want to delete this post?')) {
      this.authService.getToken().subscribe({
        next: token => {
          this.blogService.deletePost(id, token).subscribe({
            next: () => {
              console.log('Post deleted successfully');
              this.loadPosts();
            },
            error: err => console.error(err)
          });
        },
        error: err => console.error(err)
      });
    }
  }
}
