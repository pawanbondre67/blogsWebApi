export interface BlogPost {
  id: number;
  title: string;
  content: string;
  createdAt: string;
  updatedAt: string;
  categoryId: number;
  categoryName: string;
  authorId: string;
  authorName: string;
  commentCount: number;
  likeCount: number;
}
