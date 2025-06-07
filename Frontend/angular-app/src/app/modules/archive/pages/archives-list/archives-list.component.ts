import {AfterViewInit, ChangeDetectorRef, Component, TemplateRef, ViewChild} from '@angular/core';
import {Mediator} from "../../../../core/services/mediator/mediator.service";
import {MatDialog, MatDialogConfig} from "@angular/material/dialog";
import {PageModes} from "../../../../core/enums/page-modes";
import {BaseValueType} from "../../../admin/entities/base-value-type";
import {GetBaseValueTypesQuery} from "../../../admin/repositories/base-value-type/queries/get-base-value-types-query";
import {
  BaseValueTypeDialogComponent
} from "../../../admin/pages/base-value-types/base-value-type-dialog/base-value-type-dialog.component";
import {SearchQuery} from "../../../../shared/services/search/models/search-query";
import {BaseTable} from "../../../../core/abstraction/base-table";
import {TableColumnDataType} from "../../../../core/components/custom/table/models/table-column-data-type";
import {TableColumnFilter} from "../../../../core/components/custom/table/models/table-column-filter";
import {TableColumnFilterTypes} from "../../../../core/components/custom/table/models/table-column-filter-types";
import {TypeFilterOptions} from "../../../../core/components/custom/table/models/column";
import {
  TableScrollingConfigurations
} from "../../../../core/components/custom/table/models/table-scrolling-configurations";
import {TableOptions} from "../../../../core/components/custom/table/models/table-options";
import {TablePaginationOptions} from "../../../../core/components/custom/table/models/table-pagination-options";
import {PrintOptions} from "../../../../core/components/custom/table/models/print_options";

import {AddArchiveDialogComponent} from "../dialog/add-archive-dialog/add-archive-dialog.component";
import {
  GetBaseValuesByUniqueNameQuery
} from "../../../admin/repositories/base-value/queries/get-base-values-by-unique-name-query";
import {GetAttachmentQuery} from "../../repositories/queries/get-attachment-query";
import {
  ConfirmDialogComponent,
  ConfirmDialogIcons
} from "../../../../core/components/material-design/confirm-dialog/confirm-dialog.component";

import {DeleteAttachmentCommand} from "../../repositories/commands/delete-attachment-command";
import {GetBaseValuesQuery} from "../../../admin/repositories/base-value/queries/get-base-values-query";
import {Toastr_Service} from "../../../../shared/services/toastrService/toastr_.service";
import {Router} from "@angular/router";
import {DecimalFormat} from "../../../../core/components/custom/table/models/decimal-format";
import {BaseValueDialogComponent} from "../../../admin/pages/base-values/base-value-dialog/base-value-dialog.component";
import {BaseValue} from "../../../admin/entities/base-value";
import {PreDefinedActions} from "../../../../core/components/custom/action-bar/action-bar.component";
import {GetArchivesQuery} from "../../repositories/archives/get-archives-query";
import {AddAttachmentDialogComponent} from "../dialog/add-attachment-dialog/add-attachment-dialog.component";
import {Archive} from "../../entities/archive";

@Component({
  selector: 'app-archives-list',
  templateUrl: './archives-list.component.html',
  styleUrls: ['./archives-list.component.scss']
})
export class ArchivesListComponent extends BaseTable implements AfterViewInit {
  @ViewChild('actionsButtons', {read: TemplateRef}) actionsButtons!: TemplateRef<any>;
  @ViewChild('expandRowTable', {read: TemplateRef}) expandRowTable!: TemplateRef<any>;

  baseValueIds: any[] = [];
  path: any[] = [];
  activeBaseValueType: BaseValueType = {
    groupName: "B",
    id: 365,
    isReadOnly: false,
    levelCode: "0056",
    subSystem: "بایگانی",
    title: "بایگانی",
    uniqueName: "Archive",
    parentId: 0
  };
  activeArchiveId = 0;
  baseValueTypes: BaseValueType[] = [];
  baseValues: BaseValue[] = [];

  constructor(
    private _mediator: Mediator,
    private cdr: ChangeDetectorRef,
    public dialog: MatDialog,
    private toastr: Toastr_Service,
    private router: Router,
  ) {
    super();
  }

