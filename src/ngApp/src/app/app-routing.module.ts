import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { DashboardComponent } from './dashboard/dashboard.component';
import { TodoListComponent } from './todo-list/todo-list.component';
import { TobuyListComponent } from './tobuy-list/tobuy-list.component';
import { TodoEditComponent } from './todo-edit/todo-edit.component';
import { TobuyEditComponent } from './tobuy-edit/tobuy-edit.component';

const routes: Routes = [
  {path: '', component: DashboardComponent},
  {path: 'dashboard', component: DashboardComponent},
  {path: 'todo', component: TodoListComponent},
  {path: 'todo/:id', component: TodoEditComponent },
  {path: 'tobuy', component: TobuyListComponent},
  {path: 'tobuy/:id', component: TobuyEditComponent},
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
