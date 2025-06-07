import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NotFoundComponent } from './not-found/not-found.component';
import { InDevelopmentComponent } from './in-development/in-development.component';
import {AngularMaterialModule} from "../../core/components/material-design/angular-material.module";
import {MatButtonModule} from "@angular/material/button";
import {MatFormFieldModule} from "@angular/material/form-field";
import {MatOptionModule} from "@angular/material/core";
import {MatProgressSpinnerModule} from "@angular/material/progress-spinner";
import {MatSelectModule} from "@angular/material/select";



@NgModule({
  declarations: [
    NotFoundComponent,
    InDevelopmentComponent,

  ],
  exports: [
    InDevelopmentComponent
  ],
  imports: [
    CommonModule,
    AngularMaterialModule,
    MatButtonModule,
    MatFormFieldModule,
    MatOptionModule,
    MatProgressSpinnerModule,
    MatSelectModule
  ]
})
export class PagesModule { }
