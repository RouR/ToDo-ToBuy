import { BrowserModule } from '@angular/platform-browser';
import { NgModule, InjectionToken } from '@angular/core';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { FlexLayoutModule } from '@angular/flex-layout';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { FormsModule } from '@angular/forms';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { environment } from './../environments/environment';

import { JwtModule, JwtInterceptor } from '@auth0/angular-jwt';
import { API_BASE_URL } from '../_tsModels/api-client';

import {
  MatSidenavModule,
  MatToolbarModule,
  MatIconModule,
  MatListModule,
  MatCardModule,
  MatButtonModule,
  MatTableModule,
  MatDialogModule,
  MatInputModule,
  MatSelectModule,
  MatSnackBarModule,
  MatPaginatorModule,
  MatProgressSpinnerModule,
  MatSortModule,

  MAT_LABEL_GLOBAL_OPTIONS,
} from '@angular/material';

import { AuthenticationService } from './_services/authentication.service';
import { ErrorDtoInterceptorService } from './_services/error-dto-interceptor.service';

import { DashboardComponent } from './dashboard/dashboard.component';
import { TodoListComponent } from './todo-list/todo-list.component';
import { TodoEditComponent } from './todo-edit/todo-edit.component';
import { TobuyListComponent } from './tobuy-list/tobuy-list.component';
import { TobuyEditComponent } from './tobuy-edit/tobuy-edit.component';
import { LoginComponent } from './login/login.component';
import { RegisterComponent } from './register/register.component';

export function tokenGetter() {
  return localStorage.getItem('jwt');
}


@NgModule({
  declarations: [
    AppComponent,
    DashboardComponent,
    TodoListComponent,
    TodoEditComponent,
    TobuyListComponent,
    TobuyEditComponent,
    LoginComponent,
    RegisterComponent
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    FlexLayoutModule,
    AppRoutingModule,
    HttpClientModule,
    FormsModule,

    MatSidenavModule,
    MatToolbarModule,
    MatIconModule,
    MatListModule,
    MatCardModule,
    MatButtonModule,
    MatTableModule,
    MatDialogModule,
    MatInputModule,
    MatSelectModule,
    MatSnackBarModule,
    HttpClientModule,
    MatInputModule,
    MatTableModule,
    MatPaginatorModule,
    MatSortModule,
    MatProgressSpinnerModule,

    JwtModule.forRoot({
      config: {
        // headerName: 'token',
        tokenGetter: tokenGetter,
        whitelistedDomains: [location.host],
        // blacklistedRoutes: ['localhost:3001/auth/']
      }
    }),
  ],
  providers: [
    { provide: API_BASE_URL, useValue: environment.apiUrl },
    { provide: MAT_LABEL_GLOBAL_OPTIONS, useValue: 'auto' },
    { provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true },
    { provide: HTTP_INTERCEPTORS, useClass: ErrorDtoInterceptorService, multi: true },

  ],
  bootstrap: [AppComponent]
})
export class AppModule {
  constructor() {
    // console.log('env', environment);
  }
}
