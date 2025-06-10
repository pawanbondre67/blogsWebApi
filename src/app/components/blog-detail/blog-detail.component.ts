import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { BlogService } from '../../services/blog.service';
import { AuthService } from '../../services/auth.service';
import { BlogPost} from '../../models/blog-post.model';
import { Comment } from '../../models/comment.model.ts';
import { AsyncPipe, NgFor, NgIf, DatePipe } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-blog-detail',
  standalone: true,
  imports: [NgFor, NgIf, AsyncPipe, DatePipe, FormsModule],
  templateUrl: './blog-detail.component.html',
  styleUrls: ['./blog-detail.component.scss']
})
export class BlogDetailComponent implements OnInit {
  post: BlogPost | null = null;
  comments: Comment[] = [];
  newComment: string = '';
  currentUserId: string | null = null;

  constructor(
    private route: ActivatedRoute,
    private blogService: BlogService,
    public authService: AuthService,

  ) {}

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id')!;
    this.loadPost(+id);
    this.loadComments(+id);
    this.authService.getUserId().subscribe(id => this.currentUserId = id);
  }

  loadPost(id: number): void {
    this.blogService.getPost(id).subscribe({
      next: data => this.post = data,
      error: err => console.error(err)
    });
  }

  loadComments(postId: number): void {
    this.blogService.getComments(postId).subscribe({
      next: data => this.comments = data,
      error: err => console.error(err)
    });
  }

  addComment(): void {
    if (this.newComment && this.post) {
      this.authService.getToken().subscribe({
        next: token => {
          this.blogService.addComment({ content: this.newComment, blogPostId: this.post!.id }, token)
            .subscribe({
              next: () => {
                console.log('Comment added successfully');
                this.newComment = '';
                this.loadPost(this.post!.id);
                this.loadComments(this.post!.id);
              },
              error: err => console.error(err)
            });
        },
        error: err => console.error(err)
      });
    }
  }

  deleteComment(id: number): void {
    if (confirm('Are you sure you want to delete this comment?')) {
      this.authService.getToken().subscribe({
        next: token => {
          this.blogService.deleteComment(id, token).subscribe({
            next: () => {
              console.log('Comment deleted successfully');
              this.loadComments(this.post!.id);
            },
            error: err => console.error(err)
          });
        },
        error: err => console.error(err)
      });
    }
  }

  addLike(): void {
    if (this.post) {
      this.authService.getToken().subscribe({
        next: token => {
          this.blogService.addLike({ blogPostId: this.post!.id }, token)
            .subscribe({
              next: () => {
                console.log('Like added successfully');
                this.loadPost(this.post!.id);
              },
              error: err => console.error(err)
            });
        },
        error: err => console.error(err)
      });
    }
  }
}
