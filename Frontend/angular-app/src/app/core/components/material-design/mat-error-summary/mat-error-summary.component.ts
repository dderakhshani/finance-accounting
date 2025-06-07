import {Component, Input, OnInit} from '@angular/core';
import {AbstractControl, ControlContainer, FormGroup} from "@angular/forms";

@Component({
  selector: 'mat-error-summary',
  templateUrl: './mat-error-summary.component.html',
  styleUrls: ['./mat-error-summary.component.scss']
})
export class MatErrorSummaryComponent {

  @Input() control!:string;
  @Input() caption:string = 'فیلد';
  @Input() visible:any;

  constructor(private controlContainer: ControlContainer) {}

  get form():FormGroup {
    return this.controlContainer.control  as FormGroup;
  }

  get formControl() :AbstractControl{
    return this.form.get(this.control) as AbstractControl;
  }

  get errors(): string[] {
    return Object.keys(this.formControl.errors as Object)
  }

  get isNotValid() {
    return this.formControl.invalid && (this.formControl.touched || this.formControl.dirty)
  }

}
