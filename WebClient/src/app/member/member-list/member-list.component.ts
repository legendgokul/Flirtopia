import { Component } from '@angular/core';
import { member } from 'src/app/_CustomModels/member';
import { MembersService } from 'src/app/_services/members.service';

@Component({
  selector: 'app-member-list',
  templateUrl: './member-list.component.html',
  styleUrls: ['./member-list.component.css']
})
export class MemberListComponent {

  members : member[] = [];

  constructor(private _memberservice:MembersService){

  }

  ngOnInit():void
  {
    this.loadMembers();
  }


  loadMembers(){
    this._memberservice.getMembers().subscribe(
      {
        next: resp => this.members = resp
      });
  }

}
