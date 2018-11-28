import { BrowserModule } from '@angular/platform-browser';
import { NgModule, InjectionToken } from '@angular/core';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { FlexLayoutModule } from '@angular/flex-layout';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule } from '@angular/forms';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { environment } from './../environments/environment';

import { JwtModule } from '@auth0/angular-jwt';
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

  MAT_LABEL_GLOBAL_OPTIONS,
} from '@angular/material';

import { AuthenticationService } from './_services/authentication.service';

import { DashboardComponent } from './dashboard/dashboard.component';
import { TodoListComponent } from './todo-list/todo-list.component';
import { TodoEditComponent } from './todo-edit/todo-edit.component';
import { TobuyListComponent } from './tobuy-list/tobuy-list.component';
import { TobuyEditComponent } from './tobuy-edit/tobuy-edit.component';
import { LoginComponent } from './login/login.component';


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
    LoginComponent
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

    JwtModule.forRoot({
      config: {
        tokenGetter: tokenGetter,
        // whitelistedDomains: ['localhost:3001'],
        // blacklistedRoutes: ['localhost:3001/auth/']
      }
    }),
  ],
  providers: [
    {
      provide: API_BASE_URL,
      useValue: environment.apiUrl
    },
    { provide: MAT_LABEL_GLOBAL_OPTIONS, useValue: 'auto' }
  ],
  bootstrap: [AppComponent]
})
export class AppModule {
  constructor() {
    // console.log('env', environment);
  }
}
