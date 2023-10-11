/*
  When you send an HTTP request from your Angular application using Angular's HttpClient, 
  the request goes through a series of interceptors before it reaches the server. 
  Each interceptor can modify the request, add headers, or perform other actions. 
  The intercept method is called for each interceptor in the order they are provided
*/

import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor,
  HttpErrorResponse
} from '@angular/common/http';
import { Observable, catchError } from 'rxjs';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {

  constructor(private toastr:ToastrService, private router:Router) {}

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    return next.handle(request).pipe(
      catchError((error:HttpErrorResponse) =>{
        if(error){    // checking if there is any error obtained in current http request.
          console.log(error);
          switch(error.status){
            case 400:  // bad request.
              { 
                if (error.error && error.error.errors) {
                  throw  Object.values(error.error.errors).flat(); 
                } else {
                  // Handle the error in a different way or provide a fallback
                  this.toastr.error(error.error, error.status.toString());
                } 
                break;
              }
            case 401:  // un authorized.
              { 
                this.toastr.error("Unauthorized: "+error.message, error.status.toString());
                break;
              }
            case 500:  //internal server error.
              { 
                // design a diff page or server err similar to amazon and redirect.
                console.log("interceptor",error.message);
                break;
              }
            case 404:  //wrong url / missing endpoint .
              { 
                //we can redirect the user to page not found component yet to build
                this.router.navigateByUrl("not-found");
                break;
              }
            default :
            { 
              this.toastr.error("Something Unexpected has happened !. please contack support Team.","OH NO!😨");
              break;
            }
          }
        }
        throw error;  //similar to dotnet catch post logging we will throw it ;
      })
    );


  }
}
