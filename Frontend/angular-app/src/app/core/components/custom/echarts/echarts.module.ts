import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NgxEchartsModule } from 'ngx-echarts';
import 'echarts/theme/sakura.js';
import 'echarts/theme/royal.js';
import 'echarts/theme/roma.js';
import 'echarts/theme/shine.js';
import 'echarts/theme/tech-blue.js';
import 'echarts/theme/vintage.js';
import 'echarts/theme/dark.js';
import 'echarts/theme/dark-blue.js';
import 'echarts/theme/azul.js';
import 'echarts/theme/caravan.js';
import 'echarts/theme/forest.js';
import 'echarts/theme/eduardo.js';
import 'echarts/theme/fresh-cut.js';
import 'echarts/theme/fresh-cut.js';
import 'echarts/theme/macarons2.js';
import 'echarts/theme/mint.js';
import 'echarts/theme/fruit.js';
import 'echarts/theme/cool.js';
import { GaugeWidgetComponent } from './gauge-widget/gauge-widget.component';
import { BarWidgetComponent } from './bar-widget/bar-widget.component';



@NgModule({
  declarations: [
    GaugeWidgetComponent,
    BarWidgetComponent
  ],
  imports: [
    CommonModule,
    NgxEchartsModule.forRoot({
      echarts: () => import('echarts')
    }),
  ],
  exports: [
    NgxEchartsModule,
    GaugeWidgetComponent,
    BarWidgetComponent
  ]
})
export class EChartsModule { }
