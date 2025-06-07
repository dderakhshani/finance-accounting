import {Component, Inject} from '@angular/core';
import {BaseComponent} from 'src/app/core/abstraction/base.component';
import {
  TableConfigurations
} from "../../../../../../core/components/custom/table/models/table-configurations";
import {Audit} from "../../../../entities/audit";
import {MAT_DIALOG_DATA, MatDialogRef} from "@angular/material/dialog";
import {Mediator} from "../../../../../../core/services/mediator/mediator.service";
import {DatabaseMetadata} from "../../../../entities/DatabaseMetadata";
import {GetDatabaseMetadataQuery} from "../../../../repositories/database-metadata/queries/get-database-metadata-query";
import {SearchQuery} from "../../../../../../shared/services/search/models/search-query";
import {AuditValueChanges} from "../../../../entities/audit-value-changes";
import {TableColumnFilter} from "../../../../../../core/components/custom/table/models/table-column-filter";
import {TableColumnDataType} from "../../../../../../core/components/custom/table/models/table-column-data-type";
import {TableColumnFilterTypes} from "../../../../../../core/components/custom/table/models/table-column-filter-types";
import {TableOptions} from "../../../../../../core/components/custom/table/models/table-options";
import {TableColumn} from "../../../../../../core/components/custom/table/models/table-column";


@Component({
  selector: 'app-audit-dialog',
  templateUrl: './audit-dialog.component.html',
  styleUrls: ['./audit-dialog.component.scss']
})
export class AuditDialogComponent extends BaseComponent {
  entity!: Audit;
  tableConfigurations!: TableConfigurations;
  databaseMetadata:DatabaseMetadata[] = [];
  constructor(
    private _mediator:Mediator,
    public dialogRef: MatDialogRef<AuditDialogComponent>,
    @Inject(MAT_DIALOG_DATA) data: any
  ) {
    super()
    this.entity = data.entity
  }

  async ngOnInit() {
    await this.resolve()
  }


  async resolve() {
    let searchQuery :SearchQuery = new SearchQuery({
      comparison: 'equals',
      propertyName:'TableName',
      values:[this.entity.tableName]
    })
    await this._mediator.send(new GetDatabaseMetadataQuery(0,0,[searchQuery])).then(res => {
      this.databaseMetadata = res.data
    })
    let tableColumns = [
      new TableColumn('index', 'ردیف', TableColumnDataType.Index, '2%'),
      new TableColumn(
        'title',
        'اسم فیلد',
        TableColumnDataType.Text,
        '2.5%',
        false,
        undefined,
        (item:AuditValueChanges) => {
          return this.databaseMetadata.find(x => x.property.toLowerCase() === item.title.toLowerCase())?.translated ?? ''
        }
      ),
      new TableColumn(
        'old',
        'مقدار قبلی',
        TableColumnDataType.Text,
        '2%',
        false,
        undefined,

      ),
      new TableColumn(
        'new',
        'مقدار جدید',
        TableColumnDataType.Text,
        '2%',
        false,
        undefined,

      ),

    ];
    this.tableConfigurations = new TableConfigurations(tableColumns, new TableOptions(false, true));




  }


  get(param?: any): any {
  }


  add(param?: any): any {
  }

  close(): any {
  }

  delete(param?: any): any {
  }
  initialize(params?: any): any {
  }
  update(param?: any): any {
  }

}

