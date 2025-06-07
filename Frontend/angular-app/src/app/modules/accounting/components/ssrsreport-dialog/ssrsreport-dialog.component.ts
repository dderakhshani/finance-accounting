import {Component, Inject, OnInit} from '@angular/core';
import {MAT_DIALOG_DATA} from "@angular/material/dialog";
import {da} from "date-fns/locale";
import {DomSanitizer} from "@angular/platform-browser";
import {environment} from "../../../../../environments/environment";

@Component({
  selector: 'app-ssrsreport-dialog',
  templateUrl: './ssrsreport-dialog.component.html',
  styleUrls: ['./ssrsreport-dialog.component.scss']
})
export class SSRSReportDialogComponent implements OnInit {

  sourceURL!: any;
  SSRSServerAddress: string =environment.SSRSServerAddress;
  constructor(
    @Inject(MAT_DIALOG_DATA) data: any,
    private sanitizer:DomSanitizer
  ) {

    this.sourceURL = this.sanitizer.bypassSecurityTrustResourceUrl(this.SSRSServerAddress + data.sourceURL);
    console.log('SSRS_URL : ',this.SSRSServerAddress + data.sourceURL)
  }

  ngOnInit(): void {
  }

}
