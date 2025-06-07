import { Component } from '@angular/core';
import { BaseComponent } from "../../../../../core/abstraction/base.component";
import {
  GetCommodityCategoriesQuery
} from "../../../repositories/commodity-category/queries/get-commodity-categories-query";
import { map, startWith } from "rxjs/operators";
import { CommodityCategory } from "../../../entities/commodity-category";
import { forkJoin } from "rxjs";
import { Mediator } from "../../../../../core/services/mediator/mediator.service";
import { FormArray, FormControl, FormGroup } from "@angular/forms";
import {
  GetCommodityCategoryPropertiesQuery
} from "../../../repositories/commodity-category-property/queries/get-commodity-category-properties-query";
import { SearchQuery } from "../../../../../shared/services/search/models/search-query";
import { CommodityCategoryProperty } from "../../../entities/commodity-category-property";
import { PageModes } from "../../../../../core/enums/page-modes";
import { Commodity } from "../../../entities/commodity";
import { UpdateCommodityCommand } from "../../../repositories/commodity/commands/update-commodity-command";
import { AddCommodityCommand } from "../../../repositories/commodity/commands/add-commodity-command";
import { BaseValue } from "../../../../admin/entities/base-value";
import { MeasureUnit } from "../../../entities/measure-unit";
import {
  GetBaseValuesByUniqueNameQuery
} from "../../../../admin/repositories/base-value/queries/get-base-values-by-unique-name-query";
import { GetMeasureUnitsQuery } from "../../../repositories/measure-units/queries/get-measure-units-query";
import { ActivatedRoute, Router } from "@angular/router";
import { PreDefinedActions } from "../../../../../core/components/custom/action-bar/action-bar.component";
import { GetCommodityQuery } from "../../../repositories/commodity/queries/get-commodity-query";
import {
  AddCommodityPropertyValueCommand
} from "../../../repositories/commodity-property-value/commands/add-commodity-property-value-command";
import {
  GetCommodityCategoryParentTreeQuery
} from "../../../repositories/commodity-category/queries/get-commodity-category-parent-tree-query";
import { GetCommoditiesQuery } from "../../../repositories/commodity/queries/get-commodities-query";
import { NotificationService } from '../../../../../shared/services/notification/notification.service';
import { P } from '@angular/cdk/keycodes';
import { PagesCommonService } from '../../../../../shared/services/pages/pages-common.service';
import { UpdateCommodityPropertyValueCommand } from '../../../repositories/commodity-property-value/commands/update-commodity-property-value-command';

@Component({
  selector: 'app-add-commodity',
  templateUrl: './add-commodity.component.html',
  styleUrls: ['./add-commodity.component.scss']
})
export class AddCommodityComponent extends BaseComponent {
  commodityCategories: CommodityCategory[] = [];
  filteredCommodityCategories: CommodityCategory[] = [];

  selectedCommodityCategories: CommodityCategory[] = []
  commodityCategoryProperties: CommodityCategoryProperty[] = []
  doesSelectedCategoryNeedsParentProduct: boolean = false;

  designCommodities: Commodity[] = [];
  filteredDesignCommodities: Commodity[] = [];

  pricingTypes: BaseValue[] = []
  measureUnits: MeasureUnit[] = []
  isAddMode: boolean = false;


  HistoryCommodityLayout = new FormGroup({
    commodityId: new FormControl(this.getQueryParam('id')),
    warehouseId: new FormControl(),
    documentNo: new FormControl(),

  });

  constructor(
    private mediator: Mediator,
    private router: Router,
    private route: ActivatedRoute,
    public Service: PagesCommonService,
    public _notificationService: NotificationService,
  ) {
    super(route, router);
    this.request = new AddCommodityCommand();

  }

  ngAfterViewInit() {
    this.actionBar.actions = [
      PreDefinedActions.save(),
      PreDefinedActions.add(),
      PreDefinedActions.list(),
    ]
    this.request = new AddCommodityCommand();
  }

  async ngOnInit() {
    await this.resolve()
  }

  async resolve(params?: any) {
    forkJoin([
      this.mediator.send(new GetBaseValuesByUniqueNameQuery('PricingTypeBaseId')),
      this.mediator.send(new GetMeasureUnitsQuery()),
      this.mediator.send(new GetCommodityCategoriesQuery())
    ]).subscribe(async ([
      pricingTypes,
      measureUnits,
      categories
    ]) => {

      this.pricingTypes = pricingTypes;
      this.measureUnits = measureUnits;
      this.commodityCategories = categories;
      this.commodityCategories.forEach(a => a.title = a.code + '🔅' + a.title);
      await this.initialize()
    })
  }

