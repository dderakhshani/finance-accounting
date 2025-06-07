import {AfterViewInit, Component, Inject, OnInit, ViewChild} from '@angular/core';
import {Mediator} from "../../../../../core/services/mediator/mediator.service";
import {MAT_DIALOG_DATA, MatDialogRef} from "@angular/material/dialog";
import {
  AttachmentsModel,
  UploaderComponent,
  UploadFileData
} from "../../../../../core/components/custom/uploader/uploader.component";
import {environment} from "../../../../../../environments/environment";

import {PageModes} from "../../../../../core/enums/page-modes";
import {FormControl} from "@angular/forms";
import {MatChipInputEvent} from "@angular/material/chips";
import {UploadFileService} from "../../../../../core/services/upload-file.service";
import {NotificationService} from "../../../../../shared/services/notification/notification.service";

class Attachments extends AttachmentsModel {
  fileNumber: string = '';
}

@Component({
  selector: 'app-add-archive-dialog',
  templateUrl: './add-attachment-dialog.component.html',
  styleUrls: ['./add-attachment-dialog.component.scss']
})
export class AddAttachmentDialogComponent implements OnInit, AfterViewInit {
  @ViewChild(UploaderComponent) uploader!: UploaderComponent;

  titleHeader: string = 'افزودن سند بایگانی'
  formControlKeywords = new FormControl(['']);
  setKeywords = new Set([]);
  //------------Attachment------------------------
  attachmentAssets: number[] = [];
  public attachment: Attachments = {
    typeBaseId: 0,
    title: '',
    description: ' ',
    keyWords: '',
    fileNumber: ''
  };

  public isUploading: boolean = false;
  public attachmentIds: number[] = [];
  public imageUrls: UploadFileData[] = [];
  public baseUrl: string = environment.fileServer + "/";

  //----------call by uploder component-------------
  set files(values: string[]) {
    this.imageUrls = [];

    this.attachmentIds = [];
    values.forEach((value: any) => {
      this.imageUrls.push(value);

      this.attachmentIds.push(value.id);
    })
  }


  pageModes = PageModes;

  constructor(private _mediator: Mediator,
              private dialogRef: MatDialogRef<AddAttachmentDialogComponent>,
              private uploadFileService: UploadFileService,
              private notificationService: NotificationService,
              @Inject(MAT_DIALOG_DATA) data: any,
  ) {
    if (data.pageMode === PageModes.Add) {
      this.attachment.archiveId = data.archiveId;
      this.attachment.typeBaseId = data.typeBaseId;
      this.attachment.fileNumber = data.fileNumber;
      this.attachment.description = data.description;
      this.attachment.keyWords = data.keyWords + ',' + data.typeBaseTitle;
      this.titleHeader = 'افزودن سند بایگانی'
    } else {

      this.attachment = data.attachments;
      this.files = data.files;
      this.titleHeader = 'ویرایش سند بایگانی'
    }

    if (this.attachment.keyWords) {

      const keywordsArray = (this.attachment.keyWords ?? '').split(',');
      this.formControlKeywords.setValue(keywordsArray);
      // @ts-ignore
      this.setKeywords = new Set<string>(keywordsArray);

    }

  }

  async ngOnInit() {

  }

  async ngAfterViewInit() {

  }


  filesChange($event: UploadFileData[]) {
    this.imageUrls = $event;
  }

  async submit() {
    let fileData = <any>(this.uploader.files[0])
    if (fileData != null) {

      let formData = new FormData();
      formData.append('file', fileData.file);
      formData.append('typeBaseId', this.attachment.typeBaseId.toString());
      formData.append('title', this.attachment.title.toString());
      formData.append('description', this.attachment.description);
      formData.append('keyWords', this.attachment.keyWords);
      formData.append('archiveId', (<number>(this.attachment.archiveId)).toString());
      if (this.attachment.fileNumber) {

        formData.append('fileNumber', this.attachment.fileNumber);
      }

      this.uploadFileService.upload(formData).subscribe(() => {
        this.notificationService.showSuccessMessage()
        this.dialogRef.close({
          attachments: this.attachment,
          attachmentIds: this.attachmentIds
        })
      })


    }

  }

  addKeywordFromInput(event: MatChipInputEvent) {
    if (event.value) {
      // @ts-ignore
      this.setKeywords.add(event.value);
      event.chipInput!.clear();
      this.attachment.keyWords = Array.from(this.setKeywords).join(',');
    }
  }

  removeKeyword(keyword: string) {
    // @ts-ignore
    this.setKeywords.delete(keyword);
    this.attachment.keyWords = Array.from(this.setKeywords).join(',');
  }
}

