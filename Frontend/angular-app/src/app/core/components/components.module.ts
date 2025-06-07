import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {FormsModule, ReactiveFormsModule} from "@angular/forms";
import {TreeComponent} from './custom/tree/tree.component';
import {TreeItemComponent} from './custom/tree/tree-item/tree-item.component';
import {AngularMaterialModule} from "./material-design/angular-material.module";
import {HttpSnackbarComponent} from './material-design/http-snackbar/http-snackbar.component';
import {TreeBranchComponent} from './custom/tree/tree-branch/tree-branch.component';
import {TreeBranchItemComponent} from './custom/tree/tree-branch/tree-branch-item/tree-branch-item.component';
import {FormActionsComponent} from './custom/form-actions/form-actions.component';
import {TableComponent} from './custom/table/table.component';
import {ToPersianDatePipe} from "../pipes/to-persian-date.pipe";
import {SharedModule} from "../../shared/shared.module";
import {MoneyPipe} from "../pipes/money.pipe";
import {AccountingMoneyPipe} from "../pipes/accounting-money.pipe";
import {EChartsModule} from "./custom/echarts/echarts.module";
import {DragDropModule} from "@angular/cdk/drag-drop";
import {ActionBarComponent} from './custom/action-bar/action-bar.component';
import {UploaderComponent} from './custom/uploader/uploader.component';
import {ComboSearchComponent} from '../../modules/inventory/pages/component/combo-search/combo-search.component';
import {ComboTreeComponent} from '../../modules/inventory/pages/component/combo-tree/combo-tree.component';
import {
  ComboAccountRefrenceComponent
} from '../../modules/inventory/pages/component/combo-account-refrence/combo-account-refrence.component';
import {ComboTagComponent} from '../../modules/inventory/pages/component/combo-tag/combo-tag.component';
import {
  ComboCommodityComponent
} from '../../modules/inventory/pages/component/combo-commodity/combo-commodity.component';
import {
  ComboWarhouseTreeComponent
} from '../../modules/inventory/pages/component/combo-warhouse-tree/combo-warhouse-tree.component';
import {
  CommodityCategoriesTreeComponent
} from '../../modules/inventory/pages/component/combo-commodity-categories-tree/combo-commodity-categories-tree.component';

import {CURRENCY_MASK_CONFIG, CurrencyMaskConfig, CurrencyMaskModule} from "ng2-currency-mask";
import {ScrollingModule} from "@angular/cdk/scrolling";
import {BomHeadersComponent} from '../../modules/commodity/pages/bom-headers/bom-headers.component';
import {FirstKeyPipe} from "../pipes/firstKey.pipe";
import {TableFilterComponent} from '../../modules/inventory/pages/component/table-filter/table-filter.component';
import {tablePaggingComponent} from '../../modules/inventory/pages/component/table-pagging/table-pagging.component';

import {NgxMaskModule} from 'ngx-mask';
import {
  WarehouseLayoutsCommodityHistoryComponent
} from '../../modules/inventory/pages/reports/warehouse-layouts-commodity-history/warehouse-layouts-commodity-history.component';
import {ArrayFilterPipe} from "../pipes/arrayFilter.pipe";
import {ResizableComponent} from "./custom/table/table-details/components/resizable/resizable.component";
import {ResizableDirective} from "./custom/table/table-details/directive/resizable.directive";
import {TruncatePipe} from "./custom/table/table-details/pipe/truncate.pipe";
import {ResizeColumnDirective} from './custom/table/table-details/directive/resize-column.directive';
import {DynamicTableHeightDirective} from './custom/table/table-details/directive/dynamic-table-height.directive';
import {TableVirtualScrollingComponent} from './custom/table/table-virtual-scrolling/table-virtual-scrolling.component';
import {CustomDecimalPipe} from './custom/table/table-details/pipe/custom-decimal.pipe';
import {
  TableColumnFiltersComponent
} from './custom/table/table-virtual-scrolling/table-column-filters/table-column-filters.component';
import {
  TableColumnSortingComponent
} from './custom/table/table-virtual-scrolling/table-column-sorting/table-column-sorting.component';
import {
  TableScrollingSettingsComponent
} from './custom/table/table-virtual-scrolling/table-scrolling-settings/table-scrolling-settings.component';
import {TableToolbarComponent} from './custom/table/table-virtual-scrolling/table-toolbar/table-toolbar.component';

