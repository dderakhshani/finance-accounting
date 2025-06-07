import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import {CustomerListComponent} from "./pages/customer-list/customer-list.component";
import {AddPersonComponent} from "../admin/pages/person/add-person/add-person.component";

const routes: Routes = [

  {
    path: 'customers',
    children: [
      {
        path:'add',
        component: AddPersonComponent,
        data: {
          title:' تعریف مشتری',
          id: '#addCustomer',
          isTab: true
        }
      },
      {
        path:'list',
        component: CustomerListComponent,
        data: {
          title:'فهرست مشتریان',
          id: '#customersList',
          isTab: true
        }
      }
    ]
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class SalesRoutingModule { }
