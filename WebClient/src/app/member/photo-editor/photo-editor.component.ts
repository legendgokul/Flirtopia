import { Component, Input, OnInit } from '@angular/core';
import { FileUploader } from 'ng2-file-upload';
import { take } from 'rxjs';
import { member } from 'src/app/_CustomModels/member';
import { photo } from 'src/app/_CustomModels/photo';
import { User } from 'src/app/_CustomModels/user';
import { AccountService } from 'src/app/_services/account.service';
import { MembersService } from 'src/app/_services/members.service';
import {environment} from "src/environments/environment"

@Component({
  selector: 'app-photo-editor',
  templateUrl: './photo-editor.component.html',
  styleUrls: ['./photo-editor.component.css']
})
export class PhotoEditorComponent implements OnInit {
  
  @Input() CurrentUser :member |undefined; 
  uploader :FileUploader |undefined;
  hasBaseDropzoneOver = false;
  baseUrl = environment.apiUrl ;
  user:User|undefined;


  constructor(private _accService :AccountService,private _membService:MembersService){
    this._accService.currentUser$.pipe(take(1)).subscribe({
      next: reponse =>{
        if(reponse) this.user = reponse;
      }
    })
  }

  ngOnInit(): void {
    this.initializeUploader()
  }
  fileOverBase(e:any){
    this.hasBaseDropzoneOver = e;
  }

  //setting main pic
  setMainPhoto(photo:photo){
    this._membService.setMainPhoto(photo.id).subscribe({
      next: () => {
        // update the member info
        if(this.user && this.CurrentUser){
          //updating the user info present in service and local storage
          this.user.photoUrl = photo.url;
          this._accService.setCurrentUser(this.user);
        
          //updating the member info present inside this component
          this.CurrentUser.photoUrl = photo.url;
          this.CurrentUser.photos.forEach(x=>{
            if(x.isMain == true) x.isMain = false;
            if(x.id === photo.id) x.isMain = true;
          });
        }
      }
    });
  }

  //Delete pic
  DeletePhoto(photo:photo){
    this._membService.DeletePhoto(photo.id).subscribe({
      next : ()=>{
        //remove the pic from current list
        if(this.CurrentUser){
          this.CurrentUser.photos = this.CurrentUser?.photos.filter(x =>x.id !== photo.id)
        }
      }
    });
  }

  initializeUploader(){
    this.uploader = new FileUploader({
      url:this.baseUrl + 'user/add-photo',
      authToken:'Bearer '+this.user?.token,
      isHTML5:true,
      allowedFileType:['image'],
      autoUpload:false,
      maxFileSize:10 *1024 *1024
    });

    //to skip the cors issue.
    this.uploader.onAfterAddingFile =(file)=>{
      file.withCredentials = false;
    }

    this.uploader.onSuccessItem = (item,response,status,headers) =>{
      if(response){
        const photo = JSON.parse(response);
        this.CurrentUser?.photos.push(photo);
      }
    }

  }



}
