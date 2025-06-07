import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from "@angular/material/dialog";
import { BaseComponent } from "../../../../../core/abstraction/base.component";
import { PageModes } from "../../../../../core/enums/page-modes";
import { UpdateWarehouseLayoutCommand } from "../../../repositories/warehouse-layout/commands/update-warehouse-layout-command";
import { Mediator } from "../../../../../core/services/mediator/mediator.service";
import { DeleteWarehouseLayoutCommand } from "../../../repositories/warehouse-layout/commands/delete-warehouse-layout-command";
import { WarehouseLayout,_WarhousteCategoryItemsEdit } from "../../../entities/warehouse-layout";
import { CommodityCategory } from "../../../../commodity/entities/commodity-category";
import { CommodityCategoryProperty } from '../../../../commodity/entities/commodity-category-property';
import { CommodityCategoryPropertyItem } from '../../../../commodity/entities/commodity-category-property-item';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { GetCommodityCategoryPropertyItemsQuery } from '../../../../commodity/repositories/commodity-category-property-item/queries/get-commodity-category-property-items-query';
import { GetCommodityCategoryPropertiesQuery } from '../../../../commodity/repositories/commodity-category-property/queries/get-commodity-category-properties-query';
import { DeleteWarehouseLayoutCategoryCommand } from '../../../repositories/warehouse-layout/commands/warhouste-category-items/delete-warehouse-layout-category-command';
import { DeleteWarehouseLayoutPropertiesCommand } from '../../../repositories/warehouse-layout/commands/warhouste-category-items/delete-warehouse-layout-properties-command';
import {SearchQuery} from "../../../../../shared/services/search/models/search-query";
import { GetInverntoryCommodityCategoriesQuery } from '../../../repositories/commodity-categories/get-commodity-categories-query';
import { GetInverntoryCommodityCategoriesPropertyQuery } from '../../../repositories/commodity-categories/get-commodity-categories-Property-query';
import { GetInverntoryCommodityCategoriesPropertyItemsQuery } from '../../../repositories/commodity-categories/get-commodity-categories-Property-items-query';
import { NotificationService } from '../../../../../shared/services/notification/notification.service';

@Component({
  selector: 'app-update-warehouse-layout-dialog',
  templateUrl: './update-warehouse-layout-dialog.component.html',
  styleUrls: ['./update-warehouse-layout-dialog.component.scss']
})
export class UpdateWarehouseLayoutDialogComponent extends BaseComponent {

  pageModes = PageModes;
  WarehouseLayout!: WarehouseLayout;

  _WarhousteCategoryItemsEdit: _WarhousteCategoryItemsEdit[] = [];


  public CommodityCategories: CommodityCategory[] = [];
  public filterCommodityCategories: CommodityCategory[] = [];

  public CommodityCategoryPropertes: CommodityCategoryProperty[] = [];
  public filterCommodityCategoryProperty: CommodityCategoryProperty[] = [];

  public CommodityCategoryPropertyItems: CommodityCategoryPropertyItem[] = [];
  public filterCommodityCategoryPropertyItems: CommodityCategoryPropertyItem[] = [];


  public warehouseId: number = 0
  public Row_i?: number = 0;
  public Row_j?: number = 0;
  public Row_c?: number = 0;
  public IsFormValid:boolean=true;
  public lablelTitleCombo:string ="انتخاب گروه محصول"
  public spinner:boolean=false;
  constructor(
    private _mediator: Mediator,
    private dialogRef: MatDialogRef<UpdateWarehouseLayoutDialogComponent>,
    @Inject(MAT_DIALOG_DATA) data: any,
    private fb: FormBuilder,
    public _notificationService: NotificationService
  ) {
    super();

    this.WarehouseLayout = data.WarehousLayout;
    this.pageMode = data.pageMode;
    this.request = new UpdateWarehouseLayoutCommand();
    this.WarehouseLayout = data.WarehouseLayout;
    this._WarhousteCategoryItemsEdit = data.WarehouseLayout.items;



    this.pageMode = data.pageMode;

    this.form = this.fb.group({
      id: this.WarehouseLayout.id,
      warehouseId: this.WarehouseLayout.warehouseId,
      parentId: this.WarehouseLayout.parentId,
      title: this.WarehouseLayout.title,
      entryMode: this.WarehouseLayout.entryMode,
      lastLevel: this.WarehouseLayout.lastLevel,
      isDefault: this.WarehouseLayout.isDefault,
      capacity: this.WarehouseLayout.capacity,
      status: this.WarehouseLayout.status,
      orderIndex: this.WarehouseLayout.orderIndex,
      Items: this.fb.array([])
    })

  }
  get Items(): FormArray {
    return this.form.get("Items") as FormArray;
  }
  ItemsForm(): FormGroup {
    return this.fb.group({
    commodityCategoryId: [''],
     categoryPropertyId:  [''],
     categoryPropertyItemId:  [''],
     ValueItem:  [''],
     warehouseLayoutCategoriesId:  [''],
     warehouseLayoutPropertiesId: [''],

    });
  }
  async ngOnInit() {
    await this.resolve();
  }

