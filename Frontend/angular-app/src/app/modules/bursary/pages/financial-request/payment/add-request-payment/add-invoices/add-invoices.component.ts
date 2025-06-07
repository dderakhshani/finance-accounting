import {
  Component,
  Inject,
  OnInit,
  TemplateRef,
  ViewChild,
} from "@angular/core";
import { FormArray } from "@angular/forms";
import { MatDialogRef, MAT_DIALOG_DATA } from "@angular/material/dialog";
import { ActivatedRoute, Router } from "@angular/router";
import { data } from "jquery";
import { Observable } from "rxjs/internal/Observable";
import { BaseComponent } from "src/app/core/abstraction/base.component";
import {
  TableConfigurations,

} from "src/app/core/components/custom/table/models/table-configurations";
import { FormActionTypes } from "src/app/core/constants/form-action-types";
import { Mediator } from "src/app/core/services/mediator/mediator.service";
import { DocumentHead } from "src/app/modules/bursary/entities/document-head";
import { FinancialRequestDocumentHeads } from "src/app/modules/bursary/entities/financial-request-document-head";
import { RequestPayment } from "src/app/modules/bursary/entities/request-payment";
import { GetDocumentHeadsQueries } from "src/app/modules/bursary/repositories/financial-request/document-head/queries/get-document-heads-queries";
import { SearchQuery } from "src/app/shared/services/search/models/search-query";
import {TableColumnFilter} from "../../../../../../../core/components/custom/table/models/table-column-filter";
import {TableColumnDataType} from "../../../../../../../core/components/custom/table/models/table-column-data-type";
import {
  TableColumnFilterTypes
} from "../../../../../../../core/components/custom/table/models/table-column-filter-types";
import {TableOptions} from "../../../../../../../core/components/custom/table/models/table-options";
import {TableColumn} from "../../../../../../../core/components/custom/table/models/table-column";

@Component({
  selector: "app-add-invoices",
  templateUrl: "./add-invoices.component.html",
  styleUrls: ["./add-invoices.component.scss"],
})
export class AddInvoicesComponent extends BaseComponent {
  //  @ViewChild('addDocHead', { read: TemplateRef }) addDocHead!: TemplateRef<any>;

  isValue: boolean = false;
  tableConfigurations!: TableConfigurations;
  financialRequest !:RequestPayment;
  financialRequestDocumentHead !:FinancialRequestDocumentHeads;

  constructor(
    private _mediator: Mediator,
    private route: ActivatedRoute,
    public dialogRef: MatDialogRef<AddInvoicesComponent>,
    private router: Router,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) {
    super(route, router);
  }

  async ngOnInit() {
    await this.resolve();
  }

  resolve() {
    this.formActions = [
      FormActionTypes.add,
      FormActionTypes.save,
      FormActionTypes.saveandexit,
      FormActionTypes.list,
    ];
    this.financialRequest = JSON.parse(this.data.data);
    this.getDocumentHeadByReferenceId(this.financialRequest);
  }
  initialize() {}
  add(param?: any) {
    let documentHeads: DocumentHead[];
    documentHeads = this.form.value.filter((x:any)=>x.selected);

    this.financialRequestDocumentHead.financialRequestId = this.financialRequest.id
    this.financialRequestDocumentHead.documentHeadsId=documentHeads.map(({ id }) => id)

    this.dialogRef.close(this.financialRequestDocumentHead);

  }
  get(param?: any) {}
  update(param?: any) {}
  delete(param?: any) {}
  close() {}

  async getDocumentHeadByReferenceId(item: RequestPayment) {
    let searchQueries: SearchQuery[] = [];

    searchQueries.push(new SearchQuery({
      propertyName: "ReferenceId",
      values: [], //item.referenceId
      comparison: "equal",
    }));

    return await this._mediator.send(new GetDocumentHeadsQueries(0, 0, searchQueries))
      .then((res) => {
        this.getDocumentHeads(res.data);
      });
  }
  getDocumentHeads(data: DocumentHead[]) {

    let columns: TableColumn[] = [
      new TableColumn('selected', '', TableColumnDataType.Select, '2.5%'),
      new TableColumn('index', 'ردیف', TableColumnDataType.Index, '2.5%'),
      new TableColumn(
        "priceMinusDiscountPlusTax",
        "مبلغ قابل پرداخت",
        TableColumnDataType.Text,
        "15%",
        true,
        new TableColumnFilter(
          "priceMinusDiscountPlusTax",
          TableColumnFilterTypes.Text
        )
      ),
      new TableColumn(
        "documentDiscount",
        "تخفیف",
        TableColumnDataType.Text,
        "15%",
        true,
        new TableColumnFilter("documentDiscount", TableColumnFilterTypes.Text)
      ),
      new TableColumn(
        "documentNo",
        "شماره صورتحساب",
        TableColumnDataType.Text,
        "15%",
        true,
        new TableColumnFilter("documentNo", TableColumnFilterTypes.Text)
      ),

      new TableColumn(
        "documentDate",
        "تاریخ فاکتور",
        TableColumnDataType.Date,
        "15%",
        true,
        new TableColumnFilter("documentDate", TableColumnFilterTypes.Date)
      ),

      new TableColumn(
        "vatTax",
        "عوارض ارزش افزوده",
        TableColumnDataType.Text,
        "15%",
        true,
        new TableColumnFilter("vatTax", TableColumnFilterTypes.Text)
      ),
      new TableColumn(
        "vatDutiesTax",
        "مالیات ارزش افزوده",
        TableColumnDataType.Text,
        "15%",
        true,
        new TableColumnFilter("vatDutiesTax", TableColumnFilterTypes.Text)
      ),
      //  colAddDocumentToFinancialRequest
    ];
    this.tableConfigurations = new TableConfigurations(
      columns,
      new TableOptions(true, true)
    );
    this.form = new FormArray(data.map((x) => this.createForm(x)));
  }

}
