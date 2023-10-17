import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { member } from '../_CustomModels/member';
import { User } from '../_CustomModels/user';

@Injectable({
  providedIn: 'root'
})
export class MembersService {

  baseUrl :string = environment.apiUrl;

  constructor(private http:HttpClient) 
  { 
    
  }

  getMembers(){
    return this.http.get<member[]>(this.baseUrl+"user/GetUserList");
  }

  getMember(username:string){
    return this.http.get<member>(this.baseUrl+"user/GetUserByName/"+username);
  }







}
