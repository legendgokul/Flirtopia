import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { Observable, take } from 'rxjs';
import { AccountService } from '../_services/account.service';
import { User } from '../_CustomModels/user';

@Injectable()
export class JwtInterceptor implements HttpInterceptor {

  constructor(private accService :AccountService) {}
  
  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    this.accService.currentUser$.pipe(take(1)).subscribe({
      next : resp => {
        if(resp)
        {
          //adding token to the header
          request = request.clone({
          setHeaders :
              {
                Authorization : `Bearer ${resp.token}`
              }
          });
        }
      }
    })
    
 


    return next.handle(request);
  }
}
