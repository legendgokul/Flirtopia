import { Component } from '@angular/core';
import { Observable } from 'rxjs';
import { Pagination } from 'src/app/_CustomModels/Pagination';
import { member } from 'src/app/_CustomModels/member';
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
  pageNumber = 1;
  pageSize = 5;

  constructor(private _memberservice:MembersService){

  }

  ngOnInit():void
  {
    //this.members$ = this._memberservice.getMembers();
    this.loadMembers()
  }

  loadMembers(){
    this._memberservice.getMembers(this.pageNumber,this.pageSize).subscribe({
      next: resp =>{
        if(resp.pagination && resp.result){
          this.pagination = resp.pagination;
          this.members = resp.result;
        }
      }
    });
  }

  pageChange(event:any){
    if(this.pageNumber !== event.page){
      this.pageNumber = event.page;
      this.loadMembers();
    }
  }
  

}
