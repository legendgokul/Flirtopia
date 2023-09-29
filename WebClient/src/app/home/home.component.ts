import { Component } from '@angular/core';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent {

 registerMode = false;
 username:string = "String from parent";
  constructor(){

 }

 ngOnInit():void {

 }

 registerToggle(event:boolean){
  this.registerMode = event;
 }

}
