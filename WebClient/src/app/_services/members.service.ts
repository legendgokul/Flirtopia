import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { member } from '../_CustomModels/member';
import { map, of } from 'rxjs';
import { PaginationResult } from '../_CustomModels/Pagination';

@Injectable({
  providedIn: 'root'
})
export class MembersService {

  baseUrl :string = environment.apiUrl;
  memberList : member[] = [];  //contains list of available members.
  PaginationResult:PaginationResult<member[]> = new PaginationResult<member[]>;
  

  constructor(private http:HttpClient) 
  { 
    
  }

  getMembers(page?: number , itemsPerPage?:number){
    //create a model to pass as HeaderParams
    var Params = new HttpParams();
    if(page && itemsPerPage){
      Params = Params.append('pageNumber',page);
      Params = Params.append('pageSize',itemsPerPage);
    }

    //if(this.memberList.length >0) return of(this.memberList);
    return this.http.get<member[]>(this.baseUrl+"user/GetUserList",{ observe:'response', params :Params })
    .pipe( map( response =>{
       if(response.body){
          this.PaginationResult.result = response.body;
       }
       const pagination = response.headers.get('pagination');

       if(pagination){
        this.PaginationResult.pagination = JSON.parse(pagination);
       }
       return this.PaginationResult;
      })
      // map( members =>{
      //   this.memberList = members;
      //   return members;
      // })
    );
  }

  getMember(username:string){
    const member = this.memberList.find(x=>x.userName === username);
    if(member) return of(member);
    return this.http.get<member>(this.baseUrl+"user/GetUserByName/"+username);
  }

  updateMember(member:member){
    return this.http.put(this.baseUrl +"user/UpdateUser", member).pipe(
      map( ()=> {
        const index = this.memberList.indexOf(member); //find the index for updated user in existing list.
        this.memberList[index] = {...this.memberList[index], ...member};
      })
    );
  }

  setMainPhoto(photoId:number){
      return this.http.put(this.baseUrl + "user/set-main-photo/"+photoId,{});
    
  }

  DeletePhoto(photoId:number){
    return this.http.delete(this.baseUrl+"user/delete-photo/"+photoId);
  }






}
