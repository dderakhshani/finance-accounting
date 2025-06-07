import { Component } from '@angular/core';
import { WarehouseReceiptsBooksComponent } from '../warehouse-receipts-books.component';
import { SearchQuery } from '../../../../../../shared/services/search/models/search-query';
import { GetReceiptsCommoditesQuery } from '../../../../repositories/commodity/get-receipt-commodites-query';
@Component({
  selector: 'app-warehouse-receipts-book',
  templateUrl: './warehouse-receipts-book.component.html',
  styleUrls: ['./warehouse-receipts-book.component.scss']
})
export class WarehouseReceiptsBookComponent extends WarehouseReceiptsBooksComponent {


  async navigateToRecive(item: any) {

    await this.router.navigateByUrl(`inventory/receiptDetails?id=${item.documentHeadId}&displayPage=archive`)

  }
  async navigateToHistory(Receipt: any) {


    let searchQueries: SearchQuery[] = []

    searchQueries.push(new SearchQuery({
      propertyName: "Code",
      values: [Receipt.commodityCode],
      comparison: "equal",
      nextOperand: "and "
    }))
    await this._mediator.send(new GetReceiptsCommoditesQuery(true, Receipt.warehouseId, "", 0, 50, searchQueries)).then(res => {

      this.router.navigateByUrl(`inventory/commodityReceiptReports?commodityId=${res.data[0].id}&warehouseId=${Receipt.warehouseId}`)
    });


  }
}
