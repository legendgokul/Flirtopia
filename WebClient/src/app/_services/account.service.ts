import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http'
import { BehaviorSubject, map } from 'rxjs';
import { User } from '../_CustomModels/user';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AccountService {

  baseUrl = environment.apiUrl ;
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
            this.setCurrentUser(user);
            this.currentUserName = user.userName;
          }
        })
      )
  }

  register(model:any){
    return this.http.post<User>(this.baseUrl +'account/register',model).pipe(
      map(user=>{
        if(user){    
          this.currentUserName = user.userName;
        }
      })
    )
  }

  setCurrentUser(user:User){
    localStorage.setItem('user',JSON.stringify(user));
    this.currentUserValue.next(user);
  }

  logout(){
    localStorage.removeItem('user');
    this.currentUserValue.next(null);
  }
  



}
