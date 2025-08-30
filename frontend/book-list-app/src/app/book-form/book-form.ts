import { Component, EventEmitter, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { BookService } from '../services/book.service';
import { Book } from '../models/book.model';

@Component({
  selector: 'app-book-form',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './book-form.html',
  styleUrl: './book-form.css'
})
export class BookFormComponent {
  @Output() bookAdded = new EventEmitter<void>();

  book: Partial<Book> = {
    title: '',
    author: '',
    isbn: '',
    publishedDate: '',
    genre: '',
    price: 0,
    description: ''
  };

  loading = false;
  error: string | null = null;

  constructor(private bookService: BookService) {}

  onSubmit(): void {
    if (this.isFormValid()) {
      this.loading = true;
      this.error = null;

      this.bookService.createBook(this.book as Book).subscribe({
        next: () => {
          this.loading = false;
          this.resetForm();
          this.bookAdded.emit();
        },
        error: (error) => {
          this.loading = false;
          this.error = 'Failed to add book. Please try again.';
          console.error('Error adding book:', error);
        }
      });
    }
  }

  private isFormValid(): boolean {
    return !!(this.book.title && this.book.author && this.book.isbn && 
              this.book.publishedDate && this.book.genre);
  }

  private resetForm(): void {
    this.book = {
      title: '',
      author: '',
      isbn: '',
      publishedDate: '',
      genre: '',
      price: 0,
      description: ''
    };
  }
}
