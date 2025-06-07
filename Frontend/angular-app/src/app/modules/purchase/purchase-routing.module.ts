import { NgModule } from '@angular/core';
import { RouterModule, Routes } from "@angular/router";
import { ArchiveContractListComponent } from './pages/invoice-list/archive-contract-list/archive-contract-list.component';
import { ArchiveFactorListComponent } from './pages/invoice-list/archive-factor-list/archive-factor-list.component';
import { ContractListComponent } from './pages/invoice-list/contract-list/contract-list.component';
import { InvoiceDetailsComponent } from './pages/invoice-list/details/invoice-details.component';
import { FactorListComponent } from './pages/invoice-list/factor-list/factor-list.component';
import { InvoiceListComponent } from './pages/invoice-list/invoice-list.component';

import { AddContractComponent } from './pages/invoice-operations/add-contract/add-contract.component';
import { AddFactorComponent } from './pages/invoice-operations/add-factor/add-factor.component';
import { AddIncoiceItemsComponent } from './pages/invoice-operations/add-invoice-items/add-invoice-items.component';



const routes: Routes = [
  {
    path: '',
    data: {
      title: 'purchase'
    },
    children: [
      //{
      //  path: 'archiveInvoiceList',
      //  component: ArchiveInvoiceListComponent,
      //  data: {
      //    title: 'فاکتورهای بایگانی شده',
      //    id: '#archiveInvoiceList',
      //    isTab: true
      //  },
      //},
      {
        path: 'archiveContractList',
        component: ArchiveContractListComponent,
        data: {
          title: 'قراردادهای بایگانی شده',
          id: '#archiveContractList',
          isTab: true
        },
      },
      {
        path: 'invoice-operations/addContract',
        component: AddContractComponent,
        data: {
          title: 'ثبت قرارداد',
          id: '#addInvoiceContract',
          isTab: true
        },
      },
      {
        path: 'contractList',
        component: ContractListComponent,
        data: {
          title: 'فهرست قراردادها',
          id: '#contractList',
          isTab: true
        },
      },
      {
        path: 'invoice-operations/addFactor',
        component: AddFactorComponent,
        data: {
          title: 'ثبت پیش فاکتور',
          id: '#addFactor',
          isTab: true
        },
      },
      {
        path: 'archiveFactorList',
        component: ArchiveFactorListComponent,
        data: {
          title: 'پیش فاکتورهای بایگانی شده',
          id: '#archiveFactorList',
          isTab: true
        },
      },

      
      {
        path: 'factorList',
        component: FactorListComponent,
        data: {
          title: 'فهرست پیش فاکتورها',
          id: '#FactorList',
          isTab: true
        },
      },
      {
        path: 'invoiceList',
        component: InvoiceListComponent,
        data: {
          title: 'فهرست جامع',
          id: '#invoiceList',
          isTab: true
        },
      },
      {
        path: 'addContractItems',
        component: AddIncoiceItemsComponent,
        data: {
          title: 'ویرایش جزئیات',
          id: '#addContractItems',
          isTab: true
        },
      },
      {
        path: 'invoiceDetails',
        component: InvoiceDetailsComponent,
        data: {
          title: 'جزئیات',
          id: '#invoiceDetails',
          isTab: true
        },
      },
      
      
    ],
  },
]

@NgModule({

  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class PurchaseRoutingModule { }