  async ngOnInit() {
    await this.resolve();
  }

  ngAfterViewInit(): void {
    this.actionBar.actions = [
      PreDefinedActions.add(),
      PreDefinedActions.edit(),
    ]

    const column = this.tableConfigurations.columns.find((col: any) => col.field === 'actionsButtons');
    if (column) {
      column.template = this.actionsButtons;
    }
    const columnEexpand = this.tableConfigurations.columns.find((col: any) => col.field === 'expandRowTableAttachment');

    if (columnEexpand) {
      columnEexpand.expandRowWithTemplate = this.expandRowTable;
    }
    this.cdr.detectChanges();
  }

  get() {

  }

  async resolve() {
    this.columns = [
      {
        ...this.defaultColumnSettings,
        index: 0,
        field: 'selected',
        title: '#',
        width: 1,
        type: TableColumnDataType.Select,
        isDisableDrop: true,
        lineStyle: 'onlyShowFirstLine',
        digitsInfo: DecimalFormat.Default,
      },
      {
        ...this.defaultColumnSettings,
        index: 2,
        field: 'rowIndex',
        title: 'ردیف',
        width: 1,
        type: TableColumnDataType.Index,
        isDisableDrop: true,
        lineStyle: 'onlyShowFirstLine',
      },

      {
        ...this.defaultColumnSettings,
        index: 3,
        field: 'fileNumber',
        title: 'شماره بایگانی',
        width: 3.5,
        type: TableColumnDataType.Text,
        // filter: new TableColumnFilter('code', TableColumnFilterTypes.Text),
        // typeFilterOptions: TypeFilterOptions.TextInputSearch,
        lineStyle: 'onlyShowFirstLine',
        sortable: false
      },
      {
        ...this.defaultColumnSettings,
        index: 1,
        field: 'expandRowTableAttachment',
        title: '',
        width: 1,
        type: TableColumnDataType.ExpandRowWithTemplate,
        display: true,
        lineStyle: 'onlyShowFirstLine',
        expandRowWithTemplate: this.expandRowTable,
        sortable: false

      },
      {
        ...this.defaultColumnSettings,
        index: 4,
        field: 'title',
        title: 'عنوان',
        width: 3,
        type: TableColumnDataType.Text,
        lineStyle: 'onlyShowFirstLine',
        // filter: new TableColumnFilter('title', TableColumnFilterTypes.Text),
        // typeFilterOptions: TypeFilterOptions.TextInputSearch,
        sortable: false
      },
      {
        ...this.defaultColumnSettings,
        index: 5,
        field: 'typeBaseTitle',
        title: 'نوع آرشیو',
        width: 3.5,
        type: TableColumnDataType.Text,
        lineStyle: 'onlyShowFirstLine',
        // filter: new TableColumnFilter('uniqueName', TableColumnFilterTypes.Text),
        // typeFilterOptions: TypeFilterOptions.TextInputSearch,
        sortable: false
      }
      ,
      {
        ...this.defaultColumnSettings,
        index: 6,
        field: 'keyWords',
        title: 'کلمات کلیدی',
        width: 3.5,
        type: TableColumnDataType.Text,
        lineStyle: 'onlyShowFirstLine',
        // filter: new TableColumnFilter('value', TableColumnFilterTypes.Text),
        // typeFilterOptions: TypeFilterOptions.TextInputSearch,
        sortable: false
      }
      ,
      {
        ...this.defaultColumnSettings,
        index: 7,
        field: 'description',
        title: 'توضیحات',
        width: 3.5,
        type: TableColumnDataType.Text,
        sortable: false,
        lineStyle: 'onlyShowFirstLine',
      },
      {
        ...this.defaultColumnSettings,
        index: 8,
        field: 'actionsButtons',
        title: 'عملیات',
        width: 3.5,
        type: TableColumnDataType.Template,
        sortable: false,
        lineStyle: 'onlyShowFirstLine',
        template: this.actionsButtons,
        print: false
      }];
    this.tableConfigurations = new TableScrollingConfigurations(this.columns, new TableOptions(false, true), new TablePaginationOptions(), this.toolBar, new PrintOptions('گزارش بایگانی'));
    this.tableConfigurations.options.usePagination = true;
    this.tableConfigurations.options.showTopSettingMenu = true;
    this.tableConfigurations.options.exportOptions.showExportButton = true;

    await this.getBaseValueTypes()
  }

