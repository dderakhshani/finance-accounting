import { Component } from '@angular/core';

import { WarehouseReceiptsBooksComponent } from '../warehouse-receipts-books.component';
import { SearchQuery } from '../../../../../../shared/services/search/models/search-query';
import { GetReceiptsCommoditesQuery } from '../../../../repositories/commodity/get-receipt-commodites-query';
@Component({
  selector: 'app-warehouse-receipts-book-Rial',
  templateUrl: './warehouse-receipts-book-Rial.component.html',
  styleUrls: ['./warehouse-receipts-book-Rial.component.scss']
})
export class WarehouseReceiptsBookRilaComponent extends WarehouseReceiptsBooksComponent {

  async navigateToRecive(item: any) {

    await this.router.navigateByUrl(`inventory/receiptDetails?id=${item.documentHeadId}&displayPage=none`)

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

      this.router.navigateByUrl(`inventory/commodityReceiptReportsRial?commodityId=${res.data[0].id}&warehouseId=${Receipt.warehouseId}`)
    });


  }
}
