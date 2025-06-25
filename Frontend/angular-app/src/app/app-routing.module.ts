import { NgModule } from '@angular/core';
import { PreloadAllModules, RouterModule, Routes } from '@angular/router';
// import { MainContainerComponent } from "./layouts/main-container/main-container.component";
import { NewContainerComponent } from "./layouts/new-container/new-container.component";
import { VersionUpdateGuard } from "./core/gaurds/version-update.guard";
import { IdentityResolver } from "./modules/identity/IdentityResolver/identity.resolver";
import { environment } from "../environments/environment";

const routes: Routes = [
    {
        path: '',
        component: NewContainerComponent,
        resolve: { init: IdentityResolver },
        canActivate: [VersionUpdateGuard,],
        data: {
            title: 'Home',
        },
        children: [
            // {
            //   path: '',
            //   pathMatch: 'full',
            //   redirectTo: 'dashboard'
            // },
            {
                path: 'dashboard',
                loadChildren: () => import('./modules/dashboard/dashboard.module').then(m => m.DashboardModule)
            },
            {
                path: 'admin',
                loadChildren: () => import('./modules/admin/admin.module').then(m => m.AdminModule)
            }
            ,
            {
                path: 'archive',
                loadChildren: () => import('./modules/archive/archive.module').then(m => m.ArchiveModule)
            },

            {
                path: 'accounting',
                loadChildren: () => import('./modules/accounting/accounting.module').then(m => m.AccountingModule)
            },
            {
                path: 'inventory',
                loadChildren: () => import('./modules/inventory/inventory.module').then((m => m.InventoryModule))
            },
            {
                path: 'purchase',
                loadChildren: () => import('./modules/purchase/purchase.module').then((m => m.PurchaseModule))
            },
            {
                path: 'logistics',
                loadChildren: () => import('./modules/logistics/logistics.module').then((m => m.LogisticsModule))
            },
            {
                path: 'sales',
                loadChildren: () => import('./modules/sales/sales.module').then((m => m.SalesModule))
            },
            {
                path: 'bursary',
                loadChildren: () => import('./modules/bursary/bursary.module').then((m => m.BursaryModule))
            },
            {
                path: 'commodity',
                loadChildren: () => import('./modules/commodity/commodity.module').then((m => m.CommodityModule))
            },
            {
                path: 'tickets',
                loadChildren: () => import('./modules/ticketing/ticketing.module').then((m => m.TicketingModule))
            },
        ]
    },
    {
        path: 'authentication',
        loadChildren: () => import('./modules/identity/identity.module').then(m => m.IdentityModule)
    }
];

@NgModule({
    imports: [RouterModule.forRoot(routes, { preloadingStrategy: PreloadAllModules })],
    exports: [RouterModule]
})
export class AppRoutingModule { }
