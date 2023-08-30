import { Subscription } from 'rxjs';
import { Router } from '@angular/router';
import { Component } from '@angular/core';
import { NgToastService } from 'ng-angular-popup';
import ValidateForm from 'src/app/helpers/validateForm';
import { DataService } from 'src/app/services/data.service';
import { SecurityService } from 'src/app/services/security.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { IResponse } from 'src/app/models/i-response';

@Component({
  selector: 'app-sign-up',
  templateUrl: './sign-up.component.html',
  styleUrls: ['./sign-up.component.css']
})

export class SignUpComponent {
  userForm!: FormGroup;
  type: string = "password";
  labelText: string = "Mostrar";
  subRef$: Subscription = new Subscription();
  
  constructor(
    private fb: FormBuilder, 
    private routes: Router,
    private dataService: DataService,
    private securityService: SecurityService,
    private toast: NgToastService
    ) { }

  ngOnInit() : void {
    if (this.securityService.IsAuthorized)
      this.routes.navigate(['dashboard']);

    this.userForm = this.fb.group({
      Usuario: ['', Validators.required],
      Contrasenia: ['', Validators.required],
      RepetirContrasenia: ['', Validators.required],

      Nombres: ['', Validators.required],
      Apellidos: ['', Validators.required],
      DUI: ['', Validators.required],
      Correo: ['', Validators.required],
      Telefono: ['', Validators.required],
      Direccion: ['', Validators.required],
      FechaNacimiento: ['', Validators.required]
    });
  }

  onSignUp() {
    if (this.userForm.valid) {
      var newUser = {
        Usuario: this.userForm.get("Usuario")?.value,
        Contrasenia: this.userForm.get("Contrasenia")?.value,
        DatosPersonales: {
          Nombres: this.userForm.get("Nombres")?.value,
          Apellidos: this.userForm.get("Apellidos")?.value,
          DUI: this.userForm.get("DUI")?.value,
          Correo: this.userForm.get("Correo")?.value,
          Direccion: this.userForm.get("Direccion")?.value,
          Telefono: this.userForm.get("Telefono")?.value,
          FechaNacimiento: this.formatDate(this.userForm.get("FechaNacimiento")?.value)
        }
      };
      
      const url = 'https://localhost:7174/SysBiblioteca/API/Authentication/Register';
      this.subRef$ = this.dataService.POST<IResponse>(url, newUser)
        .subscribe(res => {
          this.toast.success({
            detail: "Éxito",
            summary: res.body?.mensaje,
            duration: 5000
          });

          this.userForm.reset();
          this.routes.navigate(['login']);
        }, err => {
          this.toast.error({
            detail: "Error",
            summary: err.body?.mensaje,
            duration: 5000
          });
        });
    }
    else {
      ValidateForm.validateFields(this.userForm);
    }
  }

  formatDate(date: string){
    return this.parse2Digits(date.split('-')[2]) +'/'+  this.parse2Digits(date.split('-')[1]) +'/'+ date.split('-')[0];
  }

  parse2Digits(str: string){
    return str.length < 2 ? 0+str : str;
  }
}