  async resolve() {

    await this._mediator.send(new GetInverntoryCommodityCategoriesQuery(this.WarehouseLayout.parentId,this.WarehouseLayout.warehouseId)).then(res => {
      this.CommodityCategories = res;
      this.filterCommodityCategories = this.CommodityCategories;

    })
    await this._mediator.send(new GetInverntoryCommodityCategoriesPropertyQuery(this.WarehouseLayout.id)).then(res => {
      this.CommodityCategoryPropertes = res;
      this.filterCommodityCategoryProperty = this.CommodityCategoryPropertes;

    })
    await this._mediator.send(new GetInverntoryCommodityCategoriesPropertyItemsQuery(this.WarehouseLayout.id)).then(res => {
      this.CommodityCategoryPropertyItems = res;
      this.filterCommodityCategoryPropertyItems = this.CommodityCategoryPropertyItems;


    })

    await this.initialize()
  }

  initialize() {

    if (this.pageMode === PageModes.Update) {

      this.request = new UpdateWarehouseLayoutCommand().mapFrom(this.WarehouseLayout)
      if(this._WarhousteCategoryItemsEdit.length==0)
      {
        this.addItems();
      }

      this.form.controls['status'].setValue(this.WarehouseLayout.status);
      this.form.controls['entryMode'].setValue(this.WarehouseLayout.entryMode);
    }

  }
  //------------Items-------------------------------------------------
  //--------Remove---------------------------------------------------
  async removeCategory(i: number,callFromChangeLastCode:boolean=false){

    var Id=this._WarhousteCategoryItemsEdit[i].warehouseLayoutCategoriesId;
    if(Id!=undefined)
    await this._mediator.send(new DeleteWarehouseLayoutCategoryCommand(Number(Id))).then(res => {

      if(callFromChangeLastCode)
      {
        this.update();
      }
      this._WarhousteCategoryItemsEdit.splice(i,1);

    });
    else
    {
      this._WarhousteCategoryItemsEdit.splice(i,1)
    }

 }
  async  removeProperty(i: number,j:number){

    var warehouseLayoutPropertiesId=this._WarhousteCategoryItemsEdit[i]?.items[j].warehouseLayoutPropertiesId;
    var categoryPropertyId=this._WarhousteCategoryItemsEdit[i]?.items[j].categoryPropertyId;

    if(warehouseLayoutPropertiesId!=undefined)
    await this._mediator.send(new DeleteWarehouseLayoutPropertiesCommand(Number(categoryPropertyId),Number(warehouseLayoutPropertiesId))).then(res => {

      this._WarhousteCategoryItemsEdit[i]?.items?.splice(j,1);

    });
    else{
      this._WarhousteCategoryItemsEdit[i]?.items?.splice(j,1);
    }


  }
  removePropertyItems(i: number,j:number,c:number){


    this._WarhousteCategoryItemsEdit[i]?.items[j]?.items.splice(c,1)
 }
//--------------------Add---------------------------------
 addItems() {

  var item :_WarhousteCategoryItemsEdit={
   commodityCategoryId: undefined,
   categoryPropertyId: undefined,
   categoryPropertyItemId: undefined,
   ValueItem: undefined,
   disable:false,
   valid:true,
   warehouseLayoutCategoriesId: undefined,
   warehouseLayoutPropertiesId: undefined,
   items:[]
  }
   this._WarhousteCategoryItemsEdit.push(item)
 }

