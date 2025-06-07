import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {ComponentsModule} from "./components/components.module";
import {MoneyPipe} from "./pipes/money.pipe";
import {ToPersianDatePipe} from "./pipes/to-persian-date.pipe";
import {FirstKeyPipe} from "./pipes/firstKey.pipe";
import { AccountingMoneyPipe } from './pipes/accounting-money.pipe';
import {ArrayFilterPipe} from "./pipes/arrayFilter.pipe";
import {ToastrModule} from "ngx-toastr";
import {CustomToastComponent} from "./components/custom/custom-toast/custom-toast.component";

@NgModule({

  imports: [
    CommonModule,
    ComponentsModule,
    ToastrModule.forRoot(
      {
        toastComponent: CustomToastComponent,
        maxOpened : 1
      }
    ),
  ],
  providers: [
    MoneyPipe,
    FirstKeyPipe,
    ToPersianDatePipe,
    AccountingMoneyPipe,
    ArrayFilterPipe,

  ]
})
export class CoreModule {
}
