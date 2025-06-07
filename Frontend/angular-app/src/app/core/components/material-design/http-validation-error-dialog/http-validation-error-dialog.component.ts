import {Component, Inject, OnInit} from '@angular/core';
import {MAT_DIALOG_DATA} from "@angular/material/dialog";
import {HttpValidationError} from "../../../models/http-validation-error";
import {group} from "@angular/animations";

@Component({
  selector: 'app-http-validation-error-dialog',
  templateUrl: './http-validation-error-dialog.component.html',
  styleUrls: ['./http-validation-error-dialog.component.scss']
})
export class HttpValidationErrorDialogComponent implements OnInit {

  errors:HttpValidationError[] = []

  groupedErrorsList: any[] = [];
  constructor(
    @Inject(MAT_DIALOG_DATA) data:any
  ) {

    this.errors = data;

    let groupedErrors = groupBy(this.errors,'propertyName');

    Object.keys(groupedErrors).forEach(x => {
      this.groupedErrorsList.push(groupedErrors[x])
    })
  }

  ngOnInit(): void {
  }


  protected readonly group = group;
}
var groupBy = function(xs:any, key:any) {
  return xs.reduce(function(rv:any, x:any) {
    (rv[x[key]] = rv[x[key]] || []).push(x);
    return rv;
  }, {});
};
