import { Component, EventEmitter, Input, Output } from '@angular/core';
import { AccountService } from '../_services/account.service';
import { ToastrService } from 'ngx-toastr';
import { AbstractControl, FormControl, FormGroup, ValidatorFn, Validators } from '@angular/forms';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent {
  //input tag to get datafrom parent.
  @Input() userFromHomeComponent:any;

  @Output() RegisterToggle = new EventEmitter();
  model :any ={};
  registerForm : FormGroup = new FormGroup({}); // cannot declare it as undefined hence initialize it with empty object.

  constructor(private accountService :AccountService, private toastr:ToastrService) {

  }

  ngOnInit() :void {
    this.initializeForm();
  }

  // initializing the password.
  initializeForm(){
    this.registerForm = new FormGroup({
      // initial value , [ validation ]
      username:new FormControl('',Validators.required),
      password:new FormControl('', [ Validators.required,
                        Validators.minLength(4), 
                        Validators.maxLength(8)]),
      confirmPassword:new FormControl('',[Validators.required,this.matchValues('password')])
    });

    //we need to preform validation even if user changes initial password.
    this.registerForm.controls['password'].valueChanges.subscribe({
      next:()=>{
      this.registerForm.controls['confirmPassword'].updateValueAndValidity();
      }
    });

  }

  // comparing password and confirm password
  matchValues(matchTO :string ):ValidatorFn {
    return (control:AbstractControl) =>{
      return control.value === control.parent?.get(matchTO)?.value ? null : {notMatching:true}
    }
  }

  register() {

    console.log(this.registerForm?.value);

    /*
    console.log(this.model);
    this.accountService.register(this.model).subscribe({
      next: (resp) => {
                        console.log("register success ");
                        this.RegisterToggle.emit(false);
                      },
      error: (err) => {
      this.toastr.error(err,"Register new user Failed.")}
    });   */ 


  }

  cancel(){
    this.RegisterToggle.emit(false);
    console.log("cancel");
  }


}
