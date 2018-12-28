import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { RegisterRequest } from '../../_tsModels/api-client';
import { AuthenticationService } from '../_services/authentication.service';
import { tap } from 'rxjs/operators';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {

  validationErrors: string[];

  constructor(
    public auth: AuthenticationService,
  ) { }

  ngOnInit() {
  }

  async register(form: NgForm) {
    // console.log('NgForm', form);
    const credentials = new RegisterRequest({
      name: form.value.name,
      email: form.value.email,
      password: form.value.password
    });
    const result = await this.auth.register(credentials);
    console.log('RegisterComponent', result);
    if (result.validationErrors) {
      this.validationErrors = result.validationErrors.map(x => x.value);
    }
  }

}
