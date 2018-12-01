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
    const credentials = new RegisterRequest({
      userName: form.value.username,
      password: form.value.password
    });
    await this.auth.register(credentials).subscribe(
      result => {
        // console.log('RegisterComponent', result);
        this.validationErrors = result.validationErrors.map(x => x.value);
      }
    );
  }

}
