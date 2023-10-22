import { Component } from '@angular/core';
import { Observable } from 'rxjs';
import { member } from 'src/app/_CustomModels/member';
import { MembersService } from 'src/app/_services/members.service';

@Component({
  selector: 'app-member-list',
  templateUrl: './member-list.component.html',
  styleUrls: ['./member-list.component.css']
})
export class MemberListComponent {

  //conventionally we add $ at end of variable to denote its observable.
  members$ : Observable< member[]> | undefined ;

  constructor(private _memberservice:MembersService){

  }

  ngOnInit():void
  {
    this.members$ = this._memberservice.getMembers();
  }


  

}
