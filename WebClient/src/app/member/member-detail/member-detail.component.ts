import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { member } from 'src/app/_CustomModels/member';
import { MembersService } from 'src/app/_services/members.service';

@Component({
  selector: 'app-member-detail',
  templateUrl: './member-detail.component.html',
  styleUrls: ['./member-detail.component.css']
})
export class MemberDetailComponent implements OnInit {
  
  memberInfo : member |undefined;

  constructor(private memberService:MembersService,private route:ActivatedRoute){

  }

  ngOnInit(): void {
    const userName = this.route.snapshot.paramMap.get("userName");
    if(userName){
      this.getMemberDetail(userName);
    }
  }

  getMemberDetail(username:string){

      this.memberService.getMember(username).subscribe({
        next : resp => {
          this.memberInfo = resp;
        }
      })
  }

}
