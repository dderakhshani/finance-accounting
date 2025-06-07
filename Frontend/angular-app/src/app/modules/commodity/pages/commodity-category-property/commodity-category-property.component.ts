import { Component } from '@angular/core';
import { BaseComponent } from "../../../../core/abstraction/base.component";
import { FormControl } from "@angular/forms";
import { CommodityCategory } from "../../entities/commodity-category";
import { Mediator } from "../../../../core/services/mediator/mediator.service";
import {
  GetCommodityCategoriesQuery
} from "../../repositories/commodity-category/queries/get-commodity-categories-query";
import {
  TableConfigurations
} from "../../../../core/components/custom/table/models/table-configurations";
import { CommodityCategoryProperty } from "../../entities/commodity-category-property";
import { SearchQuery } from "../../../../shared/services/search/models/search-query";
import {
  GetCommodityCategoryPropertiesQuery
} from "../../repositories/commodity-category-property/queries/get-commodity-category-properties-query";
import { Observable } from "rxjs";
import { map, startWith } from "rxjs/operators";
import { PreDefinedActions } from "../../../../core/components/custom/action-bar/action-bar.component";
import { MatDialog, MatDialogConfig } from "@angular/material/dialog";
import { PageModes } from "../../../../core/enums/page-modes";
import {
  CommodityCategoryPropertyDialogComponent
} from "./commodity-category-property-dialog/commodity-category-property-dialog.component";
import { TableColumnDataType } from "../../../../core/components/custom/table/models/table-column-data-type";
import { TableOptions } from "../../../../core/components/custom/table/models/table-options";
import { TableColumn } from "../../../../core/components/custom/table/models/table-column";
import { NotificationService } from '../../../../shared/services/notification/notification.service';

@Component({
  selector: 'app-commodity-category-property',
  templateUrl: './commodity-category-property.component.html',
  styleUrls: ['./commodity-category-property.component.scss']
})
export class CommodityCategoryPropertyComponent extends BaseComponent {
  commodityCategories: CommodityCategory[] = [];
  filteredCommodityCategories!: Observable<CommodityCategory[]>;

  commodityCategoryChips: CommodityCategory[] = []

  tableConfigurations!: TableConfigurations;
  inheritedCategoryPropertiesTableConfigurations!: TableConfigurations;


  commodityCategoryProperties: CommodityCategoryProperty[] = []
  inheritedCommodityCategoryProperties: CommodityCategoryProperty[] = []

 
  canAddCategoryProperty: boolean = false;

  constructor(
    private mediator: Mediator,
    public dialog: MatDialog,
    public _notificationService: NotificationService
  ) {
    super();
    this.form = new FormControl('')
  }

  ngAfterViewInit() {
    this.actionBar.actions = [
      PreDefinedActions.add().setTitle('افزودن خصوصیات'),

    ]
  }

  async ngOnInit() {
    await this.resolve()
  }

  async resolve(params?: any) {

    let columns: TableColumn[] = [
      new TableColumn(
        'select',
        '',
        TableColumnDataType.Select,
      ),

      new TableColumn(
        'orderIndex',
        'ترتیب نمایش',
        TableColumnDataType.Number,
      ),
      new TableColumn(
        'title',
        'نام خصوصیات',
        TableColumnDataType.Text,
      ),
      new TableColumn(
        'uniqueName',
        'نام یکتا',
        TableColumnDataType.Text,
      ),
      new TableColumn(
        'measureTitle',
        'واحد اندازه گیری',
        TableColumnDataType.Text,
      ),
      new TableColumn(
        'propertyTypeBaseTitle',
        'نوع داده',
        TableColumnDataType.Text,
      ),
    ];
    this.tableConfigurations = new TableConfigurations(columns, new TableOptions(false, true))
    this.inheritedCategoryPropertiesTableConfigurations = new TableConfigurations(columns, new TableOptions(false, true))

    await this.mediator.send(new GetCommodityCategoriesQuery()).then(res => {
      this.commodityCategories = res
      this.filteredCommodityCategories = this.form.valueChanges.pipe(
        startWith(''),
        map(commodityCategory => commodityCategory ? this.commodityCategories.filter(x => x.title.toLowerCase().includes((<string>commodityCategory).toLowerCase())) : this.commodityCategories)
      )
    })
  }

  initialize(params?: any): any {
  }

  async get(ids?: number[]) {
    let searchQueries: SearchQuery[] = [
      ids ? new SearchQuery({
        propertyName: 'categoryId',
        comparison: 'in',
        values: ids
      }) :
        new SearchQuery({
          propertyName: 'categoryId',
          comparison: 'equal',
          values: [this.form?.value]
        })
    ]
    
    return await this.mediator.send(new GetCommodityCategoryPropertiesQuery(this.tableConfigurations.pagination.pageIndex, this.tableConfigurations.pagination.pageSize, searchQueries))


  }