import {OnlyNumberDirective} from './custom/table/table-details/directive/only-number.directive';
import {
  TableSkeletonLoaderComponent
} from './custom/table/table-virtual-scrolling/table-skeleton-loader/table-skeleton-loader.component';
import {NgSelectModule} from "@ng-select/ng-select";
import {
  TableBodyVirtualScrollingComponent
} from './custom/table/table-virtual-scrolling/table-body-virtual-scrolling/table-body-virtual-scrolling.component';
import {BreadcrumbComponent} from './custom/breadcrumb/breadcrumb.component';
import {ResizableDivDirective} from "./custom/table/table-details/directive/resizableDiv.directive";
import {CustomToastComponent} from './custom/custom-toast/custom-toast.component';

export const CustomCurrencyMaskConfig: CurrencyMaskConfig = {
  align: "right",
  allowNegative: true,
  decimal: ".",
  precision: 0,
  prefix: "",
  suffix: " ریال",
  thousands: ","
};

@NgModule({
  declarations: [
    // base
    TreeComponent,
    TreeItemComponent,
    HttpSnackbarComponent,
    TreeBranchComponent,
    TreeBranchItemComponent,
    FormActionsComponent,
    TableComponent,
    ToPersianDatePipe,
    MoneyPipe,
    AccountingMoneyPipe,
    ActionBarComponent,
    UploaderComponent,
    ComboSearchComponent,
    ComboTreeComponent,
    ComboAccountRefrenceComponent,
    ComboTagComponent,
    ComboCommodityComponent,
    ComboWarhouseTreeComponent,
    CommodityCategoriesTreeComponent,
    WarehouseLayoutsCommodityHistoryComponent,
    BomHeadersComponent,
    TableFilterComponent,
    tablePaggingComponent,
    FirstKeyPipe,
    ArrayFilterPipe,
    ResizableComponent,
    ResizableDirective,
    ResizableDivDirective,
    TruncatePipe,
    ResizeColumnDirective,
    DynamicTableHeightDirective,
    TableVirtualScrollingComponent,
    CustomDecimalPipe,
    TableColumnFiltersComponent,
    TableColumnSortingComponent,
    TableScrollingSettingsComponent,
    TableToolbarComponent,
    OnlyNumberDirective,
    TableSkeletonLoaderComponent,
    TableBodyVirtualScrollingComponent,
    BreadcrumbComponent,
    CustomToastComponent,

  ],
  exports: [

    TreeComponent,
    FormActionsComponent,
    TableComponent,
    ToPersianDatePipe,
    MoneyPipe,
    AccountingMoneyPipe,
    ActionBarComponent,
    ComboSearchComponent,
    ComboTreeComponent,
    ComboTagComponent,
    UploaderComponent,
    ComboAccountRefrenceComponent,
    ComboCommodityComponent,
    ComboWarhouseTreeComponent,
    CommodityCategoriesTreeComponent,
    WarehouseLayoutsCommodityHistoryComponent,
    BomHeadersComponent,
    WarehouseLayoutsCommodityHistoryComponent,
    TableFilterComponent,
    FirstKeyPipe,
    CurrencyMaskModule,
    tablePaggingComponent,
    ArrayFilterPipe,
    TruncatePipe,

    ResizeColumnDirective,
    DynamicTableHeightDirective,
    ResizableComponent,
    TableVirtualScrollingComponent,
    CustomDecimalPipe,
    OnlyNumberDirective,
    CustomToastComponent,
    ResizableDivDirective,

  ],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    FormsModule,
    AngularMaterialModule,
    SharedModule,
    EChartsModule,
    DragDropModule,
    CurrencyMaskModule,

    ScrollingModule,
    NgxMaskModule.forRoot({
      dropSpecialCharacters: true
    }),
    NgSelectModule,


  ],

  providers: [
    {provide: CURRENCY_MASK_CONFIG, useValue: CustomCurrencyMaskConfig},
    CustomDecimalPipe
  ]
})
export class ComponentsModule {
}
