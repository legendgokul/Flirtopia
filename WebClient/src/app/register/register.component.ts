import { Component, EventEmitter, Input, Output } from '@angular/core';
import { AccountService } from '../_services/account.service';
import { ToastrService } from 'ngx-toastr';
import { AbstractControl, FormBuilder, FormControl, FormGroup, ValidatorFn, Validators } from '@angular/forms';
import { Router } from '@angular/router';


@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent {
  //input tag to get datafrom parent.
  @Input() userFromHomeComponent:any;
  @Output() RegisterToggle = new EventEmitter();

  registerForm : FormGroup = new FormGroup({}); // cannot declare it as undefined hence initialize it with empty object.
  minValidDate : Date = new Date(); //create minValidDate with current date 
  validationErrors:string[] |undefined;

  constructor(private accountService :AccountService, private toastr:ToastrService,private fb:FormBuilder, private router :Router ) {

  }

  ngOnInit() :void {
    this.initializeForm();
    this.minValidDate.setFullYear(this.minValidDate.getFullYear() - 18);
  }

  // initializing the password.
  initializeForm(){
    this.registerForm = this.fb.group({
      // initial value , [ validation ]
      gender:['Male'],
      username:['',Validators.required],
      knownAs:['',Validators.required],
      dateOfBirth:['',Validators.required],
      city:['',Validators.required],
      country:['',Validators.required],
      password:['', [ Validators.required,
                        Validators.minLength(4), 
                        Validators.maxLength(16)]],
      confirmPassword:['',[Validators.required,this.matchValues('password')]]
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
    const dob = this.getDateOnly(this.registerForm.controls['dateOfBirth'].value);
    const values = {...this.registerForm.value, dateOfBirth :dob}
    console.log(values);
    
    this.accountService.register(values).subscribe({
      next: (resp) => {
                        this.router.navigateByUrl('/members')
                      },
      error: (err) => {
        this.validationErrors = err;
      }
    });   

  }

  private getDateOnly(dob:string|undefined){
    if(!dob) return;
    let theDob = new Date(dob);
    //return theDob.toISOString().split('T')[0];
    return new Date(theDob.setMinutes(theDob.getMinutes() - theDob.getTimezoneOffset()))
    .toISOString().slice(0,10);
  }

  cancel(){
    this.RegisterToggle.emit(false);
    console.log("cancel");
  }


}
