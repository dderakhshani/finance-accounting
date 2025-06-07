import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { ActivatedRoute, Router } from '@angular/router';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-attachment-dialog',
  templateUrl: './attachment-dialog.component.html',
  styleUrls: ['./attachment-dialog.component.scss']
})

export class AttachmentDialogComponent implements OnInit {

  baseUrl : string = environment.crmServerAddress+"/";
  address : string = "";
  isAttachement :boolean=true;

  constructor(private route: ActivatedRoute,
    public dialogRef: MatDialogRef<AttachmentDialogComponent>,
    private router: Router,
    @Inject(MAT_DIALOG_DATA) public value: any)
     {

      }

  ngOnInit(): void {
    if (this.value.data == null ||this.value.data == "null" )  this.isAttachement = false;
    this.address =this.baseUrl+ JSON.parse(this.value.data);

  }




}
