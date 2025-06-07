import { Component } from '@angular/core';
import { AddManualTemporaryReceiptComponent } from '../add-manual-temporary-receipt/add-manual-temporary-receipt.component';
import { ReceiptAllStatusModel } from '../../../../entities/receipt-all-status';
import { Warehouse } from '../../../../entities/warehouse';

@Component({
  selector: 'app-add-manual-temporary-receipt-small',
  templateUrl: './add-manual-temporary-receipt-small.component.html',
  styleUrls: ['./add-manual-temporary-receipt-small.component.scss']
})
export class AddManualTemporaryReceiptSamallComponent extends AddManualTemporaryReceiptComponent {

  async ngOnInit() {
    setTimeout(() => {
     
      this.form.controls.codeVoucherGroupId.setValue(2384);
      this.form.controls['documentNo'].disable();

    }, 20);
   
  }
  
}
