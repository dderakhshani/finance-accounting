import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {CommodityCategoriesComponent} from "./pages/commodity-categories/commodity-categories.component";
import {
  CommodityCategoryPropertyComponent
} from "./pages/commodity-category-property/commodity-category-property.component";
import {AddCommodityComponent} from "./pages/commodity/add-commodity/add-commodity.component";
import {CommodityListComponent} from "./pages/commodity/commodity-list/commodity-list.component";
import {BomComponent} from "./pages/bom/bom.component";


const routes: Routes = [
  {
    path:'',
    children: [
      {
        path: 'commodityCategories',
        component: CommodityCategoriesComponent,
        data: {
          title: 'دسته بندی کالا',
          id: '#CommodityCategories',
          isTab: true
        },
      },
      {
        path: 'commodityCategoryProperties',
        component: CommodityCategoryPropertyComponent,
        data: {
          title: 'تعریف خصوصیات',
          id: '#CommodityCategoryProperties',
          isTab: true
        },
      },
      {
        path: 'add',
        component: AddCommodityComponent,
        data: {
          title: 'تعریف کالا',
          id: '#AddCommodity',
          isTab: true
        },
      },
      {
        path: 'list',
        component: CommodityListComponent,
        data: {
          title: 'فهرست کالا',
          id: '#CommodityList',
          isTab: true
        },
      },
      {
        path:'bom',
        component: BomComponent,
        data: {
          title: 'فرمول ساخت',
          id: '#Bom',
          isTab: true
        },
      }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CommodityRoutingModule { }
