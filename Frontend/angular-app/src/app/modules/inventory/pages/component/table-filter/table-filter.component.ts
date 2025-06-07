
import { EventEmitter, OnChanges, Output, SimpleChanges } from '@angular/core';
import { Component, Input } from '@angular/core';
import { Mediator } from '../../../../../core/services/mediator/mediator.service';
import { NotificationService } from '../../../../../shared/services/notification/notification.service';
import { PagesCommonService } from '../../../../../shared/services/pages/pages-common.service';
import { FormControl, FormGroup } from '@angular/forms';
import { SearchQuery } from '../../../../../shared/services/search/models/search-query';
import { BaseComponent } from '../../../../../core/abstraction/base.component';
import { ActivatedRoute, Router } from '@angular/router';
import { DomSanitizer } from '@angular/platform-browser';
import * as XLSX from 'xlsx';
import { timeout } from 'rxjs/operators';
@Component({
  selector: 'app-table-filter',
  templateUrl: './table-filter.component.html',
  styleUrls: ['./table-filter.component.scss']
})
export class TableFilterComponent extends BaseComponent {

  selectedOperands: string = 'contains'
  KeyUrl = window.location.href.split('?')[0];
  @Input() IsShowFilter: boolean = false;
  @Input() IsShowFilterPro_btn: boolean = true;
  @Input() IsShowFilter_ColumnSelection: boolean = true;
  @Input() data: any[] = [];

  @Output() GetQuery = new EventEmitter<any>();
  @Output() IslargeSize = new EventEmitter<boolean>();

  @Output() onPrint = new EventEmitter<any>();
  @Output() filterTable = new EventEmitter<any>();
  @Output() isSelectedRows = new EventEmitter<boolean>();


  public columns: any[] = [];
  SearchInput: string = "";
  SearchQuereis: any[] = [];
  _searchQueries: SearchQuery[] = [];
  IsOpenFilterModal: boolean = false;
  IsOpenColumnModal: boolean = false;
  IsVisibleRows: boolean = false;
  largeSize: boolean = false;
  filterCount: number = 0;
  columnsCount: number = 0;
  data_Filter: any[] = [];
  data_filde: any;


  nextOperand: any = [{ value: '1', title: ' و', selected: true }, { value: '2', title: ' یا', selected: false }
  ]

  TableColumnFilterOperands: any = [
    {
      title: 'مشمول بر',
      value: 'contains',
      dataTypes: ['string', 'text']
    },

    {
      title: 'مساوی با',
      value: 'equal',
      dataTypes: ['string', 'text', 'number', 'int', 'money', 'date', 'datetime']

    },
    {
      title: 'نا مساوی با',
      value: 'notEqual',
      dataTypes: ['string', 'text', 'number', 'int', 'money', 'date', 'datetime']
    },
    {
      title: 'نامشتمل بر',
      value: 'notContains',
      dataTypes: ['string', 'text']
    },

    {
      title: 'بزرگتر از',
      value: 'greaterThan',
      dataTypes: ['number', 'int', 'money', 'date', 'datetime']
    },
    {
      title: 'کوچکتر از',
      value: 'lessThan',
      dataTypes: ['number', 'int', 'money', 'date', 'datetime']
    },
    {
      title: 'شروع با ',
      value: 'startsWith',
      dataTypes: ['string', 'text']
    },

  ]
  FilterOperands: any[] = this.TableColumnFilterOperands;
  filterType: string = '';
  constructor(
    private router: Router,
    public _mediator: Mediator,
    private route: ActivatedRoute,
    private sanitizer: DomSanitizer,
    public Service: PagesCommonService,
    public _notificationService: NotificationService,

  ) {
    super(route, router);
    this.isSelectedRows.emit(false);
  }
  ngOnChanges(changes: SimpleChanges) {


    this.data_Filter = this.data;
    this.onfilterTable();
      setTimeout(() => {
        this.VisibleColumnModal();
        this.onfilterTable();
      }, 2);




  }



  async ngOnInit() {

    this.resolve();
  }

  async resolve() {
    this.OpenFilterTable()
  }
  async initialize() {

  }

