import {Component, HostListener, TemplateRef, ViewChild} from '@angular/core';
import {InvoiceBuy} from '../invoice-buy-abstrac-class';
import {
  DynamicTableHeightDirective
} from "../../../../../../core/components/custom/table/table-details/directive/dynamic-table-height.directive";
import {
  UpdateReadStatusReceiptCommand
} from "../../../../repositories/receipt/commands/reciept/update-read-status-receipt-command";
import {invoice} from "../../../../../purchase/entities/invoice";

@Component({
  selector: 'app-invoice-buy-currency',
  templateUrl: './invoice-buy-currency.component.html',
  styleUrls: ['./invoice-buy-currency.component.scss']
})
export class InvoiceBuyCurrencyComponent extends InvoiceBuy {
  @ViewChild(DynamicTableHeightDirective) tableHeightDirective!: DynamicTableHeightDirective;
  marginFromBottom: number = 50;
  SearchInput: string = '';
  selectedReadTab: any = 1;

  async ngOnInit() {
    this.isProvider = true;
    this.Service.ListId = [];
    this.isReadReceipt = 1;
   
  }

  onTotalQuantity(invoices: any[]) {
    let total = 0;
    let selectedInvoices = invoices.filter(a => a.selected);
    if (selectedInvoices.length > 0)
      selectedInvoices.forEach(a => total += Number(a.quantity));
    else
      invoices.forEach(a => total += Number(a.quantity));
    return total;
  }

  onTotalItemUnitPrice(invoices: any[]) {
    let total = 0;
    let selectedInvoices = invoices.filter(a => a.selected);
    if (selectedInvoices.length > 0)
      selectedInvoices.forEach(a => total += Number(a.itemUnitPrice));
    else
      invoices.forEach(a => total += Number(a.itemUnitPrice));
    return total;
  }

  onSumAll(invoices: any[]) {
    let total = 0;
    
    let selectedInvoices = invoices.filter(a => a.selected);
    if (selectedInvoices.length > 0)
      selectedInvoices.forEach(a => total += Number(a.itemUnitPrice) * Number(a.quantity));
    else
      invoices.forEach(a => total += Number(a.itemUnitPrice) * Number(a.quantity));
    return total;
  }

  SelectAll(statuse: boolean, invoices: any[]) {
    this.selectedAll = statuse;
    invoices.forEach(a => {
      if (this.selectedAll) {
        this.checkValue(a)
      } else {
        this.RemoveId(a)
      }
    })
  }

  SortTable(event: any, Reports_filter: any) {
    var accessKey: any = event.target.getAttribute("accessKey");
    if (accessKey) {
      var dir = event.target.getAttribute('sort-data') || 'asc';
      event.target.setAttribute("sort-data", dir == 'asc' ? 'desc' : 'asc');
      event.target.classList.remove(dir == 'asc' ? 'triangle-down' : 'triangle-up');
      event.target.classList.add(dir == 'asc' ? 'triangle-up' : 'triangle-down');
      this.sortlist(accessKey, dir == 'asc' ? true : false, Reports_filter)
    }
  }

  sortlist(colName: string, boolean: boolean, Reports_filter: any) {
    if (boolean == true) {
      Reports_filter.sort((a: any, b: any) => a[colName] < b[colName] ? 1 : a[colName] > b[colName] ? -1 : 0)

    } else {
      Reports_filter.sort((a: any, b: any) => a[colName] > b[colName] ? 1 : a[colName] < b[colName] ? -1 : 0)
    }
    this.booleanValue = !this.booleanValue
  }

  filterTable(tableId: string | any) {
    const filter: string = this.SearchInput.trim().toUpperCase();
    const table: HTMLElement | null = document.getElementById(tableId);
    if (table) {
      const tr: HTMLCollectionOf<HTMLTableRowElement> = table.getElementsByTagName("tr");
      for (let i: number = 0; i < tr.length; i++) {

        const td: any = tr[i].getElementsByTagName("td");

        let j = 0
        let display = "";
        if (this.SearchInput == "" || this.SearchInput == undefined) {
          tr[i].style.display = "";
          continue
        }
        for (j = 0; j < td.length; j++) {
          if (td[j].accessKey) {

            const txtValue: string = td[j].innerText;

            if (txtValue.toUpperCase().includes(filter)) {

              display = "block";

              continue
            }

          }
        }//End For J---------------
        if (display != "block") {
          tr[i].style.display = "none";
        } else {
          tr[i].style.display = "";
        }

      }//End For I---------------

    }//End IF table
  }

  async updateReadStatus(item: any, event: any) {
    let request = new UpdateReadStatusReceiptCommand();
    let ids = new Array<number>();
    ids.push(item.documentHeadId);
    request.ids = ids;
    request.isRead = event.checked;
    let response = await this._mediator.send(<UpdateReadStatusReceiptCommand>request);
    this.get();
  }

  async updateAllReadStatus(isRead: boolean, invoices: any[]) {

    this.selectedAllRead = isRead;
    let request = new UpdateReadStatusReceiptCommand();
    let ids = new Array<number>();
    invoices.forEach(a => {
      ids.push(a.documentHeadId);
    })
    request.ids = ids;
    request.isRead = isRead;
    let response = await this._mediator.send(<UpdateReadStatusReceiptCommand>request);
    this.get();
  }

  getLengthIsRead(receipt: any) {
    return receipt.filter((x: any) => x.isRead == true).length;
  }

  adjustHeight() {
    if (this.tableHeightDirective) {
      this.tableHeightDirective.adjustTableHeight();
    }
  }

  setvalue(number: number) {
    this.isReadReceipt = number;
    this.get();
    this.isReadReceipt = 1;
  }

  getAllReciptData() {
    this.get();
    this.selectedReadTab = 1;
  }

  clearCurrentListId() {
    this.Receipts.forEach(receipt => {
      receipt.Parts.forEach((part: any) => {
        part.Invoices.forEach((invoice: any) => {
          invoice.selected = false;
        })
      })
    })
    this.privateListId = [];
    this.CalculateSum();
  }

  clearCurrentDocuments() {
    this.Receipts.forEach(receipt => {
      receipt.Parts.forEach((part: any) => {
        part.Invoices.forEach((invoice: any) => {
          invoice.allowInput = false;
          this.RemoveId(invoice);
        })
      })
    })
  }


  protected readonly invoice = invoice;
}
