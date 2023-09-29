import { Component } from '@angular/core';
import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent {

  model :any={};


  constructor(public accountservice:AccountService){

  }



  ngOnInit(): void {

  }



  Login(){
    console.log(this.model);
    this.accountservice.login(this.model).subscribe({
      next: response =>{
        console.log(response);
    
      },
      error: err =>{
        console.log(err);
      }
    })
  }

  //logging out user.
  Logout(){
    this.accountservice.logout();

    this.model = {};
  }
  
}
