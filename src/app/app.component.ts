import { Component } from '@angular/core';
import { Router, RouterOutlet } from '@angular/router';
import { AuthService } from './services/auth.service';
import { NgIf, AsyncPipe } from '@angular/common';
import { MaterialModule } from './Material/Material.module';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, NgIf, AsyncPipe , RouterModule, MaterialModule],
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  constructor(public authService: AuthService,
              private router: Router) {
    console.log('AppComponent initialized');
  }

   logout(): void {
    console.log('Logout triggered from AppComponent');
    this.authService.logout();
    this.router.navigate(['/']).then(() => {
      console.log('Navigated to login page after logout');
    }).catch(err => {
      console.error('Navigation error after logout:', err);
    });
  }
}