  async getBaseValueTypes() {
    let searchQueries: SearchQuery[] = [
      {
        propertyName: 'levelCode',
        values: ['0056'],
        comparison: 'contains',
        nextOperand: 'and'

      }
    ]
    await this._mediator.send(new GetBaseValueTypesQuery(0, 0, searchQueries, "id ASC")).then(res => {
      this.baseValueTypes = res.data.map(item => {
        if (item.uniqueName === 'Archive') {
          this.activeBaseValueType = item;
          return {...item, canAdd: true};
        }
        return item;
      });
    });
  }

  addBaseValueType(baseValueType: any) {
    let dialogConfig = new MatDialogConfig();
    dialogConfig.data = {
      baseValueType: baseValueType,
      pageMode: PageModes.Add
    };
    let dialogReference = this.dialog.open(BaseValueTypeDialogComponent, dialogConfig);
    dialogReference.afterClosed().subscribe(async ({response, pageMode}) => {
      if (response && pageMode === PageModes.Add) {
        await this.getBaseValueTypes()
      }
    })
  }

  async updateBaseValueType(baseValueType: any) {

    let dialogConfig = new MatDialogConfig();
    dialogConfig.data = {
      baseValueType: baseValueType,
      pageMode: PageModes.Update
    };
    let dialogReference = this.dialog.open(BaseValueTypeDialogComponent, dialogConfig);
    dialogReference.afterClosed().subscribe(async ({response, pageMode}) => {
      if (response) {
        await this.getBaseValueTypes()
        if (pageMode === PageModes.Update) {
          let baseValueTypeToUpdate = this.baseValueTypes.find(x => x.id === response.id)
          if (baseValueTypeToUpdate) {
            Object.keys(response).forEach((key: string) => {
              // @ts-ignore
              baseValueTypeToUpdate[key] = response[key]
            })
          }
        } else if (pageMode === PageModes.Delete) {
          let baseValueTypeToRemove = this.baseValueTypes.find(x => x.id === response.id)
          if (baseValueTypeToRemove) {
            this.baseValueTypes.splice(this.baseValueTypes.indexOf(baseValueTypeToRemove), 1)
            this.baseValueTypes = [...this.baseValueTypes]
          }
        }
      }
    })
  }


  addArchive() {
    let keywords: string[] = [];
    let basevalueType: any = this.activeBaseValueType;
    if (!basevalueType.parentId) keywords.unshift(basevalueType.title);
    while (basevalueType.parentId) {
      keywords.unshift(basevalueType.title)
      basevalueType = this.baseValueTypes.find(x => x.id === basevalueType.parentId);
      if (!basevalueType.parentId) keywords.unshift(basevalueType.title);
    }
    let dialogConfig = new MatDialogConfig();
    dialogConfig.data = {
      baseValueTypeId: this.activeBaseValueType.id,
      keywords: keywords,
      code: this.activeBaseValueType.groupName,
      pageMode: PageModes.Add
    };
    let dialogReference = this.dialog.open(AddArchiveDialogComponent, dialogConfig);
    dialogReference.afterClosed().subscribe(async (res) => {
      if(res === true)await this.getArchivesByTypeBaseId(this.activeBaseValueType.id)
    })
  }

  updateArchive(archive?: Archive) {
    let archiveToUpdate = archive ?? this.rowData.filter(x => x.selected === true)[0]
    console.log(archiveToUpdate)
    let dialogConfig = new MatDialogConfig();
    dialogConfig.data = {
      archive: archiveToUpdate,
      pageMode: PageModes.Update
    };
    let dialogReference = this.dialog.open(AddArchiveDialogComponent, dialogConfig);
    dialogReference.afterClosed().subscribe(async (res) => {
      if(res === true)await this.getArchivesByTypeBaseId(this.activeBaseValueType.id)

    })
  }

