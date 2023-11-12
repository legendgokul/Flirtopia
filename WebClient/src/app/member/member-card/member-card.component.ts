import { Component, Input, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { member } from 'src/app/_CustomModels/member';
import { MembersService } from 'src/app/_services/members.service';

@Component({
  selector: 'app-member-card',
  templateUrl: './member-card.component.html',
  styleUrls: ['./member-card.component.css']
})
export class MemberCardComponent implements OnInit {
  
  @Input() memberItem: member |undefined;
  constructor(private memberService:MembersService,
    private _toastr:ToastrService){
  }
  ngOnInit(): void {
  }

  addLike(member:member){
    this.memberService.addLike(member.userName).subscribe({
      next:() =>{this._toastr.success("you have liked "+member.userName);},
      error:(err) =>{console.log(err);}
    })
  }

}
