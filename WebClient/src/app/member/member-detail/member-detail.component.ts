import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { GalleryItem, GalleryModule, ImageItem } from 'ng-gallery';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { member } from 'src/app/_CustomModels/member';
import { MembersService } from 'src/app/_services/members.service';

@Component({
  selector: 'app-member-detail',
  standalone:true,
  templateUrl: './member-detail.component.html',
  styleUrls: ['./member-detail.component.css'],
  imports:[CommonModule,TabsModule,GalleryModule]
})
export class MemberDetailComponent implements OnInit {
  
  memberInfo : member |undefined;
  images:GalleryItem[] = [];
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
          //loading the images.
          for(const photo of resp.photos){
            this.images.push(new ImageItem({src:photo.url, thumb:photo.url}));
          }

        }
      })
  }

}
