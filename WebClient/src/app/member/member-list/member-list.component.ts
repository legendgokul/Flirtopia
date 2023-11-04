import { Component } from '@angular/core';
import { Observable, take } from 'rxjs';
import { Pagination } from 'src/app/_CustomModels/Pagination';
import { member } from 'src/app/_CustomModels/member';
import { User } from 'src/app/_CustomModels/user';
import { userParams } from 'src/app/_CustomModels/userParams';
import { AccountService } from 'src/app/_services/account.service';
import { MembersService } from 'src/app/_services/members.service';

@Component({
  selector: 'app-member-list',
  templateUrl: './member-list.component.html',
  styleUrls: ['./member-list.component.css']
})
export class MemberListComponent {

  //conventionally we add $ at end of variable to denote its observable.
  //members$ : Observable< member[]> | undefined ;
  members:member[] = [];
  pagination:Pagination |undefined;
  userParam :userParams |undefined;
  genderList = [{value:"male", Display:"Male"},{value:"female", Display:"Female"}];
  sortBy = [{value:"createdon", Display:"Profile Created"},{value:"lastActive", Display:"Last Active"}];
  constructor(private _memberservice:MembersService){
     this.userParam = this._memberservice.getUserParams();

  }

  ngOnInit():void
  {
    //this.members$ = this._memberservice.getMembers();
    this.loadMembers()
  }

  loadMembers(){
    if(!this.userParam) return ;
    //setting user param post update.
    this._memberservice.setUserParams(this.userParam); 
    this._memberservice.getMembers(this.userParam).subscribe({
      next: resp =>{
        if(resp.pagination && resp.result){
          this.pagination = resp.pagination;
          this.members = resp.result;  
        }
      }
    });
  }

  //Reset filters in the page and its data
  resetFilters(){
      this.userParam = this._memberservice.resetUserParams();
      this.loadMembers();
  }


  pageChange(event:any){
    if(this.userParam && this.userParam.pageNumber !== event.page){
      this.userParam.pageNumber = event.page;
      this.loadMembers();
    }
  }
  

}
