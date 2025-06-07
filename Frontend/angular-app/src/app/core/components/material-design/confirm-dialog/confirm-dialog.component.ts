import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';

@Component({
  selector: 'app-confirm-dialog',
  templateUrl: './confirm-dialog.component.html',
  styleUrls: ['./confirm-dialog.component.scss']
})
export class ConfirmDialogComponent implements OnInit {

  ConfirmDialogIcons = ConfirmDialogIcons;

   element: any | undefined = undefined;
  constructor(
    public dialogRef: MatDialogRef<ConfirmDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public config: ConfirmDialogConfig) {
  }

  ngOnInit(): void {
    this.element = document.getElementById('Modal_Message');

    if (this.element) {
      this.element.innerHTML = this.config.message;
    }

  }

}

export class ConfirmDialogConfig {
  public title: string = "تایید";
  public message: string = "آیا مطمئن می باشید؟";
  public description: string = '';
  public color : 'primary' | 'red' | 'amber' = 'primary';
  icon: ConfirmDialogIcons = ConfirmDialogIcons.basic;
  actions: {
    confirm?: {
      show?: boolean;
      title?: string;
      color?:string;
    };
    cancel?: {
      show?: boolean;
      title?: string;
      color?:string;
    };
  } = { confirm: { title: 'بله', show: true }, cancel: { title: 'خیر', show: true } };
  dismissible?: boolean;
}

export enum ConfirmDialogIcons {
  basic,
  info,
  success,
  warning,
  error
}
