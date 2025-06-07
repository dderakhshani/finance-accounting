import { Component, Inject, OnInit, Optional } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialog, MatDialogRef } from '@angular/material/dialog';
import { AttachmentsModel } from 'src/app/core/components/custom/uploader/uploader.component';
import { ConfirmDialogComponent, ConfirmDialogIcons } from 'src/app/core/components/material-design/confirm-dialog/confirm-dialog.component';
import { Mediator } from 'src/app/core/services/mediator/mediator.service';
import { ChequeSheet } from 'src/app/modules/bursary/entities/cheque-sheet';
import { FinancialAttachment } from 'src/app/modules/bursary/entities/financial-attachmen';
import { CreateChequeSheetAttachmentCommand } from 'src/app/modules/bursary/repositories/cheque/commands/create-cheque-sheet-attachment-command';
import { CreateFinancialAttachmentCommand } from 'src/app/modules/bursary/repositories/financial-request/customer-receipt/commands/create-financial-attachment-command';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-cheque-attachments',
  templateUrl: './cheque-attachments.component.html',
  styleUrls: ['./cheque-attachments.component.scss']
})
export class ChequeAttachmentsComponent implements OnInit {

  public isUploading !: boolean;
  public imageUrls: string[] = [];
  baseUrl : string = environment.fileServer+"/";
  public attachmentsModel!: AttachmentsModel;

  attachments:FinancialAttachment[] =[]

chequeSheet !: ChequeSheet;
  set files(values: string[]) {
    this.imageUrls = [];
    values.forEach((value: string) => {
      this.imageUrls.push(value);
    })
  }


  constructor(private _mediator: Mediator, @Optional() public dialogRef: MatDialogRef<ChequeAttachmentsComponent>, @Inject(MAT_DIALOG_DATA) public value: any, public dialog: MatDialog) { }

  ngOnInit(): void {
    this.attachmentsModel = {
      typeBaseId: 28690, //  348 is typeBasevalueId for bursaryAttachment // TODO : it should be ENUM
      title: 'دریافت خزانه داری',
      description: 'دریافت خزانه داری',
      keyWords: 'ReceiveBursary',
    };

    if (this.value.data != null && this.value.data != "null")
      this.chequeSheet = JSON.parse(this.value.data);
  }

  saveAttachments() {
    this.imageUrls.forEach((item: any) => {
      let command = new FinancialAttachment();
      command.addressUrl = item.filePath;
      command.financialRequestId = this.chequeSheet.financialRequestId;
      command.isVerified = true;
      command.chequeSheetId = this.chequeSheet.id;
      command.attachmentId = item.id;
      this.attachments.push(command);
    });
    let chequeAttachments = new CreateChequeSheetAttachmentCommand();
    chequeAttachments.attachments = this.attachments
    let result = this._mediator.send(<CreateChequeSheetAttachmentCommand>chequeAttachments);


    this.dialogRef.close();
  }


  // removeDocument(item : any ){
  //   const dialogRef = this.dialog.open(ConfirmDialogComponent, {
  //     data: {
  //       title: 'تایید حذف',
  //       message: 'آیا از انتخاب خود مطمئن هستید ؟',
  //       icon: ConfirmDialogIcons.warning,
  //       actions: {
  //         confirm: { title: 'بله', show: true }, cancel: { title: 'خیر', show: true }
  //       }
  //     }
  //   });
  //   dialogRef.afterClosed().subscribe(async result => {
  //     if (result == true) {
  //     //  this.attachments.forEach(doc=>{
  //     //      if (doc.attachmentId == item.attachmentId)
  //     //          doc.isDeleted = true;
  //     //  });
  //     }
  //     // const indexToRemove = this.attachments.findIndex(item => item.isDeleted === true);
  //     // if (indexToRemove !== -1) {
  //     //   // this.attachments.splice(indexToRemove, 1); // Remove the item at the specified index
  //     // }

  //   });
  // }


}
