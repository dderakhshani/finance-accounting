import { NgModule } from '@angular/core';
import {Route, RouterModule} from "@angular/router";
import { MapSamatozinToDanaListComponent } from './pages/map-samatozin-to-dana/map-samatozin-to-dana-list.component';

let routes: Route[];
routes = [
  {
    path: '',
    data: {
      title: 'logistics'
    },
    children: [
      
      {
        path: 'mapSamatozinToDanaList',
        component: MapSamatozinToDanaListComponent,
        data: {
          title: 'تبدیل کد های تامین کنندگان سما توزین',
          id: '#mapSamatozinToDanaList',
          isTab: true
        },
      },
    ]
  }
]

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class LogisticsRoutingModule { }
