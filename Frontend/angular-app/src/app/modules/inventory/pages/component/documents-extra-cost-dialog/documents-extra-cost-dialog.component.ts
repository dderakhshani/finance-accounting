import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from "@angular/material/dialog";
import { BaseComponent } from '../../../../../core/abstraction/base.component';
import { PageModes } from '../../../../../core/enums/page-modes';
import { Mediator } from '../../../../../core/services/mediator/mediator.service';
import { PagesCommonService } from '../../../../../shared/services/pages/pages-common.service';
import { DocumentHeadExtraCost } from '../../../entities/documentHeadExtraCost';
import { GetDocumentHeadExtraCostByDocumentHeadIdQuery } from '../../../repositories/documentHeadExtraCost/queries/get-documentHeadExtraCost-by-documentId-query ';
import { ModifyDocumentHeadExtraCostCommand } from '../../../repositories/documentHeadExtraCost/commands/create-documentHeadExtraCost-command';
import { SearchQuery } from '../../../../../shared/services/search/models/search-query';
import { Prehension } from '../../../../logistics/entities/prehension';
import { GetPrehensionQuery } from '../../../../logistics/repositories/prehension/queries/get-all-prehension-query';


@Component({
  selector: 'app-documents-extra-cost-dialog',
  templateUrl: './documents-extra-cost-dialog.component.html',
  styleUrls: ['./documents-extra-cost-dialog.component.scss']
})
export class DocumentHeadExtraCostDialogComponent extends BaseComponent {

  documentHeadExtraCosts: DocumentHeadExtraCost[] = [];

  pageModes = PageModes;
  documentHeadIds: any[];
  Prehensions: Prehension[] = [];


  constructor(
    private _mediator: Mediator,
    public Service: PagesCommonService,
    private dialogRef: MatDialogRef<DocumentHeadExtraCostDialogComponent>,
    @Inject(MAT_DIALOG_DATA) data: any
  ) {
    super();

    this.documentHeadIds = data.documentHeadIds;
    this.get();
    this.pageMode = PageModes.Add;

  }


  async ngOnInit() {
    await this.resolve();
  }

  async resolve() {


    await this.initialize()

  }

  async initialize() {

    this.addItems();


  }

  async update(entity?: any) {

  }

  async delete(item: any) {

    item.isDeleted = true;
    
  }
  async add() {

    let model = new ModifyDocumentHeadExtraCostCommand()

    this.documentHeadExtraCosts.forEach(a => {
      a.id < 0 ? a.id = 0 : a.id;

    });

    model.documentHeadExtraCosts = this.documentHeadExtraCosts;

    await this._mediator.send(<ModifyDocumentHeadExtraCostCommand>model).then(res => {
      this.dialogRef.close({
        response: this.documentHeadExtraCosts.filter(a=> !a.isDeleted).reduce((sum, current) => sum + Number(current.extraCostAmount), 0),
        pageMode: this.pageMode
      })
    });
  }

  debitReferenceSelect(item: any, DocumentHeadExtraCost: DocumentHeadExtraCost) {

    DocumentHeadExtraCost.extraCostAccountReferenceGroupId = item.accountReferenceGroupId;
    DocumentHeadExtraCost.extraCostAccountReferenceId = item?.id;
  }

  debitAccountHeadIdSelect(item: any, DocumentHeadExtraCost: DocumentHeadExtraCost) {

    DocumentHeadExtraCost.extraCostAccountHeadId = item?.id;

  }


  async get(id?: number) {
    let request = new GetDocumentHeadExtraCostByDocumentHeadIdQuery();
    request.listIds = this.documentHeadIds;
    this.documentHeadExtraCosts = (await this._mediator.send(request)).data;
  }

  addItems() {
    var model = new DocumentHeadExtraCost();
    model.id = (this.documentHeadExtraCosts.length + 1) * -1
    model.documentHeadId = this.documentHeadIds[0];
    model.extraCostAmount = 0;
    model.extraCostAccountHeadId = this.Service.extraCostAccountHeadId;
    this.documentHeadExtraCosts.push(model);
  }

  close() {
    this.dialogRef.close({
      response: this.documentHeadExtraCosts.reduce((sum, current) => sum + Number(current.extraCostAmount), 0),
      pageMode: this.pageMode
    })
  }

  async getPrehension(barCode: string) {
    var filter: SearchQuery[] = [
      new SearchQuery({
        propertyName: 'prehensionCode',
        comparison: 'contains',
        values: [barCode],
        nextOperand: 'and'
      })

    ];
    await this._mediator.send(new GetPrehensionQuery(0, 25, filter, '')).then(res => {

      this.Prehensions = res.data;
      this.Prehensions.forEach(a => a.title = a.productTitle);
    })

  }
  PrehensionFilter(item: Prehension, DocumentHeadExtraCost: DocumentHeadExtraCost) {


    DocumentHeadExtraCost.extraCostAmount = Number(item.transferPrice);

    DocumentHeadExtraCost.barCode = item.id;
    
  }

}

