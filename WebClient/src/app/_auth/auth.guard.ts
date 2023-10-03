import { inject } from '@angular/core';
import { CanActivateFn } from '@angular/router';
import { AccountService } from '../_services/account.service';
import { ToastrService } from 'ngx-toastr';
import { map } from 'rxjs';


// creating a route guard to prevent unwanted access
export const authGuard: CanActivateFn = (route, state) => {

  //injecting other services to access their object
  const accService = inject(AccountService);
  const toastr = inject(ToastrService);

   return accService.currentUser$.pipe(
        map(user =>{
          if(user){
            return true;
          }else{
            toastr.error("Pls login to proceed ahead!");          
            return false;
          }
        })
   );
};
