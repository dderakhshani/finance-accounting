import {Component, Input, OnInit} from '@angular/core';
import {FormArray, FormControl, FormGroup} from "@angular/forms";

@Component({
  selector: 'app-create-condition',
  templateUrl: './create-condition.component.html',
  styleUrls: ['./create-condition.component.scss']
})
export class CreateConditionComponent implements OnInit {
 formCondition: FormGroup = new FormGroup({});
 formControl: FormControl = new FormControl();
 arrayParams:Array<any> = [];

  @Input() set JsonConditionSetter(value: FormControl) {
    this.formControl = value;
    this.generateForm()
  }

  constructor() {
    this.formCondition=new FormGroup({
      expression:new FormControl(),
      paramCondition:new FormControl()
    })
  }

  ngOnInit(): void {

  }

  generateJson() {
    debugger
      let generatedCondition: any;
      let arraycondition: Array<any> = [];
      generatedCondition={
          Expression: this.formCondition.controls['expression'].value,
          Properties:this.arrayParams?.map((y: any) => {
            return {Name: y}
          })
        }
        arraycondition.push(generatedCondition);
      this.formControl.setValue(JSON.stringify(arraycondition));
      console.log(this.formControl.value);
  }
  addParameter(value:any) {
    this.arrayParams.push(value);
    this.formCondition.controls['paramCondition'].setValue(undefined);
  }

  generateForm() {
     debugger
    let formData = JSON.parse(this.formControl.value);
    formData?.forEach((x: any) => {
    this.formCondition.controls['expression'].setValue(x.Expression);
    this.arrayParams=x.Properties?.map((y: any) => y.Name)
    })
  }

  removeParam(param: any){
    // @ts-ignore
    this.arrayParams.pop(param);
  }

}
