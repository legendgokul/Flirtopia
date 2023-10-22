import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { MemberListComponent } from './member/member-list/member-list.component';
import { MemberDetailComponent } from './member/member-detail/member-detail.component';
import { ListsComponent } from './lists/lists.component';
import { MessagesComponent } from './messages/messages.component';
import { authGuard } from './_auth/auth.guard';
import { PageNotFoundComponent } from './_errComp/page-not-found/page-not-found.component';
import { MemberEditComponent } from './member/member-edit/member-edit.component';
import { preventUnsavedChangesGuard } from './_auth/prevent-unsaved-changes.guard';

const routes: Routes = [
  {path:'',component:HomeComponent },
  {path:'', 
  runGuardsAndResolvers:'always',
  canActivate:[authGuard],
  children:[
    {path:'members',component:MemberListComponent},
    {path:'members/:userName',component:MemberDetailComponent}, //going to pass user mame to get particular user.
    {path:'lists',component:ListsComponent},
    {path:'messages',component:MessagesComponent},
    {path:'member/edit',component:MemberEditComponent , canDeactivate:[preventUnsavedChangesGuard]}
    ] 
  },
  {path:"not-found",component:PageNotFoundComponent},
  {path:'**',component:HomeComponent ,pathMatch:'full'},
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
