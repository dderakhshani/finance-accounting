import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import {NewContainerComponent} from "./new-container.component";
import {SidebarComponent} from "./sidebar/sidebar.component";
import {HeaderComponent} from "./header/header.component";
import {MatSidenavModule} from "@angular/material/sidenav";
import {MatInputModule} from "@angular/material/input";
import {MatSelectModule} from "@angular/material/select";
import { ContentComponent } from './content/content.component';
import {MatIconModule} from "@angular/material/icon";
import {MatProgressSpinnerModule} from "@angular/material/progress-spinner";
import {FormsModule, ReactiveFormsModule} from "@angular/forms";
import {MatMenuModule} from "@angular/material/menu";
import {MatTooltipModule} from "@angular/material/tooltip";
import {SidebarItemComponent} from "./sidebar/sidebar-item/sidebar-item.component";



@NgModule({
  declarations: [
    HeaderComponent,
    SidebarComponent,
    ContentComponent,
    SidebarItemComponent ,
    NewContainerComponent,
  ],
  imports: [
    CommonModule,
    MatSidenavModule,
    MatInputModule,
    MatSelectModule,
    MatIconModule,
    MatProgressSpinnerModule,
    FormsModule,
    MatMenuModule,
    MatTooltipModule,
    ReactiveFormsModule
  ],
  exports: [NewContainerComponent]
})
export class NewContainerModule { }
