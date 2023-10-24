import { Component, Input, Self } from '@angular/core';
import { ControlValueAccessor, FormControl, NgControl } from '@angular/forms';

@Component({
  selector: 'app-text-input',
  templateUrl: './text-input.component.html',
  styleUrls: ['./text-input.component.css']
})
export class TextInputComponent implements ControlValueAccessor {

  @Input() label ='';
  @Input() propertyName = '';
  @Input() type = '';

  /*
    ngControl is parent class for FormControl , its a base call which all form control must extend to,
    inside constructor we are using @Self why?
      usually when a component is initialized , it will check if the same component is initialized , if yes it will reuse it .
      adding @Self prevent this from happening , why ? because each input field will hold unique data. 
  */
  constructor(@Self() public ngControl : NgControl){
    this.ngControl.valueAccessor = this;
  }

  writeValue(obj: any): void {

  }
  registerOnChange(fn: any): void {

  }
  registerOnTouched(fn: any): void {

  }

  get control() :FormControl{
    return this.ngControl.control as FormControl;
  }
}
