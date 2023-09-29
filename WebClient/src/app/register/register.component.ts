import { Component, EventEmitter, Input, Output } from '@angular/core';
import { AccountService } from '../_services/account.service';

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

  constructor(private accountService :AccountService) {

  }

  ngOnInit() :void {

  }

  register() {
    console.log(this.model);
    this.accountService.register(this.model).subscribe({
      next: (resp) => {
                        console.log("register success ");
                        this.RegisterToggle.emit(false);
                      },
      error: (err) => console.error(err)
    });    
  }

  cancel(){
    this.RegisterToggle.emit(false);
    console.log("cancel");
  }


}
