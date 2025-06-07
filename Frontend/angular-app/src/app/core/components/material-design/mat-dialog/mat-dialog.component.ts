import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';

@Component({
  selector: 'mat-dialog',
  templateUrl: './mat-dialog.component.html',
  styleUrls: ['./mat-dialog.component.scss']
})
export class MatDialogComponent implements OnInit {
  @Input() showSubmitBtn = true;
  @Input() submitBtnTitle = "ثبت";
  @Output() onSubmit = new EventEmitter<any>();

  @Input() showDeleteBtn = true;
  @Input() deleteBtnTitle = "حذف";
  @Output() onDelete = new EventEmitter<any>();

  @Input() spinner :boolean= false;



  @Input() showHeader : boolean = true;
  @Input() showContent: boolean = true;
  @Input() showActions:boolean = true;
  constructor() { }

  ngOnInit(): void {

  }

}
