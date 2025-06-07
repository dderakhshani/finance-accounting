import {Component, Inject, OnInit} from '@angular/core';
import {MAT_DIALOG_DATA, MatDialogRef} from "@angular/material/dialog";

@Component({
  selector: 'app-ledger-report-advanced-filter',
  templateUrl: './ledger-report-advanced-filter.component.html',
  styleUrls: ['./ledger-report-advanced-filter.component.scss']
})
export class LedgerReportAdvancedFilterComponent implements OnInit {

  constructor(
    public dialogRef: MatDialogRef<LedgerReportAdvancedFilterComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any)  {

  }

  ngOnInit(): void {
  }

  save() {
    this.dialogRef.close();
  }

  closeDialog() {
    this.dialogRef.close();
  }

}
