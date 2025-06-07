import { Component, HostListener, ViewChild } from "@angular/core";
import { BaseComponent } from "../../../core/abstraction/base.component";
import { SearchQuery } from "../../../shared/services/search/models/search-query";
import { TableFilterComponent } from "../pages/component/table-filter/table-filter.component";
import { tablePaggingComponent } from "../pages/component/table-pagging/table-pagging.component";
import { ServiceLocator } from "../../../core/services/service-locator/service-locator";
import { IdentityService } from "../../identity/repositories/identity.service";
@Component({
  template: ''
})
export abstract class BaseSetting extends BaseComponent {



  ngOnInit(params?: any): void {
    throw new Error("Method not implemented.");
  }
  resolve(params?: any) {
    throw new Error("Method not implemented.");
  }
  initialize(params?: any) {
    throw new Error("Method not implemented.");
  }
  add(param?: any) {
    throw new Error("Method not implemented.");
  }
  get(param?: any) {
    throw new Error("Method not implemented.");
  }
  update(param?: any) {
    throw new Error("Method not implemented.");
  }
  delete(param?: any) {
    throw new Error("Method not implemented.");
  }
  close() {
    throw new Error("Method not implemented.");
  }
  abstract exportexcel(data: any[]): any;

  abstract CalculateSum(param?: any): any;
  //--------------------*******************************************
  //===============================================================


  @HostListener("click", ["$event"]) onClick(event: any) {

    var value = event.target.closest(`table`);

    if (!value?.classList.contains('mas-table')) {
      return
    }
    if (!event.target.matches('th')) {

      return
    }
    var accessKey: any = event.target.getAttribute("accessKey");
    if (accessKey) {
      var dir = event.target.getAttribute('sort-data') || 'asc';
      event.target.setAttribute("sort-data", dir == 'asc' ? 'desc' : 'asc');
      event.target.classList.remove(dir == 'asc' ? 'triangle-down' : 'triangle-up');
      event.target.classList.add(dir == 'asc' ? 'triangle-up' : 'triangle-down');


      this.sortFunction(accessKey, dir == 'asc' ? true : false)
    }
    else {


      var parentNode: any = event.target.parentElement;

      var classList = parentNode.classList

      if (!classList.contains('background-primary-50')) {
        parentNode.classList.add('background-primary-50');
      }
      else {
        parentNode.classList.remove("background-primary-50");
      }


    }


  }

  public IslargeSize: boolean = false;
  public currentPage: number = 1;
  public searchQueries: SearchQuery[] = []
  public RowsCount: number = 0;
  public Reports_filter: any[] = [];
  public data: any[] = [];

  public SearchForm!: any;
  booleanValue: any = false;
  dataLength : number = 0
  pageSize = 100;
  // hidden footer
  isDisplayFooter: boolean = true;




  public childTable: any;
  @ViewChild(TableFilterComponent)
  set appShark(child: TableFilterComponent) {
    this.childTable = child
  };

  public childPagging: any;
  @ViewChild(tablePaggingComponent)
  set appPagging(child: tablePaggingComponent) {
    this.childPagging = child
  };
  public async ExportAllToExcel() {

    let currentPage = this.currentPage
    this.currentPage = 0;
    this.pageSize = 0;

    await this.get()
    this.currentPage = currentPage;
    this.pageSize = 100;

  }
  public ChangePage(pageMumber: number) {

    if (this.currentPage != pageMumber) {
      this.currentPage = pageMumber;
      this.get();

    }

  }

  public onGet(_searchQueries: SearchQuery[] = []) {
    this.searchQueries = _searchQueries;
    this.currentPage = 1;
    this.get(this.searchQueries);

  }

  sortFunction(colName: string, boolean: boolean) {
    if (boolean == true) {
      this.data.sort((a, b) => a[colName] < b[colName] ? 1 : a[colName] > b[colName] ? -1 : 0)
      this.booleanValue = !this.booleanValue
    }
    else {
      this.data.sort((a, b) => a[colName] > b[colName] ? 1 : a[colName] < b[colName] ? -1 : 0)
      this.booleanValue = !this.booleanValue
    }
  }

  GetChildFilter(searchQueries: SearchQuery[] = []) {
    searchQueries = [];

    if (this.childTable && this.childTable.fillFilter().length > 0) {

      searchQueries = this.childTable._searchQueries;
    }
    return searchQueries;
  }

  ondeleteFilter(form: any) {
    this.currentPage = 1;
    form.reset();
    let identityService = ServiceLocator.injector.get(IdentityService);
    form.controls?.toDate?.setValue(new Date(identityService.getActiveYearlastDate()));

    
    form.controls?.fromDate?.setValue((new Date(identityService.getActiveYearStartDate())));

    this.childTable.delete();

  }
  gePageSize() {
    let pageSize = this.childPagging?.pageSize ? this.childPagging?.pageSize : this.pageSize;
    if (this.currentPage == 0) {
      pageSize = 0;
    }
    return pageSize;
  }
}