  filterComparison(SearchQuereis: any) {
    SearchQuereis.filterType = this.columns.find(a => a.name == SearchQuereis.propertyName)?.type

    if (SearchQuereis.filterType != 'string' && SearchQuereis.filterType != 'text') {
      this.TableColumnFilterOperands = this.FilterOperands.filter(a => a.value != 'contains' && a.value != 'notContains')

      this.selectedOperands = 'equal'
    }
    else {
      this.TableColumnFilterOperands = this.FilterOperands;
      this.selectedOperands = 'contains'
    }
  }

  add() {
    let notValid = 0
    if (this.SearchQuereis.length > 0) {
      this.SearchQuereis.forEach(a => {
        if (!(a.propertyName != '' && a.comparison != '')) {
          notValid = 1;

        }

      })
    }
    if (notValid == 1) {
      this.Service.showWarrningMessage('اطلاعات مربوط به جستجو را به صورت کامل و صحیح وارد نمایید')
      return;
    }
    this.SearchQuereis.push({
      propertyName: "",
      values: [],
      comparison: 'contains',
      nextOperand: '1',
      filterType: "text",

    }
    )
  }

  deleteItem(SearchQuery: any) {
    this.SearchQuereis = this.SearchQuereis.filter(a => !(a.comparison == SearchQuery.comparison && a.propertyName == SearchQuery.propertyName && a.nextOperand == SearchQuery.nextOperand && a.values == SearchQuery.values))
    this._searchQueries = this.SearchQuereis.filter(a => !(a.comparison == SearchQuery.comparison && a.propertyName == SearchQuery.propertyName && a.values == SearchQuery.values))
    this.filterCount = this.SearchQuereis.length;
  }
  delete() {
    this.SearchQuereis = [];
    this._searchQueries = [];
    this.filterCount = this.SearchQuereis.length;
    this.GetQuery.emit(this._searchQueries);

    this.IsOpenFilterModal = false
  }

  filter(buttonName = 'search') {

    this._searchQueries=this.fillFilter();

    if (this.SearchQuereis.length != this._searchQueries.length && buttonName != 'refresh') {
      this.Service.showWarrningMessage('اطلاعات مربوط به جستجو را به صورت کامل و صحیح وارد نمایید')
      return
    }

      this.GetQuery.emit(this._searchQueries);

    if (buttonName != 'refresh') {
      this.filterCount = this.SearchQuereis.length
    }


    this.IsOpenFilterModal = false
  }
  fillFilter() {
    this._searchQueries = [];

    this.SearchQuereis.forEach(a => {
      if (a.propertyName != '' && a.comparison != '')
        this._searchQueries.push(new SearchQuery({
          propertyName: a.propertyName,
          values: [a.values],
          comparison: a.comparison,
          nextOperand: a.nextOperand == '2' ? 'or' : 'and'
        }))
    })
    return this._searchQueries;
  }

  async exportExcel() {
    var fileName = 'ExcelSheet.xlsx';
    /* pass here the table id */
    let element = await this.Service.GetMasTableHtml('mas-table','excel');
    element = element.cloneNode(true)
    //  delete element footer
    element.querySelector('tfoot').remove();
    const ws: XLSX.WorkSheet = XLSX.utils.table_to_sheet(element);
    /* generate workbook and add the worksheet */
    const wb: XLSX.WorkBook = XLSX.utils.book_new();
    XLSX.utils.book_append_sheet(wb, ws, 'Sheet1');
    /* save to file */
    XLSX.writeFile(wb, fileName);

  }
  async FilterTable() {

    var filter: string = this.SearchInput.toLowerCase();
    if (this.data.length > 0 && filter) {
      var keys = Object.keys(this.data[0]);
      if (keys.filter(x => x.toLowerCase().includes(filter))?.length > 0) {
        return [];
      }
    }

    var filteredData = this.data.filter(x => {
      var doesIncludeSearchValue = JSON.stringify(x)?.toLowerCase()?.includes(filter)
      if (doesIncludeSearchValue) return x;
    })
    return filteredData;
  }
  onfilterTable() {
    this.FilterTable().then(result => {
      this.filterTable.emit(result);
    })
  }

