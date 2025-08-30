import { bootstrapApplication } from '@angular/platform-browser';
import { provideHttpClient } from '@angular/common/http';
import { App } from './app/app';

console.log('Angular bootstrap starting...');

bootstrapApplication(App, {
  providers: [
    provideHttpClient()
  ]
})
  .then(() => {
    console.log('Angular bootstrap successful');
  })
  .catch((err) => {
    console.error('Angular bootstrap error:', err);
  });
