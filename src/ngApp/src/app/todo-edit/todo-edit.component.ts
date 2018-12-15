import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { TodoPublicEntity, Client, SaveTODORequest } from 'src/_tsModels/api-client';

@Component({
  selector: 'app-todo-edit',
  templateUrl: './todo-edit.component.html',
  styleUrls: ['./todo-edit.component.css']
})
export class TodoEditComponent implements OnInit {

  id: string;

  model: TodoPublicEntity;
  isNewRecord: boolean;
  isLoading: boolean;

  constructor(
    private route: ActivatedRoute,
    private api: Client
  ) { }

  ngOnInit() {
    this.id = this.route.snapshot.params['id'];
    this.model = new TodoPublicEntity();
    if (!this.id) {
      this.isNewRecord = true;
    } else {
      this.isLoading = true;
      this.api.apiTodoGet(this.id).subscribe(
        result => {
          this.model = result;
        },
        error => { },
        () => {
          this.isLoading = false;
        }
      );
    }
  }

  save() {
    if (this.isLoading) {
      return;
    }

    const req = new SaveTODORequest({
      publicId: this.isNewRecord ? null : this.model.publicId,
      description: this.model.description,
      title: this.model.title,
    });
    if (this.isNewRecord) {
      this.createFunc(req);
    } else {
      this.saveFunc(req);
    }
  }

  createFunc(data: SaveTODORequest) {
    this.isLoading = true;
    this.api.apiTodoCreate(data).subscribe(
      result => {
        if (!result.hasError) {
          this.model = result.data;
          this.isNewRecord = false;
        }
      },
      error => { },
      () => {
        this.isLoading = false;
      }
    );
  }

  saveFunc(data: SaveTODORequest) {
    this.isLoading = true;
    this.api.apiTodoUpdate(data).subscribe(
      result => {
        if (!result.hasError) {
          this.model = result.data;
        }
      },
      error => { },
      () => {
        this.isLoading = false;
      }
    );
  }

  get diagnostic() {
    return JSON.stringify(this.model);
  }
}
