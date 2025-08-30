import { Component } from '@angular/core';
import { BookListComponent } from './book-list/book-list.component';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [BookListComponent],
  template: `
    <div style="padding: 20px; background: #f5f5f5; min-height: 100vh;">
      <h1 style="text-align: center; color: #333; margin-bottom: 30px;">📚 Book List Application</h1>
      <app-book-list></app-book-list>
    </div>
  `
})
export class App {
  constructor() {
    console.log('App component constructor called');
  }
}