  async initialize(entity?: Commodity) {
    console.log(this.getQueryParam('commodityCategoryId'))
    console.log(this.getQueryParam('pageMode'))
    if (entity || this.getQueryParam('id')) {

      entity = await this.get(this.getQueryParam('id'))

      if (this.getQueryParam('pageMode') == 'Copy') {

        this.pageMode = PageModes.Add;
        this.isAddMode = true;
        this.request = new AddCommodityCommand().mapFrom(<Commodity>entity)
      }
      else {
        this.pageMode = PageModes.Update;
        this.request = new UpdateCommodityCommand().mapFrom(<Commodity>entity)
      }
      await this.commodityCategorySelectionHandler(entity.commodityCategoryId)
    } else if(this.getQueryParam('pageMode') == 'Add'){

        this.pageMode = PageModes.Add
        this.isAddMode = true;
        await this.commodityCategorySelectionHandler(Number(this.getQueryParam('commodityCategoryId')))

    }
    else {
      this.pageMode = PageModes.Add
      this.isAddMode = true;
      this.request = new AddCommodityCommand();
    }


    this.form.controls['parentCode'].disable();

    this.form.controls['code'].valueChanges.subscribe((newValue: string) => {
      this.form.controls['tadbirCode'].setValue(newValue);
      this.form.controls['compactCode'].setValue(newValue);
      let compactCode = ''

      if (newValue.length == 24) {
        //S00411100000900000003537
        //S004-111-000-009-0000000-3537
        //compactCode=1110093537
        let right4 = newValue.substring(20, 24);
        let center3 = newValue.substring(10, 13);
        let left3 = newValue.substring(4, 7);
        compactCode = left3 + center3 + right4

        this.form.controls['compactCode'].setValue(compactCode);
      }

    })


    this.form.controls['parentId'].valueChanges.pipe(
      startWith(''),
      map(async (commodity) => {
        if (typeof commodity === "string")
          return this.filteredDesignCommodities = this.designCommodities.filter(x => x.title.includes(commodity));
        else
          return this.filteredDesignCommodities = this.designCommodities;
      })).subscribe()

  }


  async add(param?: any) {
    if (this.form.controls['measureId'].value == undefined) {
      this.Service.showHttpFailMessage('واحد کالا را انتخاب نمایید');
      return
    }
    if (this.form.controls['code'].value == undefined) {
      this.Service.showHttpFailMessage('کد کالا را وارد نمایید');
      return
    }
    if (!this.form.controls['title'].value) {
      this.Service.showHttpFailMessage('نام کالا را وارد نمایید');
      return
    }
    if (!this.form.controls['compactCode'].value) {

      this.form.controls['compactCode'].setValue(this.form.controls['code'].value)
    }
    let code: string = this.form.controls['code'].value

    if (code.length > 18 && code.length != 24) {
      this.Service.showHttpFailMessage('طول کد کالا اشتباه می باشد');
      return
    }

    await this.mediator.send(<AddCommodityCommand>this.request).then(async (res) => {

      var activeTab = this.Service.TabManagerService.activeTab;
      if (activeTab != undefined) {
        this.Service.TabManagerService.closeTab(activeTab)
      }
      this.navigateToCommoditiesList();

    })
  }

  async update(param?: any) {
    if (this.form.controls['measureId'].value == undefined) {
      this.Service.showHttpFailMessage('واحد کالا را انتخاب نمایید');
      return
    }
    if (this.form.controls['code'].value == undefined) {
      this.Service.showHttpFailMessage('کد کالا را وارد نمایید');
      return
    }
    if (!this.form.controls['title'].value) {
      this.Service.showHttpFailMessage('نام کالا را وارد نمایید');
      return
    }
    if (!this.form.controls['compactCode'].value) {

      this.form.controls['compactCode'].setValue(this.form.controls['code'].value)
    }
    let Newrequest = <UpdateCommodityCommand>this.request;



    await this.mediator.send(<UpdateCommodityCommand>this.request).then(async (res) => {
      await this.initialize(res);
    })
  }



