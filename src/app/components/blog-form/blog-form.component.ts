import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { BlogService } from '../../services/blog.service';
import { AuthService } from '../../services/auth.service';

import { Category } from '../../models/category.model.ts';
import { AsyncPipe, NgFor, NgIf } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-blog-form',
  standalone: true,
  imports: [NgFor, NgIf, AsyncPipe, FormsModule],
  templateUrl: './blog-form.component.html',
  styleUrls: ['./blog-form.component.scss']
})
export class BlogFormComponent implements OnInit {
  post: { id?: number, title: string, content: string, categoryId: number } = { title: '', content: '', categoryId: 0 };
  categories: Category[] = [];
  isEdit = false;

  constructor(
    private blogService: BlogService,
    public authService: AuthService,
    private router: Router,
    private route: ActivatedRoute
  ) {}

  ngOnInit(): void {
    this.loadCategories();
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.isEdit = true;
      this.blogService.getPost(+id).subscribe({
        next: post => {
          this.post = { id: post.id, title: post.title, content: post.content, categoryId: post.categoryId };
        },
        error: err => console.error(err)
      });
    }
  }

  loadCategories(): void {
    this.blogService.getCategories().subscribe({
      next: data => this.categories = data,
      error: err => console.error(err)
    });
  }

  savePost(): void {
    if (this.post.title && this.post.content && this.post.categoryId) {
      this.authService.getToken().subscribe({
        next: token => {
          if (this.isEdit && this.post.id !== undefined) {
            const observable = this.blogService.updatePost(this.post.id, this.post as { id: number; title: string; content: string; categoryId: number }, token);
            observable.subscribe({
              next: () => {
                console.log('Post updated successfully');
                this.router.navigate(['/']);
              },
              error: (err: any) => console.error(err)
            });
          } else {
            const observable = this.blogService.createPost(this.post, token);
            observable.subscribe({
              next: () => {
                console.log('Post created successfully');
                this.router.navigate(['/']);
              },
              error: (err: any) => console.error(err)
            });
          }
        },
        error: (err: any) => console.error(err)
      });
    } else {
      console.error('Please fill all required fields');
    }
  }
}
