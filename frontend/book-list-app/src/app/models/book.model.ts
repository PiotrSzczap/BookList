export interface Book {
  id: string;
  title: string;
  author: string;
  isbn: string;
  publishedDate: string;
  genre: string;
  price: number;
  description: string;
  contentLink?: string;
}
