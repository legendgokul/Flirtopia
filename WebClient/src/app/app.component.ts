import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { AccountService } from './_services/account.service'


@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {

  title = 'WebClient';
  users:any;

  //constructor will get executed as soon as build is completed (1st code part to get executed.)
  constructor(
    private http:HttpClient, 
    private accService: AccountService
    ){

  }

  //A lifecycle hook that is called after Angular has initialized all data-bound properties of a directive.
  ngOnInit(): void {
   
    this.getUsers();
    this.setCurrentUser();
  }

  getUsers(){
    this.http.get<any>('https://localhost:5001/api/User/GetUserList').subscribe({
      next: (resp) => {this.users = resp},
      error: (error) => {console.log(error)},
      complete : ()=> {console.log("Api request Completed")},
    }); 
  }
  //we check localstorage if any user info is saved if yes inject it to behavioursubject.
  setCurrentUser(){
    const LocalUser = localStorage.getItem('user');
    if(LocalUser){
      this.accService.setCurrentUser(JSON.parse(LocalUser));
    }
  }



  
}
