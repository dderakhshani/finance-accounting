import { Component, Inject } from '@angular/core';

import { MAT_DIALOG_DATA, MatDialogRef } from "@angular/material/dialog";
import { BaseComponent } from "../../../../../core/abstraction/base.component";
import { PageModes } from "../../../../../core/enums/page-modes";
import { Mediator } from "../../../../../core/services/mediator/mediator.service";
import { Units } from '../../../entities/units';
import { CreateUnitCommodityQuotaCommand } from '../../../repositories/unitCommodityQuota/commands/create-unitCommodityQuota-command';
import { DeleteUnitCommodityQuotaCommand } from '../../../repositories/unitCommodityQuota/commands/delete-unitCommodityQuota-command';
import { UpdateUnitCommodityQuotaCommand } from '../../../repositories/unitCommodityQuota/commands/update-unitCommodityQuota-command';
import { GetUnitsQuery } from '../../../repositories/units/get-units-query';
import { UnitCommodityQuota } from '../../../entities/unitCommodityQuota';
import { Commodity } from '../../../../commodity/entities/commodity';
import { GetQuotaGroupQuery } from '../../../repositories/unitCommodityQuota/queries/get-quota-group-query';
import { QuotaGroup } from '../../../entities/quota-group';

@Component({
  selector: 'app-unitCommodityQuota-dialog',
  templateUrl: './unitCommodityQuota-dialog.component.html',
  styleUrls: ['./unitCommodityQuota-dialog.component.scss']
})
export class UnitCommodityQuotaDialogComponent extends BaseComponent {

  pageModes = PageModes;
  UnitCommodityQuota!: UnitCommodityQuota;

  QuotaGroups: QuotaGroup[] = [];

  constructor(
    private _mediator: Mediator,
    private dialogRef: MatDialogRef<UnitCommodityQuotaDialogComponent>,
    @Inject(MAT_DIALOG_DATA) data: any
  ) {
    super();

    this.UnitCommodityQuota = data.UnitCommodityQuota;
    this.pageMode = data.pageMode;
    this.request = new CreateUnitCommodityQuotaCommand();

  }


  async ngOnInit() {
    await this.resolve();
  }

  async resolve() {

    await this.UnitsFilter();
    await this.initialize()
  }

  async initialize() {

    if (this.pageMode === PageModes.Add) {
      let newRequest = new CreateUnitCommodityQuotaCommand()

      this.request = newRequest;
    }
    if (this.pageMode === PageModes.Update) {
      this.request = new UpdateUnitCommodityQuotaCommand().mapFrom(this.UnitCommodityQuota)
    }


  }

  async add(){

    await this._mediator.send(<CreateUnitCommodityQuotaCommand>this.request).then(res => {

      this.dialogRef.close({

        response: res,
        pageMode: this.pageMode
      })

    });
  }

  async update(entity?: any) {
    await this._mediator.send(<UpdateUnitCommodityQuotaCommand>this.request).then(res => {
      this.dialogRef.close({
        response: res,
        pageMode: this.pageMode
      })
    });
  }

  async delete() {
    await this._mediator.send(new DeleteUnitCommodityQuotaCommand((<UpdateUnitCommodityQuotaCommand>this.request).id ?? 0)).then(res => {

      this.dialogRef.close({
        response: res,
        pageMode: PageModes.Delete
      })

    });
  }

  getCommodityById(item: Commodity) {


    this.form.controls.commodityId.setValue(item?.id);
  }

  async UnitsFilter() {

    await this._mediator.send(new GetQuotaGroupQuery(0, 0, undefined)).then(res => {

      this.QuotaGroups = res.data
    })

  }
  QuotaGroupselect(id: number) {

    this.form.controls.quotaGroupsId.setValue(id);

  }
  get(id?: number) {
    throw new Error('Method not implemented.');
  }

  close() {
    throw new Error('Method not implemented.');
  }

}

