import {HttpErrorResponse, HttpEvent, HttpEventType} from '@angular/common/http';
import {Component, ElementRef, Input, OnInit, Output, ViewChild, EventEmitter} from '@angular/core';

import {of} from 'rxjs';
import {catchError, map} from 'rxjs/operators';
import {UploadFileService} from 'src/app/core/services/upload-file.service';
import {environment} from 'src/environments/environment';
import {NotificationService} from '../../../../shared/services/notification/notification.service';

@Component({
  selector: 'app-uploader',
  templateUrl: './uploader.component.html',
  styleUrls: ['./uploader.component.scss']
})
export class UploaderComponent implements OnInit {

  @ViewChild('fileUpload', {static: false}) fileUpload!: ElementRef;
  @Input() autoUpload: boolean = true;
  @Input() allowFileUpload: boolean = true;
  @Input() attachmentIds: number[] = [];
  @Input() singleFile: boolean = false;
  @Input() attachmentsModel: AttachmentsModel = new AttachmentsModel();
  @Output() filesChange: EventEmitter<UploadFileData[]> = new EventEmitter<UploadFileData[]>();
  @Output() isUploadingChange: EventEmitter<boolean> = new EventEmitter<boolean>();
  @Input() fileFilters: string = "image/x-png,image/jpeg,image/gif,xlsx"

  isUploading: boolean = false;
  @Input() files: UploadFileData[] = [];
  isLargSizeShow: boolean = false

  modalImg: any;
  captionText: any;


  constructor(private uploadFileService: UploadFileService, public _notificationService: NotificationService) {
  }

  ngOnInit(): void {

  }

  async ngOnChanges() {
    if (this.attachmentIds.length > 0) {
      this._notificationService.isLoaderDropdown = true;
      this.uploadFileService.getAttachments(this.attachmentIds).subscribe((result: any) => {


        if (result != undefined) {
          this.files = result?.body;
          this.files.forEach(data => {
            data.fullFilePath = `${environment.fileServer}` + "/" + data.filePath;
          })

        }
        this._notificationService.isLoaderDropdown = false;
      });


    }
  }

  ngAfterViewInit(): void {
    const fileUpload = this.fileUpload.nativeElement;
    fileUpload.onchange = () => {
      const file = fileUpload.files[0];


      const fileData = <UploadFileData>{
        file: file,
        progressStatus: '',
        progress: 0,
        filePath: '',
        fullFilePath: '',
        extention: file.name.split(".")[1].toUpperCase(),

      }
      if (FileReader) {
        var fr = new FileReader();
        fr.onload = function () {
          fileData.fullFilePath = fr.result;
        }
        fr.readAsDataURL(file);
      }

      this.files.push(fileData);
      if (this.autoUpload)
        this.uploadFile(fileData);
    };
    console.log('hi')

  }


  onClickUpload() {
    const fileUpload = this.fileUpload.nativeElement;
    fileUpload.click();
  }

  uploadAll() {
    this.files.forEach(f => {
      if (f.progressStatus == '')
        this.uploadFile(f);
    })
  }

  uploadFile(data: UploadFileData) {


    let formData = new FormData();
    formData.append('file', data.file);
    formData.append('typeBaseId', this.attachmentsModel.typeBaseId.toString());
    formData.append('title', this.attachmentsModel.title.toString());
    formData.append('description', this.attachmentsModel.description);
    formData.append('keyWords', this.attachmentsModel.keyWords);
    if (this.attachmentsModel.fileNumber) {

      formData.append('fileNumber', this.attachmentsModel.fileNumber);
    }

    this.isUploadingChange.emit(true);
    data.progressStatus = 'in-progress';

    this.uploadFileService.upload(formData).pipe(map((event: HttpEvent<any>) => {

        switch (event.type) {
          case HttpEventType.UploadProgress:
            data.progress = Math.round(
              (event.loaded * 100) / (event.total ?? 1)
            );
            break;
          case HttpEventType.Response:
            let fileName = event.body?.objResult;
            data.extention = fileName.extention;
            data.filePath = fileName.url;
            data.fullFilePath = `${environment.fileServer}` + "/" + fileName.url;
            data.id = fileName.id;
            data.progressStatus = 'uploaded';


            if (this.files.filter(x => x.progressStatus == 'in-progress').length == 0)
              this.isUploadingChange.emit(false);

            this.filesChange.emit(this.files);

            return fileName;

        }
      }),
      catchError((error: HttpErrorResponse) => {
        data.progressStatus = 'failed';
        return of(`${data.file.name} upload failed.`);
      })
    )
      .subscribe((event: any) => {
        if (typeof event === 'object') {
        }
      });


  }

  isImageFile(file: UploadFileData) {
    return ['PNG', 'JPG', 'JPEG', 'XLS', 'xlsx'].includes(file.extention.toUpperCase());
  }

  async removeImage(file: UploadFileData) {

    await (await this.uploadFileService.delete(file.id)).subscribe(a => {

      this.files.splice(this.files.indexOf(file), 1);
      file.id = 0;
      this.filesChange.emit(this.files);

    })


  }

  ondblclick() {

    this.isLargSizeShow = false;
  }

  onclick(src: any, alt: any) {


    this.modalImg = src;
    this.captionText = alt;

    this.isLargSizeShow = true;
  }

  checkIsDownlod(extention: string): boolean {
    if (extention == '.png' || extention == '.jpg' || extention == '.jpeg') {
      return false;
    } else {
      return true;
    }
  }
}

export interface UploadFileData {
  file: any;
  progressStatus: '' | 'in-progress' | 'uploaded' | 'failed';
  progress: number;
  filePath: string;
  fullFilePath: any;
  extention: string;
  id: number;
}

export class AttachmentsModel {

  public typeBaseId: number = 0;
  public title: string = '';
  public description: string = '';
  public keyWords: string = '';
  public fileNumber?: string;
  public archiveId?: number;


}
