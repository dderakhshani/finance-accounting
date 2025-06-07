import {Component, Inject} from '@angular/core';
import {BaseComponent} from "../../../../../core/abstraction/base.component";
import {Mediator} from "../../../../../core/services/mediator/mediator.service";
import {MAT_DIALOG_DATA, MatDialogRef} from "@angular/material/dialog";
import {AttachmentsModel} from "../../../../../core/components/custom/uploader/uploader.component";

import {PageModes} from "../../../../../core/enums/page-modes";
import {FormControl} from "@angular/forms";
import {MatChipInputEvent} from "@angular/material/chips";
import {CreateArchiveCommand} from "../../../repositories/archives/create-archive-command";
import {SearchQuery} from "../../../../../shared/services/search/models/search-query";
import {GetBaseValuesQuery} from "../../../../admin/repositories/base-value/queries/get-base-values-query";
import {BaseValue} from "../../../../admin/entities/base-value";
import {UpdateArchiveCommand} from "../../../repositories/archives/update-archive-command";
import {da} from "date-fns/locale";

class Attachments extends AttachmentsModel {
  fileNumber: string = '';
}

@Component({
  selector: 'app-add-archive-dialog',
  templateUrl: './add-archive-dialog.component.html',
  styleUrls: ['./add-archive-dialog.component.scss']
})
export class AddArchiveDialogComponent extends BaseComponent {

  titleHeader: string = 'افزودن آرشیو'
  pageModes = PageModes;
  keywords = new Set([]);
  baseValues: BaseValue[] = []

  data: any;

  constructor(private _mediator: Mediator,
              private dialogRef: MatDialogRef<AddArchiveDialogComponent>,
              @Inject(MAT_DIALOG_DATA) data: any,
  ) {
    super()
    this.request = new CreateArchiveCommand();
    this.data = data;

  }

  async ngOnInit() {
    let searchQueries: SearchQuery[] =
      [
        {
          propertyName: 'baseValueTypeId',
          values: [584],
          comparison: 'in',
          nextOperand: 'and'
        }
      ]
    await this._mediator.send(new GetBaseValuesQuery(0, 0, searchQueries)).then(res => {
      this.baseValues = res;

      console.log(this.data.keywords);
      if (this.data.pageMode === PageModes.Add) {
        <string[]>(this.data.keywords).forEach((keyword: never) => {
          this.keywords.add(keyword);
        })
        this.titleHeader = 'افزودن سند بایگانی'
        let request = new CreateArchiveCommand();
        request.baseValueTypeId = this.data.baseValueTypeId;
        request.keyWords = this.data.keyWords;
        request.fileNumber = this.data.code;
        this.request = request;
      } else {
        this.titleHeader = 'ویرایش سند بایگانی'
        let request = new UpdateArchiveCommand().mapFrom(this.data.archive)
        this.keywords = this.data.archive.keyWords.split(',')
        request.keyWords = "";
        this.request = request;
      }
    });
  }

  async ngAfterViewInit() {

  }

  addKeywordFromInput(event: MatChipInputEvent) {
    if (event.value) {
      // @ts-ignore
      this.keywords.add(event.value);
      event.chipInput!.clear();
    }
  }

  removeKeyword(keyword: string) {
    // @ts-ignore
    this.keywords.delete(keyword);
  }

  async add(param?: any) {
    (<CreateArchiveCommand>this.request).keyWords = Array.from(this.keywords).join(',');
    this._mediator.send(<CreateArchiveCommand>this.request).then((res) => {
      this.dialogRef.close(true)
    })

  }

  update(param?: any) {
    (<UpdateArchiveCommand>this.request).keyWords = Array.from(this.keywords).join(',');
    this._mediator.send(<CreateArchiveCommand>this.request).then((res) => {
      this.dialogRef.close(true)
    })

  }

  baseValueDisplayFn(baseValueId: number) {
    return this.baseValues.find(x => x.id === baseValueId)?.title;
  }

  resolve(params?: any) {
    throw new Error('Method not implemented.');
  }

  initialize(params?: any) {
    throw new Error('Method not implemented.');
  }

  get(param?: any) {
    throw new Error('Method not implemented.');
  }

  delete(param?: any) {
    throw new Error('Method not implemented.');
  }

  close() {
    throw new Error('Method not implemented.');
  }
}

