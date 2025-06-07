import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DashboardComponent } from './dashboard.component';
import {DashboardRoutingModule} from "./dashboard-routing.module";
import {ComponentsModule} from "../../core/components/components.module";
import {ReactiveFormsModule} from "@angular/forms";
import {AngularMaterialModule} from "../../core/components/material-design/angular-material.module";
import {EChartsModule} from "../../core/components/custom/echarts/echarts.module";
import { RecentTabsGadgetComponent } from './components/recent-tabs-gadget/recent-tabs-gadget.component';
import { BookmarkedTabsGadgetComponent } from './components/bookmarked-tabs-gadget/bookmarked-tabs-gadget.component';
import { RecentTabCardComponent } from './components/recent-tabs-gadget/recent-tab-card/recent-tab-card.component';
import {
  CreatedCorrectionRequestsComponent
} from "./components/created-correction-requests/created-correction-requests.component";
import { GeneralDashboardComponent } from './components/general-dashboard/general-dashboard.component';



@NgModule({
  declarations: [
    DashboardComponent,
    RecentTabsGadgetComponent,
    BookmarkedTabsGadgetComponent,
    RecentTabCardComponent,
    CreatedCorrectionRequestsComponent,
    GeneralDashboardComponent
  ],
  imports: [
    CommonModule,
    DashboardRoutingModule,
    ComponentsModule,
    ReactiveFormsModule,
    AngularMaterialModule,
    EChartsModule
  ]
})
export class DashboardModule { }
