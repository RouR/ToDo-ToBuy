import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { LoginRequest } from '../../_tsModels/api-client';
import { AuthenticationService } from '../_services/authentication.service';


@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  constructor(
    public auth: AuthenticationService,
  ) { }

  ngOnInit() {
  }

  async login(form: NgForm) {
    const credentials = new LoginRequest({
      userName: form.value.username,
      password: form.value.password
    });
    await this.auth.login(credentials);
  }

}
