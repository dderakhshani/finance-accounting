import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {CommodityRoutingModule} from './commodity-routing.module';
import {ComponentsModule} from "../../core/components/components.module";
import {FormsModule, ReactiveFormsModule} from "@angular/forms";
import {AngularMaterialModule} from "../../core/components/material-design/angular-material.module";
import {MatTreeSelectInputModule} from "mat-tree-select-input";
import {SharedModule} from "../../shared/shared.module";
import {MatIconModule} from "@angular/material/icon";
import {CommodityCategoriesComponent} from './pages/commodity-categories/commodity-categories.component';
import {
  CommodityCategoryDialogComponent
} from './pages/commodity-categories/commodity-category-dialog/commodity-category-dialog.component';
import {
  CommodityCategoryPropertyComponent
} from './pages/commodity-category-property/commodity-category-property.component';
import {
  CommodityCategoryPropertyDialogComponent
} from './pages/commodity-category-property/commodity-category-property-dialog/commodity-category-property-dialog.component';
import { CommodityCategoryPropertyItemComponent } from './pages/commodity-category-property/commodity-category-property-dialog/commodity-category-property-item/commodity-category-property-item.component';
import { AddCommodityComponent } from './pages/commodity/add-commodity/add-commodity.component';
import { CommodityListComponent } from './pages/commodity/commodity-list/commodity-list.component';
import {InventoryModule} from "../inventory/inventory.module";

import { CommodityPropertiesComponent } from './pages/commodity/add-commodity/commodity-properties/commodity-properties.component';
import { BomComponent } from './pages/bom/bom.component';
import { BomDialogComponent } from './pages/bom/bom-dialog/bom-dialog.component';
import { BomItemsComponent } from './pages/bom/bom-dialog/bom-items/bom-items.component';
import { BomHeadersComponent } from './pages/bom-headers/bom-headers.component';
import { BomHeadersDialogComponent } from './pages/bom-headers/bom-headers-dialog/bom-headers-dialog.component';
import { BomHeadersItemsComponent } from './pages/bom-headers/bom-headers-dialog/bom-headers-items/bom-headers-items.component';
import { WarehouseLayoutsCommodityQuantityComponent } from './pages/warehouse-layouts-commodity-quantity/warehouse-layouts-commodity-quantity.component';
import { WarehouseStocksCommodityComponent } from './pages/warehouse-stocks-commodity/warehouse-stocks-commoditycomponent';
import { CommodityTableComponent } from './pages/commodity-common/commodity-table/commodity-table.component';


@NgModule({
  declarations: [

    CommodityCategoriesComponent,
    CommodityCategoryDialogComponent,
    CommodityCategoryPropertyComponent,
    CommodityCategoryPropertyDialogComponent,
    CommodityCategoryPropertyItemComponent,
    AddCommodityComponent,
    CommodityListComponent,

    CommodityPropertiesComponent,

    BomComponent,
    BomDialogComponent,
    BomItemsComponent,
    BomHeadersDialogComponent,
    BomHeadersItemsComponent,
    WarehouseLayoutsCommodityQuantityComponent,
    WarehouseStocksCommodityComponent,
    CommodityTableComponent,



  ],
  exports: [
    BomHeadersComponent
  ],
  imports: [
    CommonModule,
    CommodityRoutingModule,
    ReactiveFormsModule,
    AngularMaterialModule,
    ComponentsModule,
    MatTreeSelectInputModule,
    FormsModule,
    SharedModule,
    MatIconModule,
    InventoryModule,

  ]
})
export class CommodityModule {

}
