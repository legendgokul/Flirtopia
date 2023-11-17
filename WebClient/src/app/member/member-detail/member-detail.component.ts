import { CommonModule } from '@angular/common';
import { Component, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { GalleryItem, GalleryModule, ImageItem } from 'ng-gallery';
import { TabDirective, TabsModule, TabsetComponent } from 'ngx-bootstrap/tabs';
import { member } from 'src/app/_CustomModels/member';
import { MembersService } from 'src/app/_services/members.service';
import { MemberMessagesComponent } from '../member-messages/member-messages.component';
import { MessageService } from 'src/app/_services/message.service';
import { Message } from 'src/app/_CustomModels/message';


@Component({
  selector: 'app-member-detail',
  standalone:true,
  templateUrl: './member-detail.component.html',
  styleUrls: ['./member-detail.component.css'],
  imports:[CommonModule,TabsModule,GalleryModule,MemberMessagesComponent]
})
export class MemberDetailComponent implements OnInit {
  
  //initialize member with empty object.
  memberInfo : member ={} as member;
  images:GalleryItem[] = [];
  @ViewChild('memberTabs',{static:true}) memberTabs?: TabsetComponent;
  activateTab?: TabDirective;
  messages:Message[] = [];

  constructor(private memberService:MembersService,
    private route:ActivatedRoute, private messageService : MessageService)
    {

    }

  ngOnInit(): void {
    //this.loadMember();
    this.route.data.subscribe({
      next:data => this.memberInfo = data['member']
    })

    this.route.queryParams.subscribe({
      next:params => {
        params['tab'] && this.selectTab(params['tab']);
      }
    });

    this.getImages();
  }

  selectTab(heading:string)
  {
    if(this.memberTabs){
      this.memberTabs.tabs.find(x=>x.heading === heading)!.active = true;
    }
  }

  onTabActivated(data : TabDirective)
  {
    this.activateTab =data;
    if(this.activateTab.heading === 'Messages')
    {
        this.loadMessages();
    }
  }

  loadMessages(){
    if(this.memberInfo){
      this.messageService.getMessageThread(this.memberInfo.userName).subscribe({
        next:message => this.messages = message
      })
    }
  }

  /*
  loadMember(){
    const username = this.route.snapshot.paramMap.get("userName");
    if(!username) return;
      this.memberService.getMember(username).subscribe({
        next : resp => {
          this.memberInfo = resp;
          //loading the images.
          for(const photo of resp.photos){
            this.images.push(new ImageItem({src:photo.url, thumb:photo.url}));
          }
        }
      });
  }
  */

  getImages(){
    if(!this.memberInfo) return;
    for(const photo of this.memberInfo.photos){
      this.images.push(new ImageItem({src:photo.url, thumb:photo.url}));
    }
  }

}