  print() {
    this.onPrint.emit();
  }
  onSearch() {
    this.IsOpenFilterModal = !this.IsOpenFilterModal;

    /*this.OpenFilterTable().then(result => {*/
    if (this.SearchQuereis.length == 0) {
      this.add();
    }
    /* });*/

  }

  onColumnSettings() {
    this.IsOpenColumnModal = !this.IsOpenColumnModal;

    /*this.OpenFilterTable().then(result => {*/
    if (this.SearchQuereis.length == 0) {
      this.add();
    }
    /* });*/

  }
  async OpenFilterTable() {

    this.data_filde = await this.Service.getTableHtml();

    var th: any[] = this.data_filde.getElementsByTagName("th");

    this.columns = [];

    for (let i = 0; i < th.length; i++) {

      if (th[i]?.abbr != "" && th[i]?.abbr != undefined) {
        this.columns.push(
          {
            type: th[i]?.abbr,
            name: th[i]?.accessKey,
            caption: th[i].innerText,
            visible: th[i].class != 'hidden' ? true : false

          }
        );
      }
    }
  }
  onIslargeSize() {
    this.largeSize = !this.largeSize
    this.IslargeSize.emit(this.largeSize);
  }
  async onVisibleComfirm() {

    this.IsOpenColumnModal = false

    this.store()
    this.VisibleColumnModal();


  }
  VisibleColumnModal() {
    this.retrieve();
    var th: any[] = this.data_filde.getElementsByTagName("th");
    var cells: any[] = this.data_filde.querySelectorAll("tbody > tr > td");
    var tfoot: any[] = this.data_filde.querySelectorAll("tfoot > tr > td");
    this.VisibleColumn(cells, th)
    this.VisibleColumn(tfoot, th)
  }
  VisibleColumn(cells: any[], th: any[]) {
    for (let i = 0; i < th.length; i++) {
      if (th[i]?.accessKey != "" && th[i]?.accessKey != undefined) {
        this.columns.forEach(res => {

          if (res.name == th[i]?.accessKey) {

            if (res.visible == true) {
              th[i].classList.add('block');
              th[i].classList.remove('hidden');

              for (let j = 0; j < cells.length; j++) {
                if (cells[j].accessKey == res.name) {
                  cells[j].classList.add('block');
                  cells[j].classList.remove('hidden');
                }

              }
            }
            else {

              th[i].classList.add('hidden');
              th[i].classList.remove('block');
              for (let j = 0; j < cells.length; j++) {

                if (cells[j].accessKey == res.name) {
                  cells[j].classList.add('hidden');
                  cells[j].classList.remove('block');

                }

              }

            }
          }
        })
      }
    }
  }
  async onVisibleRows()
  {
    this.IsVisibleRows = !this.IsVisibleRows;
    this.isSelectedRows.emit(this.IsVisibleRows);
    if (this.IsVisibleRows) {
      this.data_filde.querySelectorAll('tbody tr:not(.background-primary-50)').forEach((tr: any) => {
        tr.classList.add('hidden');

      });
      // this.data_filde.querySelectorAll('tfoot').forEach((tr: any) => {
      //
      //   tr.classList.add('hidden');
      // });
    }
    else {
      this.data_filde.querySelectorAll('tbody tr').forEach((tr: any) => {

        var cells: any[] = tr.querySelectorAll("td");


        tr.classList.remove('hidden');

      });
      this.data_filde.querySelectorAll('tfoot').forEach((tr: any) => {
        tr.classList.remove('hidden');


      });
    }



  }

  store() {
    const val = JSON.stringify(this.columns)

    window.localStorage.setItem(this.KeyUrl, val);

  }

  retrieve() {

    const val = localStorage.getItem(this.KeyUrl)
    if (val)
      this.columns = JSON.parse(val)

    this.columnsCount = this.columns.filter(a => a.visible == false).length;
  }


  remove() {

    window.localStorage.removeItem(this.KeyUrl);

  }
  update(param?: any) {
    throw new Error('Method not implemented.');
  }
  close() {
    throw new Error('Method not implemented.');
  }
  get(param?: any) {


  }
}