  addProperty(commodityCategoryId:any,warehouseLayoutCategoriesId:number,warehouseLayoutPropertiesId:number,i:number)
  {

    var item :_WarhousteCategoryItemsEdit={

    commodityCategoryId!:commodityCategoryId,
    categoryPropertyId: undefined,
    categoryPropertyItemId: undefined,
    ValueItem: undefined,
    disable:false,
    valid:true,
    warehouseLayoutCategoriesId: warehouseLayoutCategoriesId,
    warehouseLayoutPropertiesId: warehouseLayoutPropertiesId,
    items:[]
   }
   {
    this._WarhousteCategoryItemsEdit[i]?.items?.push(item);
   }

  }
  
  addPropertyItems(categoryPropertyId:number,commodityCategoryId:number,warehouseLayoutCategoriesId:number,i: number,j:number)
  {

    var item :_WarhousteCategoryItemsEdit={

      commodityCategoryId!:commodityCategoryId,
      categoryPropertyId!:categoryPropertyId,
      categoryPropertyItemId: undefined,
      ValueItem: undefined,
      disable:false,
      valid:true,
      warehouseLayoutCategoriesId: warehouseLayoutCategoriesId,
      warehouseLayoutPropertiesId: undefined,
      items:[]
     }
     this._WarhousteCategoryItemsEdit[i]?.items[j]?.items.push(item);

  }



  //------------------فیلتر کردن گروه محصول ها و بدست آوردن خصوصیات محصول-------------------------------------


  getCommodityCategoryTitleById(id: number,i:number) {

    this.Row_i=i;
    let searchQueries = [
      new SearchQuery({
        propertyName: 'categoryId',
        comparison: 'equal',
        values: [id],
      })
    ]
    this._WarhousteCategoryItemsEdit[Number(this.Row_i)].commodityCategoryId=id;
     //-------------------------------Id
    if (id != null && id != undefined && id!=0) {
      this._mediator.send(new GetCommodityCategoryPropertiesQuery(0, 0, searchQueries, "id ASC")).then(res => {
        this.CommodityCategoryPropertes = res.data;
        this.filterCommodityCategoryProperty = this.CommodityCategoryPropertes;
        this.CommodityCategories = [...this.filterCommodityCategories]

        if (res.data.length == 0) {

          this._WarhousteCategoryItemsEdit[Number(this.Row_i)].disable=true;
          if(Number(this.Row_j)>0)
          {
            this._WarhousteCategoryItemsEdit[Number(this.Row_i)]?.items.splice(Number(this.Row_j),1)
          }
        }
        else {
          this._WarhousteCategoryItemsEdit[Number(this.Row_i)].disable=false;

        }
      })
    }

    var list = this.filterCommodityCategories.find(x => x.id === id)?.title ?? '';

    return list;
  }
  //---------------------------فیلتر کردن گروه خصوصیات محصول ها و بدست آوردن ویژگی محصول----------------------

  //-------------------------------Id
  getCommodityCategoryPropertesTitleById(id: number,warehouseLayoutCategoriesId:number,warehouseLayoutPropertiesId:number,i: number,j:number) {

    this.Row_i=i;
    this.Row_j=j;
    if (id != null && id != undefined && id != 0) {

      var searchQuery = [
        new SearchQuery({
          propertyName: 'categoryPropertyId',
          comparison: 'equal',
          values: [id],
        })
      ]
      this._WarhousteCategoryItemsEdit[Number(this.Row_i)].items[Number(this.Row_j)].categoryPropertyId=id;

      this._mediator.send(new GetCommodityCategoryPropertyItemsQuery(0, 0, searchQuery, "id ASC")).then(res => {
        this.CommodityCategoryPropertyItems = res.data;
        this.filterCommodityCategoryPropertyItems = this.CommodityCategoryPropertyItems;
        this.CommodityCategoryPropertes = [...this.filterCommodityCategoryProperty];
        if (res.data.length == 0) {

          this._WarhousteCategoryItemsEdit[Number(this.Row_i)]?.items[Number(this.Row_j)].items.forEach(a=>a.disable=true);
        }
        else {
          this._WarhousteCategoryItemsEdit[Number(this.Row_i)]?.items[Number(this.Row_j)].items.forEach(a=>a.disable=false);
          var commodityCategoryId=  this._WarhousteCategoryItemsEdit[Number(this.Row_i)]?.commodityCategoryId;
          this.addPropertyItems(id,Number(commodityCategoryId),warehouseLayoutCategoriesId,Number(this.Row_i),Number(this.Row_j))

        }
      })
    }

    return this.CommodityCategoryPropertes.find(x => x.id === id)?.title ?? ''
  }
  //---------------------------فیلتر کردن  ویژگی محصول--------------------------------------------------------

