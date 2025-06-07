import {Component, Inject, ViewChild} from '@angular/core';
import {BaseComponent} from "../../../../../../../core/abstraction/base.component";
import {Mediator} from "../../../../../../../core/services/mediator/mediator.service";
import {MAT_DIALOG_DATA, MatDialogRef} from "@angular/material/dialog";
import {AutoVoucherFormula} from "../../../../../entities/AutoVoucherFormula";
import {PageModes} from "../../../../../../../core/enums/page-modes";
import {
  CreateAutoVoucherFormulaCommand
} from "../../../../../repositories/auto-voucher-formula/commands/create-auto-voucher-formula-command";
import {
  UpdateAutoVoucherFormulaCommand
} from "../../../../../repositories/auto-voucher-formula/commands/update-auto-voucher-formula-command";
import {AccountHead} from "../../../../../entities/account-head";
import {GetAccountHeadsQuery} from "../../../../../repositories/account-head/queries/get-account-heads-query";
import {forkJoin, Observable} from "rxjs";
import {
  GetDatabaseMetadataQuery
} from "../../../../../../admin/repositories/database-metadata/queries/get-database-metadata-query";
import {SearchQuery} from "../../../../../../../shared/services/search/models/search-query";
import {DatabaseMetadata} from "../../../../../../admin/entities/DatabaseMetadata";
import {map, startWith} from "rxjs/operators";
import {MatAutocomplete} from "@angular/material/autocomplete";
import {enableDebugTools} from "@angular/platform-browser";

@Component({
  selector: 'app-auto-voucher-formula-dialog',
  templateUrl: './auto-voucher-formula-dialog.component.html',
  styleUrls: ['./auto-voucher-formula-dialog.component.scss']
})
export class AutoVoucherFormulaDialogComponent extends BaseComponent {
  entity!: AutoVoucherFormula;
  pageModes = PageModes;


  accountHeads: AccountHead[] = [];
  debitCreditStatusTypes: any[] = [];

  voucherTypeTableName!: number;
  voucherTypeId!: number;
  sourceVoucherTypeTableName!: number;
  sourceVoucherTypeId!: number;

  databaseMetadata: DatabaseMetadata[] = [];
  filteredDatabaseMetadata: Observable<DatabaseMetadata[]> = new Observable<DatabaseMetadata[]>();


  formulaChips: any[] = [];




  constructor(private _mediator: Mediator,
              private dialogRef: MatDialogRef<AutoVoucherFormulaDialogComponent>,
              @Inject(MAT_DIALOG_DATA) data: any
  ) {
    super();
    this.entity = data.entity;
    this.pageMode = data.pageMode;
    this.sourceVoucherTypeTableName = data.sourceVoucherTypeTableName;
    this.voucherTypeTableName = data.voucherTypeTableName;
    this.voucherTypeId = data.voucherTypeId;
    this.sourceVoucherTypeId = data.sourceVoucherTypeId;

  }



  async ngOnInit() {
    await this.resolve()
  }

  async resolve() {
    let databaseMetadataRequest = new GetDatabaseMetadataQuery(0, 0, [
      new SearchQuery({
        propertyName: "TableName",
        comparison: "in",
        nextOperand: 'or',
        values: [this.sourceVoucherTypeTableName,this.voucherTypeTableName]
      })
    ]);

    forkJoin([
      this._mediator.send(databaseMetadataRequest),
      this._mediator.send(new GetAccountHeadsQuery())
    ]).subscribe(async ([
                    voucherTypeDatabaseMetadata,
                    accountHeads
                  ]) => {
      this.databaseMetadata = voucherTypeDatabaseMetadata.data;
      this.accountHeads = accountHeads;



      this.debitCreditStatusTypes = [
        {
          id: 1,
          title: 'بدهکار'
        },
        {
          id: 2,
          title: 'بستانکار'
        }
      ]

      await this.initialize()
    })
  }

  async initialize() {
    if (this.pageMode === PageModes.Add) {
      let request = new CreateAutoVoucherFormulaCommand();
      request.voucherTypeId = this.voucherTypeId;
      request.sourceVoucherTypeId = this.sourceVoucherTypeId
      this.request = request;
    }
    if (this.pageMode === PageModes.Update) {
      this.request = new UpdateAutoVoucherFormulaCommand().mapFrom(this.entity);
    }


  }

  async add(param?: any) {
    await this._mediator.send(<CreateAutoVoucherFormulaCommand>this.request).then(res => {
      this.dialogRef.close(res)
    })
  }

  async update(param?: any) {
    await this._mediator.send(<UpdateAutoVoucherFormulaCommand>this.request).then(res => {
      this.dialogRef.close(res)
    })
  }



  close(): any {
  }

  delete(param?: any): any {
  }

  get(param?: any): any {
  }


}
