import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { ActivatedRoute, Router } from '@angular/router';
import { FinancialAttachment } from 'src/app/modules/bursary/entities/financial-attachmen';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-show-attachment',
  templateUrl: './show-attachment.component.html',
  styleUrls: ['./show-attachment.component.scss']
})
export class ShowAttachmentComponent implements OnInit {

  baseUrl : string = environment.fileServer+"/";
  address : string = "";
  isAttachement :boolean=true;
  attachments : FinancialAttachment[] = [];

  constructor(private route: ActivatedRoute,
    public dialogRef: MatDialogRef<ShowAttachmentComponent>,
    private router: Router,
    @Inject(MAT_DIALOG_DATA) public value: any)
     {

      }

  ngOnInit(): void {

    if (this.value.data == null ||this.value.data == "null" )  {
      this.isAttachement = false;
      }
      else
      {
        let files = JSON.parse(this.value.data);
        this.attachments = files;

        this.attachments.forEach((item:FinancialAttachment)=>{
          item.addressUrl = this.baseUrl+ item.addressUrl;
        })
      }

  }

}
