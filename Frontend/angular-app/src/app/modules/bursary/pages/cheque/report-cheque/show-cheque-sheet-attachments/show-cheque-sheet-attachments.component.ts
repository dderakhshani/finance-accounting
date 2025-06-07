import { Component, Inject, OnInit, Optional } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialog, MatDialogRef } from '@angular/material/dialog';
import { Mediator } from 'src/app/core/services/mediator/mediator.service';
import { FinancialAttachment } from 'src/app/modules/bursary/entities/financial-attachmen';
import { GetChequeSheetAttachmentsQuery } from 'src/app/modules/bursary/repositories/financial-request-attachment/queries/get-cheque-sheet-attachments-query';
import { SearchQuery } from 'src/app/shared/services/search/models/search-query';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-show-cheque-sheet-attachments',
  templateUrl: './show-cheque-sheet-attachments.component.html',
  styleUrls: ['./show-cheque-sheet-attachments.component.scss']
})
export class ShowChequeSheetAttachmentsComponent implements OnInit {

  attachments:FinancialAttachment[] =[];
  chequeSheetId!:number;
  baseUrl : string = environment.fileServer+"/";

  constructor(private _mediator: Mediator , @Optional() public dialogRef: MatDialogRef<ShowChequeSheetAttachmentsComponent>, @Inject(MAT_DIALOG_DATA) public value: any, public dialog: MatDialog) { }

  async ngOnInit() {


    if (this.value.data != null && this.value.data != "null")
      this.chequeSheetId = JSON.parse(this.value.data);

    let searchQueries: SearchQuery[] = [];
    searchQueries.push(new SearchQuery({
      propertyName: 'chequeSheetId',
      values: [ this.chequeSheetId],
      comparison: 'equal'
    }))

  let result  = await this._mediator.send(new GetChequeSheetAttachmentsQuery(0, 25, searchQueries));

  result.data.forEach(element => {
    element.addressUrl = this.baseUrl+element.addressUrl
    this.attachments.push(element);
  });


  }

}
