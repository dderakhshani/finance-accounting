import { Component, Inject, Injectable } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from "@angular/material/dialog";
import { BaseComponent } from "../../../../../core/abstraction/base.component";
import { PageModes } from "../../../../../core/enums/page-modes";
import { CreateWarehouseLayoutCommand } from "../../../repositories/warehouse-layout/commands/create-warehouse-layout-command";
import { UpdateWarehouseLayoutCommand } from "../../../repositories/warehouse-layout/commands/update-warehouse-layout-command";
import { Mediator } from "../../../../../core/services/mediator/mediator.service";
import { DeleteWarehouseLayoutCommand } from "../../../repositories/warehouse-layout/commands/delete-warehouse-layout-command";
import {WarehouseLayout, _WarhousteCategoryItems} from "../../../entities/warehouse-layout";
import {CommodityCategory} from "../../../../commodity/entities/commodity-category";
import {GetCommodityCategoriesQuery} from "../../../../commodity/repositories/commodity-category/queries/get-commodity-categories-query";
import { forkJoin } from "rxjs";
import { CommodityCategoryProperty } from '../../../../commodity/entities/commodity-category-property';
import { GetCommodityCategoryPropertiesQuery } from '../../../../commodity/repositories/commodity-category-property/queries/get-commodity-category-properties-query';
import { GetCommodityCategoryPropertyItemsQuery } from '../../../../commodity/repositories/commodity-category-property-item/queries/get-commodity-category-property-items-query';
import { CommodityCategoryPropertyItem } from '../../../../commodity/entities/commodity-category-property-item';
import { FormArray, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';

import { number } from 'echarts';
import {SearchQuery} from "../../../../../shared/services/search/models/search-query";
import { GetInverntoryCommodityCategoriesQuery } from '../../../repositories/commodity-categories/get-commodity-categories-query';
import { NotificationService } from '../../../../../shared/services/notification/notification.service';

@Component({
  selector: 'app-add-warehouse-layout-dialog',
  templateUrl: './add-warehouse-layout-dialog.component.html',
  styleUrls: ['./add-warehouse-layout-dialog.component.scss']
})

export class AddWarehouseLayoutDialogComponent extends BaseComponent {

  pageModes = PageModes;
  WarehouseLayout!: WarehouseLayout;
  _WarhousteCategoryItems: _WarhousteCategoryItems[] = [];


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
  public spinner=false;
  constructor(
    private _mediator: Mediator,
    private dialogRef: MatDialogRef<AddWarehouseLayoutDialogComponent>,
    @Inject(MAT_DIALOG_DATA) data: any,
    public _notificationService: NotificationService,

    private fb: FormBuilder
  ) {
    super();

    this.WarehouseLayout = data.WarehouseLayout;
    this.warehouseId = data.warehouseId;
    this.pageMode = data.pageMode;
    ;
    this.form = this.fb.group({
      id: '',
      warehouseId:[this.warehouseId, Validators.required],
      parentId:[this.WarehouseLayout.parentId, Validators.required],
      title: [null, Validators.required],
      childCount: [null, Validators.required],
      lastLevel: '',
      entryMode: [0, Validators.required],
      capacity: [null],
      status: [0],
      Items: this.fb.array([])
    })

  }
  async ngOnInit() {
    await this.resolve();
  }

  async resolve() {


    await this._mediator.send(new GetInverntoryCommodityCategoriesQuery(this.WarehouseLayout.parentId,this.warehouseId)).then(res => {
      this.CommodityCategories = res;
      this.filterCommodityCategories = this.CommodityCategories;

    })

    await this.initialize()
  }

  initialize() {
    if (this.pageMode === PageModes.Add) {
      let newRequest = new CreateWarehouseLayoutCommand()
      if (this.WarehouseLayout) {
        newRequest.parentId = this.WarehouseLayout.parentId;

      }
      newRequest.warehouseId = this.warehouseId;
      newRequest.entryMode=0;
      newRequest.state=0;
      this.request = newRequest;
      this.form.controls['entryMode'].setValue(0);
      this.form.controls['status'].setValue(0);

    }
    this.addItems();

  }
  //------------Items-------------------------------------------------
  get Items(): FormArray {
    return this.form.get("Items") as FormArray;
  }

  ItemsForm(): FormGroup {
    return this.fb.group({
      commodityCategoryId: [''],
      categoryPropertyId: [''],
      categoryPropertyItemId: [''],
      ValueItem: [''],

    });
  }

  addItems() {


   var item :_WarhousteCategoryItems={
    commodityCategoryId: undefined,
    categoryPropertyId: undefined,
    categoryPropertyItemId: undefined,
    ValueItem: undefined,
    disable:false,
    valid:true,

    Items:[]
   }
    this._WarhousteCategoryItems.push(item)


  }

  removeItems(i: number) {
    this.Items.removeAt(i);
  }
  removeProperty(i: number,j:number){

     this._WarhousteCategoryItems[i]?.Items?.splice(j,1)
  }
  removePropertyItems(i: number,j:number,c:number){
    this._WarhousteCategoryItems[i]?.Items[j]?.Items.splice(c,1)
 }
  removeCategory(i: number){

    this._WarhousteCategoryItems.splice(i,1)
 }
  addProperty(commodityCategoryId:any,i:number)
  {
    var item :_WarhousteCategoryItems={
    commodityCategoryId!:commodityCategoryId,
    categoryPropertyId: undefined,
    categoryPropertyItemId: undefined,
    ValueItem: undefined,
    disable:false,
    valid:true,
    Items:[]
   }
   {
    this._WarhousteCategoryItems[i]?.Items?.push(item);
   }

  }

  addPropertyItems(categoryPropertyId:number,commodityCategoryId:number,i: number,j:number)
  {

    var item :_WarhousteCategoryItems={

      commodityCategoryId!:commodityCategoryId,
      categoryPropertyId!:categoryPropertyId,
      categoryPropertyItemId: undefined,
      ValueItem: undefined,
      disable:false,
      valid:true,
      Items:[]
     }
     this._WarhousteCategoryItems[i]?.Items[j]?.Items.push(item);

  }


  //--------------------------------------------------------------------

  async add() {
    this.IsFormValid=true;

    this.Items.reset();
    this._WarhousteCategoryItems.forEach(element => {//---گروه
      if(element.Items.length>0)
      {
        element.Items.forEach(a=>{//----------خصوصیات

          if(a.Items.length>0)
          {
            a.Items.forEach(b=>{//--------------وِیژگی ها
              if((b.ValueItem==undefined && b.ValueItem=="") || b.categoryPropertyItemId==undefined)
              {
                this.IsFormValid=false;
                b.valid=false;

              }

              this.Items.push(this.fb.group({
                commodityCategoryId: b.commodityCategoryId,
                categoryPropertyId: b.categoryPropertyId,
                categoryPropertyItemId: b.categoryPropertyItemId,
                ValueItem: b.ValueItem,

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
              commodityCategoryId: a.commodityCategoryId,
              categoryPropertyId: a.categoryPropertyId,
              categoryPropertyItemId: a.categoryPropertyItemId,
              ValueItem: a.ValueItem,

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

        }));

      }

    });

    if(this._WarhousteCategoryItems.length==0)
    {
      this.IsFormValid=false;

    }

    //if(this.IsFormValid)
    //{
      this.spinner=true;
      await this._mediator.send(<CreateWarehouseLayoutCommand>this.request).then(res => {

        this.dialogRef.close({

          response: res,
          pageMode: this.pageMode
        })
        this.spinner=false;
      });
    //}

  }
  //--------------------------------------------------------------------------------------------------------------
  //------------------فیلتر کردن گروه محصول ها و بدست آوردن خصوصیات محصول-------------------------------------

  getCommodityCategoryTitleById(id: number,i:number) {

    this.Row_i=i;
    var searchQueries = [
      new SearchQuery({
        propertyName: 'categoryId',
        comparison: 'equal',
        values: [id],
      })
    ]
    this._WarhousteCategoryItems[Number(this.Row_i)].commodityCategoryId=id;
     //-------------------------------Id
    if (id != null && id != undefined && id!=0) {
      this._mediator.send(new GetCommodityCategoryPropertiesQuery(0, 0, searchQueries, "id ASC")).then(res => {
        this.CommodityCategoryPropertes = res.data;
        this.filterCommodityCategoryProperty = this.CommodityCategoryPropertes;

        //this.CommodityCategoryPropertes = [...this.filterCommodityCategoryProperty]

        if (res.data.length == 0) {

          this._WarhousteCategoryItems[Number(this.Row_i)].disable=true;
          this._WarhousteCategoryItems[Number(this.Row_i)]?.Items.splice(Number(this.Row_j),1)

        }
        else {

          this._WarhousteCategoryItems[Number(this.Row_i)].disable=false;

        }
      })
    }

    var list = this.filterCommodityCategories.find(x => x.id === id)?.title ?? '';

    return list;
  }
  //--------------------------------------------------------------------------------------------------------------
  //---------------------------فیلتر کردن گروه خصوصیات محصول ها و بدست آوردن ویژگی محصول----------------------

  //-------------------------------Id
  getCommodityCategoryPropertesTitleById(id: number,i: number,j:number) {

    this.Row_i=i;
    this.Row_j=j;
    this._WarhousteCategoryItems[Number(this.Row_i)].Items[Number(this.Row_j)].categoryPropertyId=id;

    if (id != null && id != undefined && id != 0) {

      let searchQueries = [
        new SearchQuery({
          propertyName: 'categoryPropertyId',
          comparison: 'equal',
          values: [id],
        })
      ]

      this._mediator.send(new GetCommodityCategoryPropertyItemsQuery(0, 0, searchQueries, "id ASC")).then(res => {
        this.CommodityCategoryPropertyItems = res.data;
        this.filterCommodityCategoryPropertyItems = this.CommodityCategoryPropertyItems;
        //this.CommodityCategoryPropertes = [...this.filterCommodityCategoryProperty];
        if (res.data.length == 0) {

          this._WarhousteCategoryItems[Number(this.Row_i)]?.Items[Number(this.Row_j)].Items.forEach(a=>a.disable=true);
        }
        else {
          this._WarhousteCategoryItems[Number(this.Row_i)]?.Items[Number(this.Row_j)].Items.forEach(a=>a.disable=false);
          var commodityCategoryId=  this._WarhousteCategoryItems[Number(this.Row_i)]?.commodityCategoryId;
          this.addPropertyItems(id,Number(commodityCategoryId),Number(this.Row_i),Number(this.Row_j))

        }
      })
    }

    return this.CommodityCategoryPropertes.find(x => x.id === id)?.title ?? ''
  }
  //------------------------------------------------------------------------------------------------------------
  //---------------------------فیلتر کردن  ویژگی محصول--------------------------------------------------------

   //-------------------------------Id
  getCommodityCategoryPropertyItemTitleById(id: number,i: number,j:number,c:number) {

    this.Row_i=i;
    this.Row_j=j;
    this.Row_c=c;
    this._WarhousteCategoryItems[Number(this.Row_i)].Items[Number(this.Row_j)].Items[Number(this.Row_c)].categoryPropertyItemId=id;

    var list = this.CommodityCategoryPropertyItems.find(x => x.id === id)?.title ?? '';
    return list
  }
  //-----------------------------------------------------------------------------------------------------------
  onRemoveItemlastLevel()
  {
    if(!this.form.controls.lastLevel.value)
    {
      this._WarhousteCategoryItems=[];
      this.addItems();
    }

  }
  get(id?: number) {
    throw new Error('Method not implemented.');
  }

  close() {
    throw new Error('Method not implemented.');
  }
  async update(entity?: any) {
    throw new Error('Method not implemented.');
  }

  async delete() {
    throw new Error('Method not implemented.');
  }

}

