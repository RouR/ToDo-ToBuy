import { Injectable } from '@angular/core';
import { Router, CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { Observable } from 'rxjs';

import { map } from 'rxjs/operators';
import { Client, LoginRequest, RegisterRequest } from '../../_tsModels/api-client';
import { JwtHelperService } from '@auth0/angular-jwt';

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
    }

    register(data: RegisterRequest) {
        const result = this.api.apiAccountRegister(data);
        result
            .subscribe(response => {
                // console.log('apiAccountRegister response', response);
                if (response.hasError === false && response.data) {
                    this.router.navigate(['/login']);
                }
            }, err => {
                console.error('apiAccountRegister err', err);
            });
        return result;
    }

    get isLoggedIn(): boolean {
        return !this.jwtHelper.isTokenExpired();
    }

    get username(): string {
        return this.isLoggedIn && this.jwtHelper.decodeToken()['name'];
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
