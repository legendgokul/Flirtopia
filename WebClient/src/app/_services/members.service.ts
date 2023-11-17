import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { member } from '../_CustomModels/member';
import { map, of, take } from 'rxjs';
import { userParams } from '../_CustomModels/userParams';
import { AccountService } from './account.service';
import { User } from '../_CustomModels/user';
import { getPaginationHeaders, getPaginationResult } from './paginationHelper';

@Injectable({
  providedIn: 'root',
})
export class MembersService {
  baseUrl: string = environment.apiUrl;
  memberList: member[] = [];
  memberCache = new Map(); //changed it to map so that we can store different query result for each member list.
  //moving the user and params info to member service so that Query is saved and not destroyed on navigation.
  user: User | undefined; // indicates the logged in user.
  userParam: userParams | undefined;

  constructor(private http: HttpClient, private _account: AccountService) {
    this._account.currentUser$.pipe(take(1)).subscribe({
      next: (user) => {
        if (user) {
          this.user = user;
          this.userParam = new userParams(user);
        }
      },
    });
  }

  getUserParams() {
    return this.userParam;
  }

  setUserParams(params: userParams) {
    return (this.userParam = params);
  }

  resetUserParams() {
    //check if the user exists
    if (!this.user) return;
    this.userParam = new userParams(this.user);
    return this.userParam;
  }

  getMembers(userParams: userParams) {
    //check if the map has the matching keys
    var response = this.memberCache.get(Object.values(userParams).join('-'));

    //return the value from cache /in memory to the User.
    if (response) return of(response);

    //create a model to pass as HeaderParams
    var Params = getPaginationHeaders(
      userParams.pageNumber,
      userParams.pageSize
    );

    // adding gender and min max age :
    Params = Params.append('minAge', userParams.minAge);
    Params = Params.append('maxAge', userParams.maxAge);
    Params = Params.append('gender', userParams.gender);
    Params = Params.append('orderBy', userParams.orderBy);
    //if(this.memberList.length >0) return of(this.memberList);

    const URl = this.baseUrl + 'user/GetUserList';
    return getPaginationResult<member[]>(URl, Params,this.http).pipe(
      map((response) => {
        this.memberCache.set(Object.values(userParams).join('-'), response);
        return response;
      })
    );
  }


  getMember(username: string) {
    //const member = this.memberList.find(x=>x.userName === username);
    [...this.memberCache.values()].forEach((elem) => {
      elem.result.forEach((newMemb: member) => {
        if (
          !this.memberList.find(
            (userObj) => userObj.userName === newMemb.userName
          )
        ) {
          this.memberList.push(newMemb);
        }
      });
    });

    const foundMember = this.memberList.find(
      (memb) => memb.userName == username
    );

    if (foundMember) return of(foundMember);

    return this.http.get<member>(
      this.baseUrl + 'user/GetUserByName/' + username
    );
  }

  updateMember(member: member) {
    return this.http.put(this.baseUrl + 'user/UpdateUser', member).pipe(
      map(() => {
        const index = this.memberList.indexOf(member); //find the index for updated user in existing list.
        this.memberList[index] = { ...this.memberList[index], ...member };
      })
    );
  }
  // Photos
  setMainPhoto(photoId: number) {
    return this.http.put(this.baseUrl + 'user/set-main-photo/' + photoId, {});
  }

  DeletePhoto(photoId: number) {
    return this.http.delete(this.baseUrl + 'user/delete-photo/' + photoId);
  }
  
  // Likes
  addLike(username:string){
    return this.http.post(this.baseUrl+'likes/'+username,{});
  }
  getLikes(predicate:string , pageNumber:number, pageSize:number){
    let params = getPaginationHeaders(pageNumber,pageSize);
    params = params.append('predicate',predicate);

    return getPaginationResult<member[]>(this.baseUrl+'likes',params, this.http);
  }
}
