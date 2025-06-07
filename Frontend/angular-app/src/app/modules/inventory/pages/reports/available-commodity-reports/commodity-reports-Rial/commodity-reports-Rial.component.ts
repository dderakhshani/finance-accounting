
import { Component } from '@angular/core';
import { AvailableCommodityReportsComponent } from "../available-commodity-reports.component";
import { spCommodityReports } from '../../../../entities/spCommodityReports';


@Component({
  selector: 'app-commodity-reports-rial',
  templateUrl: './commodity-reports-rial.component.html',
  styleUrls: ['../commodity-reports.component.scss']
})

export class CommodityReportsRialComponent extends AvailableCommodityReportsComponent {


  
  async navigateToDetails(ware: spCommodityReports) {

    var url = `inventory/commodityReceiptReportsRial?commodityId=${ware.commodityId}&warehouseId=${ware.warehouseId}`
    if (this.SearchForm.controls.fromDate.value) {
      url += `&fromDate=${this.Service.formatDate(this.SearchForm.controls.fromDate.value)}`
    }
    if (this.SearchForm.controls.toDate.value) {
      url += `&toDate=${this.Service.formatDate(this.SearchForm.controls.toDate.value)}`
    }
    await this.router.navigateByUrl(url)

  }
}
