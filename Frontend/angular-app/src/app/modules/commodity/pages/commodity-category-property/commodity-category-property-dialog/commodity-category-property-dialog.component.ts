import {Component, Inject} from '@angular/core';
import {BaseComponent} from "../../../../../core/abstraction/base.component";
import {Mediator} from "../../../../../core/services/mediator/mediator.service";
import {MAT_DIALOG_DATA, MatDialogRef} from "@angular/material/dialog";
import {
  CreateCommodityCategoryPropertyCommand
} from "../../../repositories/commodity-category-property/commands/create-commodity-category-property-command";
import {MeasureUnit} from "../../../entities/measure-unit";
import {PageModes} from "../../../../../core/enums/page-modes";
import {GetMeasureUnitsQuery} from "../../../repositories/measure-units/queries/get-measure-units-query";
import {CommodityCategoryProperty} from "../../../entities/commodity-category-property";
import {
  UpdateCommodityCategoryPropertyCommand
} from "../../../repositories/commodity-category-property/commands/update-commodity-category-property-command";
import {CommodityCategory} from "../../../entities/commodity-category";
import {
  DeleteCommodityCategoryPropertyCommand
} from "../../../repositories/commodity-category-property/commands/delete-commodity-category-property-command";
import {forkJoin} from "rxjs";
import {
  GetBaseValuesByUniqueNameQuery
} from "../../../../admin/repositories/base-value/queries/get-base-values-by-unique-name-query";
import {BaseValue} from "../../../../admin/entities/base-value";

@Component({
  selector: 'app-commodity-category-property-dialog',
  templateUrl: './commodity-category-property-dialog.component.html',
  styleUrls: ['./commodity-category-property-dialog.component.scss']
})
export class CommodityCategoryPropertyDialogComponent extends BaseComponent {
  pageModes = PageModes;

  category!: CommodityCategory;
  categoryProperty!: CommodityCategoryProperty;

  categoryPropertyTypes: BaseValue[] = []

  measureUnits: MeasureUnit[] = [];
  filteredMeasureUnits: MeasureUnit[] = [];

  PageType = '';


  constructor(
    private _mediator: Mediator,
    private dialogRef: MatDialogRef<CommodityCategoryPropertyDialogComponent>,
    @Inject(MAT_DIALOG_DATA) data: any
  ) {
    super();
    this.category = data.category;
    this.categoryProperty = data.categoryProperty;
    this.pageMode = data.pageMode;

    this.request = new CreateCommodityCategoryPropertyCommand()
  }

  async ngOnInit() {
    await this.resolve()
  }

  async resolve(params?: any) {

    forkJoin([
      this._mediator.send(new GetMeasureUnitsQuery()),
      this._mediator.send(new GetBaseValuesByUniqueNameQuery('TypeCommodityCategoryProperties'))
    ]).subscribe(([
                    measureUnits,
                    categoryPropertyTypes
                  ]) => {
      this.measureUnits = measureUnits
      this.filteredMeasureUnits = measureUnits

      this.categoryPropertyTypes = categoryPropertyTypes

      this.initialize()
    });

  }

  initialize(params?: any): any {
    if (this.pageMode === PageModes.Add) {
      this.PageType ='Add'
      let newRequest = new CreateCommodityCategoryPropertyCommand()
      if (this.category) {
        newRequest.categoryId = this.category.id;
      }
      this.request = newRequest;
    }
    if (this.pageMode === PageModes.Update) {
      this.PageType = 'Update'
      this.request = new UpdateCommodityCategoryPropertyCommand().mapFrom(this.categoryProperty)
     
    }
  }


  filterMeasureUnits() {
    let searchTerm = this.form.controls['measureId']?.value;
    this.filteredMeasureUnits = [...this.measureUnits.filter(x => x.title.includes(searchTerm))]
  }

  getMeasureUnitTitle(measureUnitId: number) {
    let measureUnit = this.measureUnits.find(x => x.id === measureUnitId)
    return measureUnit ? measureUnit.title : '';
  }

  async add() {

    if (this.PageType == 'Add') {
     
      await this._mediator.send(<CreateCommodityCategoryPropertyCommand>this.request).then(res => {
        this.dialogRef.close({
          response: res,
          pageMode: this.pageMode

        })
      });
    }
    
  }

  async update(entity?: any) {

   
    if (this.PageType == 'Update') {

     
      await this._mediator.send(<UpdateCommodityCategoryPropertyCommand>this.request).then(res => {
        this.dialogRef.close({
          response: res,
          pageMode: this.pageMode
        })
      });
    }
  }

  async delete() {
    await this._mediator.send(new DeleteCommodityCategoryPropertyCommand(this.categoryProperty.id)).then(res => {
      this.dialogRef.close({
        response: res,
        pageMode: PageModes.Delete
      })
    });
  }

  close(): any {
  }


  get(param?: any): any {
  }


}
