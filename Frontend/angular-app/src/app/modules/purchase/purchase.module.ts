import { NgModule } from '@angular/core';
import { CommonModule, DatePipe } from '@angular/common';

import { PurchaseRoutingModule } from "./purchase-routing.module";
import { MatFormFieldModule } from "@angular/material/form-field";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { MatCardModule } from "@angular/material/card";
import { ComponentsModule } from "../../core/components/components.module";
import { MatInputModule } from "@angular/material/input";
import { MatButtonModule } from "@angular/material/button";
import { AngularMaterialModule } from "../../core/components/material-design/angular-material.module";

import { InvoiceListComponent } from './pages/invoice-list/invoice-list.component';
import { AddContractComponent } from './pages/invoice-operations/add-contract/add-contract.component';
import { InvoiceDetailsComponent } from './pages/invoice-list/details/invoice-details.component';

import { InventoryModule } from '../inventory/inventory.module';
import { ArchiveContractListComponent } from './pages/invoice-list/archive-contract-list/archive-contract-list.component';
import { ContractListComponent } from './pages/invoice-list/contract-list/contract-list.component';
import { AddFactorComponent } from './pages/invoice-operations/add-factor/add-factor.component';

import { FactorListComponent } from './pages/invoice-list/factor-list/factor-list.component';
import { ArchiveFactorListComponent } from './pages/invoice-list/archive-factor-list/archive-factor-list.component';
import { AddIncoiceItemsComponent } from './pages/invoice-operations/add-invoice-items/add-invoice-items.component';
import { CurrencyMaskModule } from 'ng2-currency-mask';




@NgModule({
  declarations: [
    ArchiveContractListComponent,
    InvoiceListComponent,
    InvoiceDetailsComponent,
    AddContractComponent,
    AddIncoiceItemsComponent, 
    ContractListComponent,
    AddFactorComponent,
   
    FactorListComponent,
    ArchiveFactorListComponent
   
  ],
  exports: [
    

  ],
  imports: [
    CommonModule,
    PurchaseRoutingModule,
    MatFormFieldModule,
    ReactiveFormsModule,
    FormsModule,
    MatCardModule,
    ComponentsModule,
    MatInputModule,
    MatButtonModule,
    AngularMaterialModule,
    InventoryModule,
    CurrencyMaskModule
    


  ]
})
export class PurchaseModule { }