  async getArchivesByTypeBaseId(typeBaseId: number) {
    this.tableConfigurations.options.isLoadingTable = true;
    await this._mediator.send(new GetArchivesQuery(typeBaseId)).then((res: any) => {
      this.rowData = res;
      this.tableConfigurations.options.isLoadingTable = false;
    }).catch(() => {
      this.tableConfigurations.options.isLoadingTable = false;
    });
  }


  async getAttachment(id: any) {
    // if (!id)
    //   this.subRowData = []
    // let searchQueries: SearchQuery[] =
    //   [
    //     {
    //       propertyName: 'typeBaseId',
    //       values: [id],
    //       comparison: 'in',
    //       nextOperand: 'and'
    //     }
    //   ]
    // this.isLoadingSubTable = true;
    // await this._mediator.send(new GetAttachmentQuery(0, 0, searchQueries)).then(res => {
    //     this.subRowData = res.data
    //     this.isLoadingSubTable = false;
    //   },
    // ).catch(() => {
    //   this.isLoadingSubTable = false;
    // });

  }

  async getAllBaseValue(baseValueTypeId: Number) {
    let searchQueries: SearchQuery[] =
      [
        {
          propertyName: 'baseValueTypeId',
          values: [baseValueTypeId],
          comparison: 'in',
          nextOperand: 'and'
        }
      ]
    await this._mediator.send(new GetBaseValuesQuery(0, 0, searchQueries)).then(res => {
      this.baseValueIds = res
    });
  }


  add() {


  }

  update(entity?: BaseValue) {

  }

  delete() {

  }

  async initialize() {

  }


  close() {
  }

  async addAttachment(row: any) {
    let dialogConfig = new MatDialogConfig();

    if (row) {
      dialogConfig.data = {
        pageMode: PageModes.Add,
        archiveId: row.id,
        keyWords: row.keyWords,
        fileNumber: row.fileNumber,
        typeBaseId: row.typeBaseId,
        description: row.description,
        typeBaseTitle: row.typeBaseTitle
      };
      let dialogReference = this.dialog.open(AddAttachmentDialogComponent, dialogConfig);
      dialogReference.afterClosed().subscribe(async ({}) => {
        await this.getArchivesByTypeBaseId(this.activeBaseValueType.id)
      })
    } else {
      this.toastr.showToast({message: 'برای این مورد نوع اطلاعات پایه ای ثبت نشده است', type: 'warning'});
    }
  }

  async updateAttachment(row: any) {
    let dialogConfig = new MatDialogConfig();
    dialogConfig.data = {
      pageMode: PageModes.Update,
      attachments: {
        id: row.id,
        title: row.title,
        description: row.description,
        keyWords: row.keyWords,
        fileNumber: row.fileNumber,
        typeBaseId: row.typeBaseId,
        url: row.url
      },
      files: [
        {
          file: null,
          progressStatus: '',
          progress: 0,
          filePath: row.url,
          fullFilePath: row.url,
          extention: row.extention,
          id: row.id
        }
      ]
    };

    let dialogReference = this.dialog.open(AddAttachmentDialogComponent, dialogConfig);
    dialogReference.afterClosed().subscribe(async ({}) => {
      await this.getAttachment(row.typeBaseId)
    })
  }

  deleteAttachment(attachment: any) {
    let attachmentId = attachment.id;
    let typeBaseId = attachment.typeBaseId
    console.log(attachment)

    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      data: {
        title: 'حذف سند بایگانی',
        message: 'آیا از حذف سند بایگانی اطمینان دارید؟',
        icon: ConfirmDialogIcons.warning,
        actions: {
          confirm: {title: 'بله', show: true}, cancel: {title: 'خیر', show: true}
        }
      }
    });


    dialogRef.afterClosed().subscribe(async result => {
      if (result == true) {

        let request = new DeleteAttachmentCommand(attachmentId, this.activeArchiveId);

        await this._mediator.send(request)
          .then((response) => {
            this.getArchivesByTypeBaseId(this.activeBaseValueType.id)
          }).catch(() => {

          });
      }
    });

  }

  async handleClickPath(event: any) {
    this.activeBaseValueType = event;
    await this.getArchivesByTypeBaseId(event.id);
  }


  async handleExpandedRowIndex(row: any) {
    if (!row) return;

    this.subRowData = row.attachments
    this.activeArchiveId = row.id
  }
}
