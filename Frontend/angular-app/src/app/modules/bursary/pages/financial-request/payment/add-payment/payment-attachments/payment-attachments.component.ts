import { Component, Inject, OnInit, Optional } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialog, MatDialogRef } from '@angular/material/dialog';
import { AttachmentsModel } from 'src/app/core/components/custom/uploader/uploader.component';
import { ConfirmDialogComponent, ConfirmDialogIcons } from 'src/app/core/components/material-design/confirm-dialog/confirm-dialog.component';
import { CreateFinancialAttachmentCommand } from 'src/app/modules/bursary/repositories/financial-request/customer-receipt/commands/create-financial-attachment-command';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-payment-attachments',
  templateUrl: './payment-attachments.component.html',
  styleUrls: ['./payment-attachments.component.scss']
})
export class PaymentAttachmentsComponent implements OnInit {


  public isUploading !: boolean;
  public imageUrls: string[] = [];
  baseUrl : string = environment.fileServer+"/";

  attachments: CreateFinancialAttachmentCommand[] = [];
  public attachmentsModel!: AttachmentsModel;

  set files(values: string[]) {
    this.imageUrls = [];
    values.forEach((value: string) => {
      this.imageUrls.push(value);
    })
  }

  constructor(@Optional() public dialogRef: MatDialogRef<PaymentAttachmentsComponent>, @Inject(MAT_DIALOG_DATA) public value: any, public dialog: MatDialog) { }

  ngOnInit(): void {

    this.attachmentsModel = {
      typeBaseId: 28690, //  348 is typeBasevalueId for bursaryAttachment // TODO : it should be ENUM
      title: 'پرداخت خزانه داری',
      description: 'پرداخت خزانه داری',
      keyWords: 'PaymentBursary',
    };

    if (this.value.data != null && this.value.data != "null") {
      let files = JSON.parse(this.value.data);
      this.attachments = files;
      this.attachments.forEach((item:any)=>{
        item.addressUrl = this.baseUrl+ item.addressUrl;
        item.attachmentId = item.id;
      });
    }
  }


  saveAttachments() {
    this.imageUrls.forEach((item: any) => {
      let command = new CreateFinancialAttachmentCommand();
      command.addressUrl = item.filePath;
      command.financialRequestId = 0;
      command.attachmentId = item.id;
      command.isVerified = true;
      this.attachments.push(<CreateFinancialAttachmentCommand>command);
    })
    this.dialogRef.close(this.imageUrls.length != 0 ? this.attachments : (this.attachments =[]));
  }


  removeDocument(item : any ){
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      data: {
        title: 'تایید حذف',
        message: 'آیا از انتخاب خود مطمئن هستید ؟',
        icon: ConfirmDialogIcons.warning,
        actions: {
          confirm: { title: 'بله', show: true }, cancel: { title: 'خیر', show: true }
        }
      }
    });
    dialogRef.afterClosed().subscribe(async result => {
      if (result == true) {
       this.attachments.forEach(doc=>{
           if (doc.attachmentId == item.attachmentId)
               doc.isDeleted = true;
       });
      }
      const indexToRemove = this.attachments.findIndex(item => item.isDeleted === true);
      if (indexToRemove !== -1) {
        this.attachments.splice(indexToRemove, 1); // Remove the item at the specified index
      }

    });
  }

}


