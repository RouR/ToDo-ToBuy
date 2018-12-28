import { Injectable } from '@angular/core';
import { Router, CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { Observable } from 'rxjs';

import { map, share } from 'rxjs/operators';
import { Client, LoginRequest, RegisterRequest } from '../../_tsModels/api-client';
import { JwtHelperService } from '@auth0/angular-jwt';
import { RegisterResponse } from 'src/_tsModels/classes';

@Injectable({
    providedIn: 'root'
})
export class AuthenticationService implements CanActivate {

    constructor(
        private router: Router,
        private api: Client,
        private jwtHelper: JwtHelperService,
    ) { }

    login(data: LoginRequest) {
        return this.api.apiAccountLogin(data)
            .subscribe(response => {
                if (response.hasError === false && response.data) {
                    localStorage.setItem('jwt', response.data);
                    // const decodedToken = this.jwtHelper.decodeToken(response.data);
                    // console.log('jwt data', decodedToken);
                    this.router.navigate(['']);
                }
            }, err => {
            });
    }

    logout() {
        localStorage.removeItem('jwt');
        this.router.navigate(['/login']);
    }

    async register(data: RegisterRequest) {
        // console.log('RegisterRequest', data);
        const response = await this.api.apiAccountRegister(data).toPromise();
        if (response.hasError === false && response.data) {
            this.router.navigate(['/login']);
        }
        return response;
    }

    get isLoggedIn(): boolean {
        return !this.jwtHelper.isTokenExpired();
    }

    get username(): string {
        return this.isLoggedIn && this.jwtHelper.decodeToken()['name'];
    }

    get userId(): string {
        return this.isLoggedIn && this.jwtHelper.decodeToken()['uid'];
    }

    canActivate(next: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean> | Promise<boolean> | boolean {
        // const token = localStorage.getItem('jwt');

        if (/*token && */!this.jwtHelper.isTokenExpired(/*token*/)) {
            return true;
        }
        this.router.navigate(['login']);
        return false;
    }
}
