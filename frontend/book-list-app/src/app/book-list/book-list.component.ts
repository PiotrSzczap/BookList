import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BookService } from '../services/book.service';
import { Book } from '../models/book.model';

@Component({
  selector: 'app-book-list',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './book-list.html',
  styleUrl: './book-list.css'
})
export class BookListComponent implements OnInit {
  @ViewChild('fileInput') fileInput!: ElementRef<HTMLInputElement>;
  
  books: Book[] = [];
  loading = false;
  error: string | null = null;
  selectedBookId: string | null = null;

  constructor(private bookService: BookService) {}

  ngOnInit(): void {
    console.log('BookListComponent ngOnInit called');
    this.loadBooks();
  }

  loadBooks(): void {
    console.log('loadBooks called');
    this.loading = true;
    this.error = null;
    
    this.bookService.getBooks().subscribe({
      next: (books) => {
        console.log('Books received:', books);
        this.books = books;
        this.loading = false;
      },
      error: (error) => {
        console.error('Error loading books:', error);
        this.error = 'Failed to load books. Please try again.';
        this.loading = false;
      }
    });
  }

  deleteBook(id: string): void {
    if (confirm('Are you sure you want to delete this book?')) {
      this.bookService.deleteBook(id).subscribe({
        next: () => {
          this.books = this.books.filter(book => book.id !== id);
        },
        error: (error) => {
          this.error = 'Failed to delete book. Please try again.';
          console.error('Error deleting book:', error);
        }
      });
    }
  }

  selectFileForUpload(bookId: string): void {
    this.selectedBookId = bookId;
    this.fileInput.nativeElement.click();
  }

  onFileSelected(event: any): void {
    const file = event.target.files[0];
    if (file && this.selectedBookId) {
      this.uploadContent(this.selectedBookId, file);
      // Reset the input
      event.target.value = '';
      this.selectedBookId = null;
    }
  }

  uploadContent(bookId: string, file: File): void {
    this.loading = true;
    this.bookService.uploadBookContent(bookId, file).subscribe({
      next: (response) => {
        console.log('Content uploaded successfully:', response);
        this.loadBooks(); // Refresh the list to show updated content status
        this.loading = false;
      },
      error: (error) => {
        this.error = 'Failed to upload content. Please try again.';
        console.error('Error uploading content:', error);
        this.loading = false;
      }
    });
  }

  downloadContent(bookId: string): void {
    this.bookService.downloadBookContent(bookId).subscribe({
      next: (blob) => {
        const book = this.books.find(b => b.id === bookId);
        const filename = book ? `${book.title}_content` : 'book_content';
        
        const url = window.URL.createObjectURL(blob);
        const a = document.createElement('a');
        a.href = url;
        a.download = filename;
        document.body.appendChild(a);
        a.click();
        document.body.removeChild(a);
        window.URL.revokeObjectURL(url);
      },
      error: (error) => {
        this.error = 'Failed to download content. Please try again.';
        console.error('Error downloading content:', error);
      }
    });
  }

  deleteContent(bookId: string): void {
    if (confirm('Are you sure you want to delete the content for this book?')) {
      this.bookService.deleteBookContent(bookId).subscribe({
        next: () => {
          console.log('Content deleted successfully');
          this.loadBooks(); // Refresh the list to show updated content status
        },
        error: (error) => {
          this.error = 'Failed to delete content. Please try again.';
          console.error('Error deleting content:', error);
        }
      });
    }
  }
}
