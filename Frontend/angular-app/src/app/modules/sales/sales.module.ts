import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { SalesRoutingModule } from './sales-routing.module';
import {ReactiveFormsModule} from "@angular/forms";
import {AngularMaterialModule} from "../../core/components/material-design/angular-material.module";
import {ComponentsModule} from "../../core/components/components.module";
import { CustomerListComponent } from './pages/customer-list/customer-list.component';
import {AdminModule} from "../admin/admin.module";


@NgModule({
  declarations: [
    CustomerListComponent,
  ],
    imports: [
        CommonModule,
        SalesRoutingModule,
        ReactiveFormsModule,
        AngularMaterialModule,
        ComponentsModule,
        AdminModule
    ]
})
export class SalesModule { }
