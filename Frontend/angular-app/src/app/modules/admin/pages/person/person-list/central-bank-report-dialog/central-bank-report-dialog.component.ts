import { Component, OnInit } from '@angular/core';
import {GetCentralBankReportQuery} from "../../../../repositories/person/queries/get-central-bank-report-query";
import {Mediator} from "../../../../../../core/services/mediator/mediator.service";
import {FormControl} from "@angular/forms";
import {IdentityService} from "../../../../../identity/repositories/identity.service";
import {id} from "date-fns/locale";
import {MatDialogRef} from "@angular/material/dialog";

@Component({
  selector: 'app-central-bank-report-dialog',
  templateUrl: './central-bank-report-dialog.component.html',
  styleUrls: ['./central-bank-report-dialog.component.scss']
})
export class CentralBankReportDialogComponent implements OnInit {

  fromDate = new FormControl();
  toDate = new FormControl();
  constructor(
    private _mediator:Mediator,
    private identityService:IdentityService,
    public dialogRef: MatDialogRef<CentralBankReportDialogComponent>,

  ) { }

  ngOnInit(): void {
    let currentYear = this.identityService.applicationUser.years.find(x => x.id == this.identityService.applicationUser.yearId);
    this.fromDate.setValue(currentYear?.firstDate)
    this.toDate.setValue(currentYear?.lastDate)
  }


  async getCentralBankReport() {
    // await this._mediator.send(new GetCentralBankReportQuery(this.fromDate.value, this.toDate.value)).then(result => {
    //   const binaryData = atob(result);
    //
    //   // Convert binary string to an array of bytes
    //   const byteNumbers = new Uint8Array(binaryData.length);
    //   for (let i = 0; i < binaryData.length; i++) {
    //     byteNumbers[i] = binaryData.charCodeAt(i);
    //   }
    //
    //   // Create a Blob from the byte array
    //   const blob = new Blob([byteNumbers], {type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet'});
    //
    //   // Create a link element to trigger download
    //   const url = window.URL.createObjectURL(blob);
    //   const a = document.createElement('a');
    //   a.href = url;
    //   a.download = 'گزارش بانک مرکزی.xlsx'; // Set the file name with .xlsx extension
    //   a.click();
    //
    //   // Clean up and release the object URL
    //   window.URL.revokeObjectURL(url);
    //   this.dialogRef.close()
    // })

  }
}
