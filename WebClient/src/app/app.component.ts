import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';


@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {

  title = 'WebClient';
  users:any;

  //constructor will get executed as soon as build is completed (1st code part to get executed.)
  constructor(private http:HttpClient){

  }

  //A lifecycle hook that is called after Angular has initialized all data-bound properties of a directive.
  ngOnInit(): void {
   
    this.http.get<any>('https://localhost:5001/api/User/GetUserList').subscribe({
      next: (resp) => {this.users = resp},
      error: (error) => {console.log(error)},
      complete : ()=> {console.log("Api request Completed")},
    }); 
  }



  
}
