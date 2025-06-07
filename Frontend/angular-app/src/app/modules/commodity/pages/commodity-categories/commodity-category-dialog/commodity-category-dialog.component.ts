import {Component, Inject, OnInit} from '@angular/core';
import {BaseComponent} from "../../../../../core/abstraction/base.component";
import {PageModes} from "../../../../../core/enums/page-modes";
import {Mediator} from "../../../../../core/services/mediator/mediator.service";
import {MAT_DIALOG_DATA, MatDialogRef} from "@angular/material/dialog";
import {
  UpdateCommodityCategoryCommand
} from "../../../repositories/commodity-category/commands/update-commodity-category-command";
import {CommodityCategory} from "../../../entities/commodity-category";
import {
  CreateCommodityCategoryCommand
} from "../../../repositories/commodity-category/commands/create-commodity-category-command";
import {
  DeleteCommodityCategoryCommand
} from "../../../repositories/commodity-category/commands/delete-commodity-category-command";
import {MeasureUnit} from "../../../entities/measure-unit";
import {GetMeasureUnitsQuery} from "../../../repositories/measure-units/queries/get-measure-units-query";

@Component({
  selector: 'app-commodity-category-dialog',
  templateUrl: './commodity-category-dialog.component.html',
  styleUrls: ['./commodity-category-dialog.component.scss']
})
export class CommodityCategoryDialogComponent extends BaseComponent {


  pageModes = PageModes;
  commodityCategory!: CommodityCategory;

  measureUnits:MeasureUnit[] = [];
  filteredMeasureUnits:MeasureUnit[] = [];

  codingModes:any[] = []
  constructor(
    private _mediator:Mediator,
    private dialogRef: MatDialogRef<CommodityCategoryDialogComponent>,
    @Inject(MAT_DIALOG_DATA) data: any
  ) {
    super();
    this.commodityCategory = data.commodityCategory;
    this.pageMode = data.pageMode;

    this.request = new CreateCommodityCategoryCommand();
  }

  async ngOnInit() {
    await this.resolve();
  }

  async resolve() {

    this.codingModes = [
      {
        id:1,
        title:'بر اساس گروه'
      },
      {
        id:2,
        title:'بر اساس خصوصیات'
      },
    ]
    await this._mediator.send(new GetMeasureUnitsQuery()).then(res => {
      this.measureUnits = res
      this.filteredMeasureUnits = res
    })
    this.initialize()
  }


  initialize() {
    if (this.pageMode === PageModes.Add) {
      let newRequest = new CreateCommodityCategoryCommand()
      if (this.commodityCategory) {
        newRequest.parentId = this.commodityCategory.id;
      }
      this.request = newRequest;
    }
    if (this.pageMode === PageModes.Update) {
      this.request = new UpdateCommodityCategoryCommand().mapFrom(this.commodityCategory)
    }
  }


  async add() {
    await this._mediator.send(<CreateCommodityCategoryCommand>this.request).then(res => {
      this.dialogRef.close({
        response:res,
        pageMode:this.pageMode
      })
    });
  }

  async update(entity?: any) {
    await this._mediator.send(<UpdateCommodityCategoryCommand>this.request).then(res => {
      this.dialogRef.close({
        response:res,
        pageMode:this.pageMode
      })
    });
  }

  async delete() {
    await this._mediator.send(new DeleteCommodityCategoryCommand((<UpdateCommodityCategoryCommand>this.request).id ?? 0)).then(res => {
      this.dialogRef.close({
        response:res,
        pageMode:PageModes.Delete
      })
    });
  }

  filterMeasureUnits() {
    let searchTerm = this.form.controls['measureId']?.value;
    this.filteredMeasureUnits = [...this.measureUnits.filter(x => x.title.includes(searchTerm))]
  }

  getMeasureUnitTitle(measureUnitId: number) {
    let measureUnit = this.measureUnits.find(x => x.id === measureUnitId)
    return measureUnit ? measureUnit.title : '';
  }



  get(id?: number) {
    throw new Error('Method not implemented.');
  }

  close() {
    throw new Error('Method not implemented.');
  }

}
