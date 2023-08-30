import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

// Componentes Importados
import { NgToastModule } from 'ng-angular-popup'
import { ReactiveFormsModule } from '@angular/forms';
import { AppRoutingModule } from './app-routing.module';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';

// Componentes propios
import { AppComponent } from './app.component';
import { LoginComponent } from './componets/login/login.component';
import { SignUpComponent } from './componets/sign-up/sign-up.component';
import { DashboardComponent } from './componets/dashboard/dashboard.component';
import { NotFoundComponent } from './componets/not-found/not-found.component';
import { NavMenuComponent } from './componets/nav-menu/nav-menu.component';
import { JwtInterceptor } from './services/auth/jwt-interceptor';
import { AuthGuard } from './services/auth/auth-guard';

@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    SignUpComponent,
    DashboardComponent,
    NotFoundComponent,
    NavMenuComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    ReactiveFormsModule,
    HttpClientModule,
    NgToastModule
  ],
  providers: [
    AuthGuard,
    {
      provide: HTTP_INTERCEPTORS,
      useClass: JwtInterceptor,
      multi: true
    }],
  bootstrap: [AppComponent]
})
export class AppModule { }
