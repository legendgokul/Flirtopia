import { Component, HostListener, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { take } from 'rxjs';
import { member } from 'src/app/_CustomModels/member';
import { User } from 'src/app/_CustomModels/user';
import { AccountService } from 'src/app/_services/account.service';
import { MembersService } from 'src/app/_services/members.service';

@Component({
  selector: 'app-member-edit',
  templateUrl: './member-edit.component.html',
  styleUrls: ['./member-edit.component.css']
})
export class MemberEditComponent implements OnInit {
  
  @ViewChild('editForm') _editForm:NgForm |undefined;

  // code to listen for browser window change and see if the window is getting closed or navigated then throw a notification to confirm the redirection.
  @HostListener('window:beforeunload',['$event']) unloadNotification($event: any){
    if(this._editForm?.dirty){
      $event.returnValue = true;
    }
  }
  member:member|undefined;  
  CurrentLoggedInUser :User |null = null;

  constructor(private _accService :AccountService, private _memberService :MembersService, private _toastr:ToastrService) 
  {
    this._accService.currentUser$.pipe(take(1)).subscribe({
      next:resp =>{ if(resp)this.CurrentLoggedInUser = resp}
    })
  }

  ngOnInit(): void {
   this.LoadMemberInfo();
  }
 
  LoadMemberInfo(){
    if(!this.CurrentLoggedInUser) return;
    
    this._memberService.getMember(this.CurrentLoggedInUser.userName).pipe(take(1)).subscribe({
      next: Response => this.member = Response
    });
    
  }

  UpdateMember(){
    console.log(this.member);
  
    this._memberService.updateMember(this._editForm?.value).subscribe({
      next:() =>{
        this._editForm?.reset(this.member);
        this._toastr.info("Profile Updated.");
      }
    });
  
  }



}
