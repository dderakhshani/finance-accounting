import { Component } from '@angular/core';

import { InvoiceBuy } from '../invoice-buy-abstrac-class';
import { UpdateIsDocumentIssuanceCommand } from '../../../../repositories/receipt/commands/reciept/update-isDocumentIssuance-command';

@Component({
  selector: 'app-invoice-buy',
  templateUrl: './invoice-buy.component.html',
  styleUrls: ['./invoice-buy.component.scss']
})
export class InvoiceBuyComponent extends InvoiceBuy {

  onSelectAll(stause: boolean) {
    this.selectedAll = stause;

    this.data.forEach(a => {
      if (this.selectedAll) {
        this.checkValue(a)
      }
      else {
        this.RemoveId(a)
      }
    })
  }
  async clearDocuments() {
    var request = new UpdateIsDocumentIssuanceCommand();
    request.ids = this.Service.ListId;
    await this._mediator.send(<UpdateIsDocumentIssuanceCommand>request).then(a => {
      this.get();
      this.Service.ListId = [];
    })
  }
}
