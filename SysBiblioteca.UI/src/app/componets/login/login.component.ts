import { Router } from '@angular/router';
import { Component } from '@angular/core';
import ValidateForm from 'src/app/helpers/validateForm';
import { AuthService } from 'src/app/services/auth.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})

export class LoginComponent {
  type: string = "password";
  icon_class: string = "fa fa-eye";
  loginForm!: FormGroup;

  constructor(private fb: FormBuilder, private auth: AuthService, private routes: Router) { }

  ngOnInit(): void {
    this.loginForm = this.fb.group({
      Usuario: ['', Validators.required],
      Contrasenia: ['', Validators.required]
    });
  }

  hideShowPassword() {
    this.type = this.type == "password" ? "text" : "password";
    this.icon_class = this.icon_class == "fa fa-eye" ? "fa fa-eye-slash" : "fa fa-eye"
  }

  onLogin() {
    if (this.loginForm.valid) {
      this.auth.login(this.loginForm.value)
      .subscribe({
        next: (res) => {
          alert(res.mensaje);
          this.loginForm.reset();
          this.routes.navigate(['dashboard']);
        },
        error: (err) => {
          alert(err.mensaje);
        }
      });
    }
    else {
      ValidateForm.validateFields(this.loginForm);
    }
  }
}