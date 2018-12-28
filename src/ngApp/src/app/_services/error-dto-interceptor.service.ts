import { Injectable, Injector, ErrorHandler, } from '@angular/core';
import { HttpInterceptor, HttpHandler, HttpRequest, HttpEvent, HttpErrorResponse, HttpResponse } from '@angular/common/http';
import { Observable, empty, throwError } from 'rxjs';
import { tap, catchError, finalize } from 'rxjs/operators';
import { MatSnackBar } from '@angular/material/snack-bar';

export interface IErrorable {
  hasError: boolean;
  message?: string | undefined;
}


@Injectable({
  providedIn: 'root'
})
export class ErrorDtoInterceptorService implements HttpInterceptor {

  constructor(public snackBar: MatSnackBar) { }

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {

    const started = Date.now();

    return next.handle(req)
      .pipe(
        tap(
          val => {
            // console.log(`check http response`, val);
            if (val instanceof HttpResponse) {
              // console.log(`HttpResponse ${val}`, val);

              if ((<Blob>val.body).type === 'application/json') {
                const reader = new FileReader();
                reader.addEventListener('loadend', (e) => {
                  const obj = <IErrorable>JSON.parse(e.srcElement['result']);
                  // console.log(obj);
                  if (obj.hasError) {
                    // console.log(`IErrorable`, obj);
                    this.snackBar.open(obj.message, 'Server Error', {
                      duration: 2500,
                      horizontalPosition: 'right',
                      verticalPosition: 'bottom',

                    });
                  }
                });
                reader.readAsText(val.body);
              }
            }
          },
          error => {
            console.error(`reguest failed`, error);
            this.snackBar.open(error.statusText, 'Server Error', {
              duration: 2500,
              horizontalPosition: 'right',
              verticalPosition: 'bottom',
            });
          }
        ),
        // Log when response observable either completes or errors
        finalize(() => {
          const elapsed = Date.now() - started;
          const msg = `${req.method} "${req.urlWithParams}" in ${elapsed} ms.`;
          // console.log(msg);
        })
      );
  }
}

