import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http'
import { BehaviorSubject, map } from 'rxjs';
import { User } from '../_CustomModels/user';

@Injectable({
  providedIn: 'root'
})
export class AccountService {

  baseUrl = "https://localhost:5000/api/" ;
  //creating a new Observable.
  // it can store and emit values to subscribers.
  private currentUserValue = new BehaviorSubject<User|null>(null); 

  //converting a normal subscribe into just an observable. (to remove access from updatng it).
  currentUser$ = this.currentUserValue.asObservable();
  currentUserName :string = "";

  constructor(private http : HttpClient) 
  { 


  }
  
  login(model :any){
      return this.http.post<User>(this.baseUrl + "Account/Login",model).pipe(
        map((resp:User)=>{
          const user= resp;
          if(user){
            localStorage.setItem('user',JSON.stringify(user));
            this.currentUserValue.next(user);
            this.currentUserName = user.userName;
          }
        })
      )
  }

  register(model:any){
    return this.http.post<User>(this.baseUrl +'account/register',model).pipe(
      map(user=>{
        if(user){
          this.currentUserValue.next(user);
          localStorage.setItem('user',JSON.stringify(user));
          this.currentUserName = user.userName;
        }
      })
    )
  }

  setCurrentUser(user:User){
    this.currentUserValue.next(user);
  }

  logout(){
    localStorage.removeItem('user');
    this.currentUserValue.next(null);
  }
  



}