   //-------------------------------Id
  getCommodityCategoryPropertyItemTitleById(id: number,i: number,j:number,c:number) {

    this.Row_i=i;
    this.Row_j=j;
    this.Row_c=c;
    this._WarhousteCategoryItemsEdit[Number(this.Row_i)].items[Number(this.Row_j)].items[Number(this.Row_c)].categoryPropertyItemId=id;

    var list = this.CommodityCategoryPropertyItems.find(x => x.id === id)?.title ?? '';
    return list
  }
  //-----------------------------------------------------------------------------------------------------------
 async update(entity?: any) {
   this.IsFormValid = true;

    this.Items.reset();
    this._WarhousteCategoryItemsEdit.forEach(element => {//---گروه

      if(element.items.length>0)
      {
        element.items.forEach(a=>{//----------خصوصیات

          if(a.items.length>0)
          {
            a.items.forEach(b=>{//--------------وِیژگی ها
              if((b.ValueItem==undefined && b.ValueItem=="") || b.categoryPropertyItemId==undefined)
              {
                this.IsFormValid=false;
                b.valid=false;

              }
              this.Items.push(this.fb.group({
                commodityCategoryId: element.commodityCategoryId,
                categoryPropertyId: b.categoryPropertyId,
                categoryPropertyItemId: b.categoryPropertyItemId,
                ValueItem: b.ValueItem,
                warehouseLayoutCategoriesId: b.warehouseLayoutCategoriesId,
                warehouseLayoutPropertiesId:b.warehouseLayoutPropertiesId,

              }));
          })
          }
          else{
            if(a.categoryPropertyId)
              {
                this.IsFormValid=false;
                a.valid=false;

              }
            this.Items.push(this.fb.group({
              commodityCategoryId: element.commodityCategoryId,
              categoryPropertyId: a.categoryPropertyId,
              categoryPropertyItemId: a.categoryPropertyItemId,
              ValueItem: a.ValueItem,
              warehouseLayoutCategoriesId: a.warehouseLayoutCategoriesId,
              warehouseLayoutPropertiesId:a.warehouseLayoutPropertiesId,

            }));

          }
    })
      }
      else{
        if(element.commodityCategoryId==undefined)
              {
                this.IsFormValid=false;
                element.valid=false;

              }
        this.Items.push(this.fb.group({
          commodityCategoryId: element.commodityCategoryId,
          categoryPropertyId: element.categoryPropertyId,
          categoryPropertyItemId: element.categoryPropertyItemId,
          ValueItem: element.ValueItem,
          warehouseLayoutCategoriesId: element.warehouseLayoutCategoriesId,
          warehouseLayoutPropertiesId:element.warehouseLayoutPropertiesId,

        }));

      }

    });

    if(this._WarhousteCategoryItemsEdit.length==0)
    {
      this.IsFormValid=false;

    }
    this.spinner=true;


      await this._mediator.send(<UpdateWarehouseLayoutCommand>this.request).then(res => {

        this.dialogRef.close({

          response: res,
          pageMode: this.pageMode
        })
        this.spinner=false;
      });


  }
//--------------------------------------------------------------------


async delete() {

  await this._mediator.send(new DeleteWarehouseLayoutCommand(Number(this.WarehouseLayout.id))).then(res => {

    this.dialogRef.close({
      response: res,
      pageMode: PageModes.Delete
    })

  });
}
//----------------------------------تغییر حالت آخرین فرزند باشد یا نباشد-------------------
  onRemoveItemlastLevel()
  {

    if(!this.form.controls.lastLevel.value)
    {
      if(this._WarhousteCategoryItemsEdit.length>0){
        var i:number=0;
        for(var item of this._WarhousteCategoryItemsEdit)
        {
          if(i!=0)
          {
            this.removeCategory(i)

          }
          i=i+1;
        }
      }

    }

  }
  add(){

  }

  get(id?: number) {

    throw new Error('Method not implemented.');
  }

  close() {
    throw new Error('Method not implemented.');
  }

}

