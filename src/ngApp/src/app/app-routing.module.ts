import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AuthenticationService } from './_services/authentication.service';
import { DashboardComponent } from './dashboard/dashboard.component';
import { TodoListComponent } from './todo-list/todo-list.component';
import { TobuyListComponent } from './tobuy-list/tobuy-list.component';
import { TodoEditComponent } from './todo-edit/todo-edit.component';
import { TobuyEditComponent } from './tobuy-edit/tobuy-edit.component';
import { LoginComponent } from './login/login.component';
import { RegisterComponent } from './register/register.component';



const routes: Routes = [
  { path: '', component: DashboardComponent },
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },
  { path: 'dashboard', component: DashboardComponent },

  { path: 'todo', component: TodoListComponent, canActivate: [AuthenticationService] },
  { path: 'todo/:id', component: TodoEditComponent, canActivate: [AuthenticationService] },
  { path: 'tobuy', component: TobuyListComponent, canActivate: [AuthenticationService] },
  { path: 'tobuy/:id', component: TobuyEditComponent, canActivate: [AuthenticationService] },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
