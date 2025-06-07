import { Component, Inject, OnInit, Optional } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialog, MatDialogRef } from '@angular/material/dialog';
import { BaseComponent } from 'src/app/core/abstraction/base.component';
import { Mediator } from 'src/app/core/services/mediator/mediator.service';
import { Documents } from 'src/app/modules/bursary/entities/enums';

@Component({
  selector: 'app-select-date',
  templateUrl: './select-date.component.html',
  styleUrls: ['./select-date.component.scss']
})
export class SelectDateComponent extends BaseComponent {

  public documents = Documents;
   
  creditAccountReferenceId!:number
  documentDate !: any
  constructor(private _mediator: Mediator,@Optional() public dialogRef: MatDialogRef<SelectDateComponent>, @Inject(MAT_DIALOG_DATA) public value: any, public dialog: MatDialog) {
    super();
   }

  resolve(params?: any) {
    throw new Error('Method not implemented.');
  }
  initialize(params?: any) {
    throw new Error('Method not implemented.');
  }
  add(param?: any) {
    throw new Error('Method not implemented.');
  }
  get(param?: any) {
    throw new Error('Method not implemented.');
  }
  update(param?: any) {
    throw new Error('Method not implemented.');
  }
  delete(param?: any) {
    throw new Error('Method not implemented.');
  }
  close() {
    throw new Error('Method not implemented.');
  }

 async ngOnInit()  {

   

  }

 



  setReference(){
 let result = this.documentDate._d;
    this.dialogRef.close(result);
  }

}