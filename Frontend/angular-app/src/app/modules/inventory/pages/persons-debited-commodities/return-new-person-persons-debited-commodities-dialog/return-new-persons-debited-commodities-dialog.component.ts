import { Component, Inject } from '@angular/core';

import { MAT_DIALOG_DATA, MatDialogRef } from "@angular/material/dialog";
import { BaseComponent } from "../../../../../core/abstraction/base.component";
import { PageModes } from "../../../../../core/enums/page-modes";
import { Mediator } from "../../../../../core/services/mediator/mediator.service";
import { Units } from '../../../entities/units';
import { PersonsDebitedCommodities } from '../../../entities/persons-debited-commodities';
import { UpdateNewPersonsDebitedCommoditiesCommand } from '../../../repositories/persons-debited-commodities/commands/update-new-persons-debited-commodities-command';
import { PagesCommonService } from '../../../../../shared/services/pages/pages-common.service';
import { environment } from 'src/environments/environment';
import { AttachmentAssets } from '../../../entities/attachment';
import { AttachmentsModel, UploadFileData } from '../../../../../core/components/custom/uploader/uploader.component';
import { GetAssetAttachmentsIdByAssetIdQuery } from '../../../repositories/assets/queries/get-assets-attachments-query-by-assetesId';
@Component({
  selector: 'app-return-new-persons-debited-commodities-dialog',
  templateUrl: './return-new-persons-debited-commodities-dialog.component.html',
  styleUrls: ['./return-new-persons-debited-commodities-dialog.component.scss']
})
export class UpdateNewPersonsDebitedDialogComponent extends BaseComponent {


  pageModes = PageModes;
  PersonsDebitedCommodity!: PersonsDebitedCommodities;

  //------------Attachment--------------------------
  attachmentAssets: AttachmentAssets[] = [];
  public attachmentsModel: AttachmentsModel;
  public isUploading !: boolean;
  public attachmentIds: number[] = [];
  public imageUrls: UploadFileData[] = [];
  public baseUrl: string = environment.fileServer + "/";
  public attachments!: any;

  set files(values: string[]) {
    this.imageUrls = [];

    values.forEach((value: any) => {
      this.imageUrls.push(value);
    })
  }
  //--------------------------------------------------

  constructor(
    private _mediator: Mediator,
    public Service: PagesCommonService,
    private dialogRef: MatDialogRef<UpdateNewPersonsDebitedDialogComponent>,
    @Inject(MAT_DIALOG_DATA) data: any,
    @Inject(MAT_DIALOG_DATA) public value: any,
  ) {
    super();

    this.PersonsDebitedCommodity = data.PersonsDebitedCommodity;
    this.pageMode = data.pageMode;
    this.request = new UpdateNewPersonsDebitedCommoditiesCommand();

    this.attachmentsModel = {
      typeBaseId: this.Service.AttachmentAssets100,
      title: 'کالا اموال',
      description: 'واگذاری کالا اموال',
      keyWords: 'CommodityAssets',
    };
  }


  async ngOnInit() {
   
    await this.resolve();
  }
  
  async resolve() {

    await this.initialize()
  }

  async initialize() {

    if (this.pageMode === PageModes.Update) {
      this.request = new UpdateNewPersonsDebitedCommoditiesCommand().mapFrom(this.PersonsDebitedCommodity)
    }

    this.disableControls()
  }

  async update(entity?: any) {
    this.attachmentAssets = [];
    this.imageUrls.forEach(res => {
      let command: AttachmentAssets = {
        addressUrl: res.filePath,
        attachmentId: res.id,
        assetsId: this.PersonsDebitedCommodity.assetId,
        personsDebitedCommoditiesId: this.PersonsDebitedCommodity.id,
        
      };
      this.attachmentAssets.push(command);
    })

    let newRequst = new UpdateNewPersonsDebitedCommoditiesCommand().mapFrom(this.PersonsDebitedCommodity)
    newRequst.debitAccountReferenceId = this.form.controls['debitAccountReferenceId'].value;
    newRequst.debitAccountReferenceGroupId = this.form.controls['debitAccountReferenceGroupId'].value;
    newRequst.createDate = this.form.controls['createDate'].value;
    newRequst.attachmentAssets = this.attachmentAssets;
   
    
    await this._mediator.send(<UpdateNewPersonsDebitedCommoditiesCommand>newRequst).then(res => {
      this.dialogRef.close({
        response: res,
        pageMode: this.pageMode
      })
    });
  }



  debitReferenceSelect(item: any) {

    this.form.controls.debitAccountReferenceId.setValue(item?.id);
    this.form.controls.debitAccountReferenceGroupId.setValue(item.accountReferenceGroupId);

  }
  disableControls() {
    this.form.controls['fullName'].disable();
    this.form.controls['commodityTitle'].disable();
    this.form.controls['commodityCode'].disable();
  }

  get(id?: number) {
    throw new Error('Method not implemented.');
  }

  close() {
    throw new Error('Method not implemented.');
  }
  delete(param?: any) {
    throw new Error('Method not implemented.');
  }
  add(param?: any) {
    throw new Error('Method not implemented.');
  }
}

