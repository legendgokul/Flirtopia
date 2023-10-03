import { Component } from '@angular/core';
import { AccountService } from '../_services/account.service';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent {

  model :any={};

  /**
   * @param accountservice  : injecting account service to make use for logiv, logout and current user functionality.
   * @param router : angular module which helps us with component routing/ redirection.
   */
  constructor(public accountservice:AccountService , private router:Router, private toastr:ToastrService){

  }


  ngOnInit(): void {

  }


  Login(){
    console.log(this.model);
    this.accountservice.login(this.model).subscribe({
      next: () =>{  // using () empty param since we are not gonna use response value.
        this.router.navigateByUrl("/members"), // navigating users to members component
        this.model={}  // resetting model to remove store username and password
      },
      error: err =>{
        this.toastr.error(err.error);
        console.log(err);
      }
    })
  }

  //logging out user.
  Logout(){
    this.accountservice.logout();
    this.router.navigateByUrl("/") // navigating users to home component
    this.model = {}; // resetting model to remove store username and password
  }
}
