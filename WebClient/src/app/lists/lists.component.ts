import { Component, OnInit } from '@angular/core';
import { member } from '../_CustomModels/member';
import { MembersService } from '../_services/members.service';
import { Pagination } from '../_CustomModels/Pagination';

@Component({
  selector: 'app-lists',
  templateUrl: './lists.component.html',
  styleUrls: ['./lists.component.css']
})
export class ListsComponent implements OnInit {
  members:member[] |undefined;
  predicate = 'liked';
  pageNumber = 1;
  pageSize = 5;
  Pagination:Pagination | undefined;

  constructor(private memberService:MembersService){

  }

  ngOnInit(): void {
    this.loadLikes();
  }

  loadLikes(){
    this.memberService.getLikes(this.predicate, this.pageNumber, this.pageSize).subscribe({
      next:(resp)=>{
        (resp.result && resp.result.length >0)? this.members =resp.result : this.members =undefined;
        this.Pagination = resp.pagination;
      }
    })
  }

  pageChange(event:any){
    if(this.pageNumber !== event.page){
      this.pageNumber = event.page;
      this.loadLikes();
    }
  }

}