  async get(id: number) {
    return await this.mediator.send(new GetCommodityQuery(id))
  }

  async commodityCategorySelectionHandler(selectedCategoryId: number) {

    this.selectedCommodityCategories = [];
    this.form.controls['commodityCategoryId'].setValue(selectedCategoryId)

    if (selectedCategoryId) {
      let categoryToPush = <CommodityCategory>this.commodityCategories.find(x => x.id === selectedCategoryId);
      this.form.controls['parentCode'].setValue(categoryToPush?.code)
      if (this.pageMode === PageModes.Add) {
        await this.mediator.send(new GetCommoditiesQuery(0, 0, 1, [new SearchQuery({
          comparison: 'equal',
          propertyName: 'commodityCategoryId',
          values: [selectedCategoryId]
        })], 'code DESC')).then(res => {
          if (res.data[0]) {
            let code = res.data[0].code;


            let commodityCode = code.substring(categoryToPush?.code.length)



            let parsedCode = parseInt(commodityCode)

            parsedCode++;


            let lentgh1 = parsedCode.toString().length
            let lentgh = commodityCode.length - lentgh1;

            let finalCode = categoryToPush.code + commodityCode.substring(0, lentgh) + parsedCode.toString()

            this.form.controls['code'].setValue(finalCode)
          } else {
            this.form.controls['code'].setValue(categoryToPush.code + "1")
          }
        })
      }
      this.doesSelectedCategoryNeedsParentProduct = categoryToPush?.requireParentProduct ?? false

      while (categoryToPush) {
        this.selectedCommodityCategories.unshift(categoryToPush)
        categoryToPush = <CommodityCategory>this.commodityCategories.find(x => x.id === categoryToPush.parentId);
      }


      await this.getCategoryProperties();

    }

  }


  async getCategoryProperties() {
    let searchQueries: SearchQuery[] = [];
    this.selectedCommodityCategories.map(x => x.id).forEach(id => {
      searchQueries.push(new SearchQuery({
        propertyName: 'categoryId',
        comparison: 'equal',
        values: [id]
      }))
    })

    await this.mediator.send(new GetCommodityCategoryPropertiesQuery(0, 0, searchQueries)).then(res => {

      var list: FormArray = <FormArray>this.form.controls['propertyValues'];


      if (this.pageMode === PageModes.Add) {

        this.clearFormArray(<FormArray>this.form.controls['propertyValues']);
        this.commodityCategoryProperties = res.data;

        this.commodityCategoryProperties.forEach(item => {
          var property = new AddCommodityPropertyValueCommand();
          property.categoryPropertyId = item.id;

          list.push(this.createForm(property));
        })
      }

      else {
        this.commodityCategoryProperties = res.data;
        this.commodityCategoryProperties.forEach(Properties => {

          let form = (<FormArray>this.form.controls['propertyValues']).controls.find(x => x.value.categoryPropertyId === Properties.id)

          //اگر وجود نداشت اضافه شود
          if (!form) {
            let property = new UpdateCommodityPropertyValueCommand();
            property.categoryPropertyId = Properties.id;
            property.commodityId = this.form.controls['id'].value;

            list.push(this.createForm(property));

          }
          //اگر وجود داشت بروز رسانی شود.
          else {

            list.controls.forEach((control: any) => {


              if ((control as FormGroup).controls.categoryPropertyId.value == Properties.id) {


                Properties.value = (control as FormGroup).controls['value'].value;
              }
            })
          }


        })
      }



    })
  }


  async navigateToCommoditiesList() {
    await this.router.navigateByUrl('/commodity/list')
  }

  async reset() {
    this.selectedCommodityCategories = []
    this.commodityCategoryProperties = []
    await this.deleteQueryParam('id')
    super.reset()
  }
  SelectedPropertyItem(item: CommodityCategoryProperty) {

    if (this.form.controls['propertyValues']) {

      var list: FormArray = <FormArray>this.form.controls['propertyValues'];

      list.controls.forEach((control: any) => {

        if ((control as FormGroup).controls.categoryPropertyId.value == item.id) {
          (control as FormGroup).controls['value'].setValue(item.value);
          (control as FormGroup).controls['valuePropertyItemId'].setValue(item.items.length > 0 ? Number(item.valuePropertyItemId) : undefined);
        }
      })
    }


  }
  close(): any {
  }

  delete(param?: any): any {
  }


}
