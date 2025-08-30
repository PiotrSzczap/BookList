import { Component } from '@angular/core';
import { bootstrapApplication } from '@angular/platform-browser';

@Component({
  selector: 'app-test',
  standalone: true,
  template: `
    <div style="background: yellow; padding: 20px; border: 3px solid black;">
      <h1>ANGULAR IS WORKING!</h1>
      <p>Component loaded successfully at {{ currentTime }}</p>
      <button (click)="updateTime()">Update Time</button>
    </div>
  `
})
export class TestApp {
  currentTime = new Date().toLocaleTimeString();
  
  constructor() {
    console.log('TestApp constructor called');
  }
  
  updateTime() {
    this.currentTime = new Date().toLocaleTimeString();
    console.log('Time updated:', this.currentTime);
  }
}

bootstrapApplication(TestApp)
  .then(() => console.log('Bootstrap successful'))
  .catch(err => console.error('Bootstrap failed:', err));
