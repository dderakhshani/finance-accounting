import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

// import { MainContainerComponent } from './main-container/main-container.component';
// import {HeaderComponent} from "./main-container/header/header.component";
// import {SidebarComponent} from "./main-container/sidebar/sidebar.component";
// import {TabsComponent} from "./main-container/tabs/tabs.component";
// import {SidebarItemComponent} from "./main-container/sidebar/sidebar-item/sidebar-item.component";
import {RouterModule} from "@angular/router";
import {ComponentsModule} from "../core/components/components.module";
import {AngularMaterialModule} from "../core/components/material-design/angular-material.module";
import {ReactiveFormsModule} from "@angular/forms";
import {NewContainerModule} from "./new-container/new-contriner-module.module";


@NgModule({
  declarations: [
    // MainContainerComponent,
    // HeaderComponent,
    // SidebarComponent,
    // TabsComponent,
    // SidebarItemComponent,

  ],
  imports: [
    CommonModule,
    RouterModule,
    ComponentsModule,
    AngularMaterialModule,
    ReactiveFormsModule,
    NewContainerModule
  ]
})
export class LayoutModule { }
