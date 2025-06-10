import { Component } from '@angular/core';
import { AuthService } from '../../services/auth.service';
import { Router } from '@angular/router';
import { AsyncPipe, NgIf } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [NgIf, AsyncPipe, FormsModule],
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent {
  loginModel = { email: '', password: '' };
  registerModel = { email: '', password: '', fullName: '' };
  isRegister = false;

  constructor(public authService: AuthService, private router: Router) {}

  toggleMode(): void {
    this.isRegister = !this.isRegister;
  }

  submit(): void {
    if (this.isRegister) {
      this.authService.register(this.registerModel.email, this.registerModel.password, this.registerModel.fullName)
        .subscribe(success => {
          console.log('Registration success:', success);
          if (success) this.router.navigate(['/']);
        });
    } else {
      this.authService.login(this.loginModel.email, this.loginModel.password)
        .subscribe(success => {
          console.log('Login success:', success);
          if (success) this.router.navigate(['/']);
        });
    }
  }

  logout(): void {
    this.authService.logout();
    this.router.navigate(['/login']);
  }
}
