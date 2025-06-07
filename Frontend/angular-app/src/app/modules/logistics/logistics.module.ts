import { MatDialogComponent } from './../../core/components/material-design/mat-dialog/mat-dialog.component';

import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from '@angular/core';
import { CommonModule, DatePipe } from '@angular/common';
import { MatFormFieldModule } from "@angular/material/form-field";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { MatCardModule } from "@angular/material/card";
import { ComponentsModule } from "../../core/components/components.module";
import { MatInputModule } from "@angular/material/input";
import { MatButtonModule } from "@angular/material/button";
import { LogisticsRoutingModule } from './logistics-routing.module';
import { AngularMaterialModule } from '../../core/components/material-design/angular-material.module';
import { NgxMaskModule } from 'ngx-mask';
import { AddDriverComponent } from './pages/driver/add-driver/add-driver.component';
import { DriverListComponent } from './pages/driver/driver-list/driver-list.component';
import { AddBankComponent } from './pages/bank/add-bank/add-bank.component';
import { BankListComponent } from './pages/bank/bank-list/bank-list.component';
import { AddFreightComponent } from './pages/freight/add-freight/add-freight.component';
import { AddUsanceComponent } from './pages/usance/add-usance/add-usance.component';
import { UsanceListComponent } from './pages/usance/usance-list/usance-list.component';
import { MapSamatozinToDanaListComponent } from './pages/map-samatozin-to-dana/map-samatozin-to-dana-list.component';
import { InventoryModule } from '../inventory/inventory.module';
import { CurrencyMaskModule } from 'ng2-currency-mask';
import { MapSamatozinToDanaDialogComponent } from './pages/map-samatozin-to-dana/map-samatozin-to-dana-dialog/samatozin-to-dana-dialog.component';


@NgModule({
  declarations: [
    AddDriverComponent,
    DriverListComponent,
    AddBankComponent,
    BankListComponent,
    AddFreightComponent,
    AddUsanceComponent,
    UsanceListComponent,
    MapSamatozinToDanaListComponent,
    MapSamatozinToDanaDialogComponent
  ],

  imports: [
    CommonModule,
    LogisticsRoutingModule,
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
  ],
 
})
export class LogisticsModule { }
