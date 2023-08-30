import { Subscription } from 'rxjs';
import { Router } from '@angular/router';
import { ILogin } from 'src/app/models/i-login';
import { NgToastService } from 'ng-angular-popup';
import { IResponse } from 'src/app/models/i-response';
import ValidateForm from 'src/app/helpers/validateForm';
import { DataService } from 'src/app/services/data.service';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { SecurityService } from 'src/app/services/security.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})

export class LoginComponent implements OnInit, OnDestroy {
  loginForm!: FormGroup;
  type: string = "password";
  icon_class: string = "fa fa-eye";
  subRef$: Subscription = new Subscription();
  
  constructor(
    private fb: FormBuilder,
    private routes: Router,
    private dataService: DataService,
    private securityService: SecurityService,
    private toast: NgToastService
  ) { }

  ngOnInit(): void {
    if (this.securityService.IsAuthorized)
      this.routes.navigate(['dashboard']);

    this.loginForm = this.fb.group({
      Usuario: ['', Validators.required],
      Contrasenia: ['', Validators.required]
    }); 
  }

  hideShowPassword() {
    this.type = this.type == "password" ? "text" : "password";
    this.icon_class = this.icon_class == "fa fa-eye" ? "fa fa-eye-slash" : "fa fa-eye"
  }

  Login() {
    if (this.loginForm.valid) {
      const usuarioLogin: ILogin = {
        Usuario: this.loginForm.value.Usuario,
        Contrasenia: this.loginForm.value.Contrasenia
      }
      
      const url = 'https://localhost:7174/SysBiblioteca/API/Authentication/LogIn';
      this.subRef$ = this.dataService.POST<IResponse>(url, usuarioLogin)
        .subscribe(res => {
          this.toast.success({
            detail: "Éxito",
            summary: res.body?.mensaje,
            duration: 5000
          });

          this.loginForm.reset();
          this.securityService.setAuthData(res.body?.datos.usuario, res.body?.datos.token);
          this.routes.navigate(['dashboard']);
        }, err => {
          this.toast.error({
            detail: "Error",
            summary: err.body?.mensaje,
            duration: 5000
          });
        });
    }
    else {
      ValidateForm.validateFields(this.loginForm);
    }
  }

  ngOnDestroy(){
    if (this.subRef$) {
      this.subRef$.unsubscribe();
    }
  }
}