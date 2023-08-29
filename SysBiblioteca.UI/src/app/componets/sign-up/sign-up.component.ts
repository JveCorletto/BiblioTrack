import { Router } from '@angular/router';
import { Component } from '@angular/core';
import ValidateForm from 'src/app/helpers/validateForm';
import { AuthService } from 'src/app/services/auth.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-sign-up',
  templateUrl: './sign-up.component.html',
  styleUrls: ['./sign-up.component.css']
})
export class SignUpComponent {
  type: string = "password";
  labelText: string = "Mostrar";
  userForm!: FormGroup;
  
  constructor(private fb: FormBuilder, private auth: AuthService, private routes: Router) { }

  ngOnInit() : void {
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

      this.auth.signUp(newUser)
      .subscribe({
        next: (res) => {
          alert(res.mensaje);
          this.userForm.reset();
          this.routes.navigate(['login']);
        },
        error: (err) => {
          alert(err.mensaje);
        }
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
