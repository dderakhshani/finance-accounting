import {NgModule} from '@angular/core';
import {Routes, RouterModule} from '@angular/router';
import {DashboardComponent} from "./dashboard.component";

import {
  CreatedCorrectionRequestsComponent
} from "./components/created-correction-requests/created-correction-requests.component";
import {GeneralDashboardComponent} from "./components/general-dashboard/general-dashboard.component";


const routes: Routes = [
  {
    path: '',
    component: DashboardComponent,
    data: {
      title: 'داشبورد',
      id: '#Dashboard',
      isTab: true

    },
    children: [
      {
        path: 'dashboard',
        component: DashboardComponent,
        data: {
          title: 'داشبورد',
          id: '#Dashboard',
          isTab: true

        },
      },
      {
        path: 'generalDashboard',
        component: GeneralDashboardComponent,
        data: {
          title: 'جنرال داشبورد',
          id: '#generalDashboard',
          isTab: true

        },
      },
      {

        path: 'CreatedCorrectionRequests',
        component: CreatedCorrectionRequestsComponent,
        data: {
          title: 'فهرست درخواست تغییرات من',
          id: '#CreatedCorrectionRequests',
          isTab: true
        }
      },
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class DashboardRoutingModule {
}
