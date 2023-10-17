import { Component, Input, OnInit } from '@angular/core';
import { member } from 'src/app/_CustomModels/member';

@Component({
  selector: 'app-member-card',
  templateUrl: './member-card.component.html',
  styleUrls: ['./member-card.component.css']
})
export class MemberCardComponent implements OnInit {
  
  @Input() memberItem: member |undefined;
  constructor(){
  }
  ngOnInit(): void {
  }

}