  commodityCategoryDisplayFn(commodityCategoryId: number) {
    return this.commodityCategories.find(x => x.id === commodityCategoryId)?.title ?? ''
  }

  async commodityCategorySelectionHandler(selectedCategoryId: number) {
    this.canAddCategoryProperty = false;
    this.commodityCategoryProperties = [];
    this.inheritedCommodityCategoryProperties = [];
    this.commodityCategoryChips = [];
    if (selectedCategoryId) {
      this.form.setValue(selectedCategoryId)

      this.updateCommodityCategoryChips(selectedCategoryId)
      await this.get().then(res => {
        this.commodityCategoryProperties = res.data
        this.tableConfigurations.pagination.totalItems = res.totalCount ?? this.tableConfigurations.pagination.totalItems
      });

      this.getData(selectedCategoryId);

      this.canAddCategoryProperty = true;
    }
  }
  async getData(selectedCategoryId: number) {
    let parentIds = this.commodityCategoryChips.filter(x => x.id !== selectedCategoryId).map(x => x.id);
    
    if (parentIds.length > 0) {
      
      await this.get(parentIds).then(res => {

        
        this.inheritedCommodityCategoryProperties = res.data
        this.inheritedCategoryPropertiesTableConfigurations.pagination.totalItems = res.totalCount ?? this.tableConfigurations.pagination.totalItems

      }
      );
    } else {
      this.inheritedCommodityCategoryProperties = []
    }
  }
  updateCommodityCategoryChips(selectedCategoryId: number) {
    this.commodityCategoryChips = [];
    let categoryToPush = this.commodityCategories.find(x => x.id === selectedCategoryId);

    while (categoryToPush) {
      if (categoryToPush) {
        this.commodityCategoryChips.unshift(categoryToPush)
      }
      categoryToPush = this.commodityCategories.find(x => x.id === categoryToPush?.parentId)
    }
  }

  getInheritedPropertiesByCategoryId(categoryId: number) {
    return this.inheritedCommodityCategoryProperties.filter(x => x.categoryId === categoryId);
  }

  add(param?: any): any {
    let dialogConfig = new MatDialogConfig();
    dialogConfig.data = {
      category: this.commodityCategories.find(x => x.id === this.form.value),
      pageMode: PageModes.Add
    };

    let dialogReference = this.dialog.open(CommodityCategoryPropertyDialogComponent, dialogConfig);

    dialogReference.afterClosed().subscribe(({ response, pageMode }) => {
      if (response && pageMode === PageModes.Add) {
        this.commodityCategoryProperties.push(response)
        this.commodityCategoryProperties = [...this.commodityCategoryProperties]
        this.get().then(res => {
          this.commodityCategoryProperties = res.data
          this.tableConfigurations.pagination.totalItems = res.totalCount ?? this.tableConfigurations.pagination.totalItems
        });
      }
    })
  }

  update(commodityCategory: CommodityCategory): any {
    let dialogConfig = new MatDialogConfig();
    dialogConfig.data = {
      category: this.commodityCategories.find(x => x.id === this.form.value),
      categoryProperty: commodityCategory,
      pageMode: PageModes.Update
    };

    let dialogReference = this.dialog.open(CommodityCategoryPropertyDialogComponent, dialogConfig);

    dialogReference.afterClosed().subscribe(({ response, pageMode }) => {
      if (response) {
        if (pageMode === PageModes.Update) {
          let categoryPropertyToUpdate = this.commodityCategoryProperties.find(x => x.id === response.id)
          if (categoryPropertyToUpdate) {
            Object.keys(response).forEach((key: string) => {
              // @ts-ignore
              categoryPropertyToUpdate[key] = response[key]
            })
          }
        } else if (pageMode === PageModes.Delete) {
          let categoryPropertyToRemove = this.commodityCategoryProperties.find(x => x.id === response.id)
          if (categoryPropertyToRemove) {
            this.commodityCategoryProperties.splice(this.commodityCategoryProperties.indexOf(categoryPropertyToRemove), 1)
            this.commodityCategoryProperties = [...this.commodityCategoryProperties]
          }
        }
      }
      this.get().then(res => {
        this.commodityCategoryProperties = res.data
        this.tableConfigurations.pagination.totalItems = res.totalCount ?? this.tableConfigurations.pagination.totalItems
      });
    })
  }


  close(): any {
  }

  delete(param?: any): any {
  }


}
