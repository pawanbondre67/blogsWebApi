import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { environment } from '../../environments/environment';
import { BlogPost } from '../models/blog-post.model';
import { Category } from '../models/category.model.ts';
import { Comment } from '../models/comment.model.ts';
import { Like } from '../models/like.model.ts';


@Injectable({
  providedIn: 'root'
})
export class BlogService {
  private apiUrl = environment.apiUrl;

  constructor(private http: HttpClient) {}

  private getHeaders(token: string | null): HttpHeaders {
    return token ? new HttpHeaders().set('Authorization', `Bearer ${token}`) : new HttpHeaders();
  }

  private handleError(error: any): Observable<never> {
    return throwError(() => new Error(error.error?.message || 'An error occurred'));
  }

  getPosts(page: number, pageSize: number): Observable<BlogPost[]> {
    return this.http.get<BlogPost[]>(`${this.apiUrl}/BlogPosts?page=${page}&pageSize=${pageSize}`)
      .pipe(catchError(this.handleError));
  }

  getPost(id: number): Observable<BlogPost> {
    return this.http.get<BlogPost>(`${this.apiUrl}/BlogPosts/${id}`)
      .pipe(catchError(this.handleError));
  }

  getByCategory(categoryId: number): Observable<BlogPost[]> {
    return this.http.get<BlogPost[]>(`${this.apiUrl}/BlogPosts/category/${categoryId}`)
      .pipe(catchError(this.handleError));
  }

  createPost(post: { title: string, content: string, categoryId: number }, token: string | null): Observable<BlogPost> {
    return this.http.post<BlogPost>(`${this.apiUrl}/BlogPosts`, post, { headers: this.getHeaders(token) })
      .pipe(catchError(this.handleError));
  }

  updatePost(id: number, post: { id: number, title: string, content: string, categoryId: number }, token: string | null): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/BlogPosts/${id}`, post, { headers: this.getHeaders(token) })
      .pipe(catchError(this.handleError));
  }

  deletePost(id: number, token: string | null): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/BlogPosts/${id}`, { headers: this.getHeaders(token) })
      .pipe(catchError(this.handleError));
  }

  getCategories(): Observable<Category[]> {
    return this.http.get<Category[]>(`${this.apiUrl}/Categories`)
      .pipe(catchError(this.handleError));
  }

  createCategory(category: { name: string }, token: string | null): Observable<Category> {
    return this.http.post<Category>(`${this.apiUrl}/Categories`, category, { headers: this.getHeaders(token) })
      .pipe(catchError(this.handleError));
  }

  updateCategory(id: number, category: { id: number, name: string }, token: string | null): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/Categories/${id}`, category, { headers: this.getHeaders(token) })
      .pipe(catchError(this.handleError));
  }

  deleteCategory(id: number, token: string | null): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/Categories/${id}`, { headers: this.getHeaders(token) })
      .pipe(catchError(this.handleError));
  }

  getComments(postId: number): Observable<Comment[]> {
    return this.http.get<Comment[]>(`${this.apiUrl}/Comments/post/${postId}`)
      .pipe(catchError(this.handleError));
  }

  addComment(comment: { content: string, blogPostId: number }, token: string | null): Observable<Comment> {
    return this.http.post<Comment>(`${this.apiUrl}/Comments`, comment, { headers: this.getHeaders(token) })
      .pipe(catchError(this.handleError));
  }

  deleteComment(id: number, token: string | null): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/Comments/${id}`, { headers: this.getHeaders(token) })
      .pipe(catchError(this.handleError));
  }

  addLike(like: { blogPostId: number }, token: string | null): Observable<Like> {
    return this.http.post<Like>(`${this.apiUrl}/Likes`, like, { headers: this.getHeaders(token) })
      .pipe(catchError(this.handleError));
  }

  deleteLike(id: number, token: string | null): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/Likes/${id}`, { headers: this.getHeaders(token) })
      .pipe(catchError(this.handleError));
  }
}
