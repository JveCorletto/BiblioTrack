import { Subscription } from 'rxjs';
import { Router } from '@angular/router';
import { Component } from '@angular/core';
import { ILogin } from 'src/app/models/i-login';
import { NgToastService } from 'ng-angular-popup';
import { IResponse } from 'src/app/models/i-response';
import { DataService } from 'src/app/services/data.service';
import { SecurityService } from 'src/app/services/security.service';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent {
  subRef$: Subscription = new Subscription();

  constructor(
    private routes: Router,
    private dataService: DataService,
    private securityService: SecurityService,
    private toast: NgToastService
  ) { }

  logOut() {
    const usuarioLogin: ILogin = {
      Usuario: String(this.securityService.getUserName()),
      Contrasenia: ''
    };

    const url = 'https://localhost:7174/SysBiblioteca/API/Authentication/LogOut';
    this.subRef$ = this.dataService.POST<IResponse>(url, usuarioLogin)
      .subscribe(res => {
        this.toast.info({
          detail: "Sesión cerrada exitósamente",
          summary: res.body?.mensaje,
          duration: 5000
        });

        this.securityService.logOut();
        this.refresh();
      }, err => {
        this.toast.error({
          detail: "Error",
          summary: err.body?.mensaje,
          duration: 5000
        });
      });
  }

  refresh(): void {
    window.location.reload();
  }

  ngOnDestroy() {
    if (this.subRef$) {
      this.subRef$.unsubscribe();
    }
  }
}