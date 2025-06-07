import {Injectable} from '@angular/core';
import {FormGroup} from '@angular/forms';
import {MatSnackBar} from '@angular/material/snack-bar';
import {Data} from '@angular/router';
import {HttpSnackbarComponent} from '../../../core/components/material-design/http-snackbar/http-snackbar.component';
import {ToPersianDatePipe} from '../../../core/pipes/to-persian-date.pipe';
import {Mediator} from '../../../core/services/mediator/mediator.service';
import {IdentityService} from '../../../modules/identity/repositories/identity.service';
import {SearchQuery} from '../search/models/search-query';
import {NotificationService} from '../notification/notification.service';
import {TabManagerService} from '../../../layouts/main-container/tab-manager.service';
import * as XLSX from 'xlsx';
import {Receipt} from '../../../modules/inventory/entities/receipt';
import {
  ConfirmDialogComponent,
  ConfirmDialogIcons
} from "../../../core/components/material-design/confirm-dialog/confirm-dialog.component";
import {MatDialog} from "@angular/material/dialog";

@Injectable({
  providedIn: 'root'
})
export class PagesCommonService {

  constructor(
    private _snackBar: MatSnackBar,
    public _mediator: Mediator,
    public identityService: IdentityService,
    public _notificationService: NotificationService,
    public TabManagerService: TabManagerService,
    public dialog: MatDialog
  ) {

  }


  public ListId: string[] = [];
  public spinner: boolean = false;
  public PageSize = 100;

  //******************************Filter value*****************************
  public readonly PersonalCodeGroup: string = "19"
  public readonly ProviderCodeGroup: string = "12"
  public readonly ReceiptPrintStatus: number = 28596;
  public readonly FromWarehoseId = 30;//آیدی انبار مواد اولیه ، انبار پیش فرض خروجی گه باید باشد.
  public readonly AccessToWarehouse: string = "Warehouses"
  public readonly AccessToWarehouseCodeGroup: string = "CodeVoucherGroups"
  public readonly TimeImportPurchases: number = 8;
  public readonly TimeInternalPurchases: number = 1;

  //----------------------------Attachment-------------------------------
  public AttachmentAssets100: number = 28691
  public AttachmentReceipt100: number = 28692
  //---------------------------------------------------------------------
  public extraCostAccountHeadId: number = 1782
  //----------------خروج کالا و تحویل به فرد یا واحد
  public ViewIdRemoveUnitId = 121
  //----------------خروج کالا و تحویل به انبار دیگر
  public ViewIdRemoveAddWarehouse = 122

  public ViewIdProductTemporaryReceipt = 25
  public ViewIdProductLeave = 65
  //---------------------------------------------------------------------
  public CodeTemporaryReceipt: number = 23;
  public CodeDirectReceipt: number = 33;
  public CodeInvoiceAmountReceipt: number = 43;
  public CodeInvoiceAmountLeave: number = 44;
  public CodeInvoiceAmountStart: number = 45;
  public CodeRegistrationAccountingReceipt: number = 53;
  public CodeRegistrationAccountingLeave: number = 54;
  public CodeRegistrationAccountingStart: number = 55;
  public CodeLeaveReceipt: number = 63;
  public CodeRequestRecive: number = 73;
  public CodeRequestBuy: number = 83;
  public CodeArchiveReceipt: number = 13;
  public CodeArchiveRequest: number = 12;
  public CodeArchiveRequestBuy: number = 11;
  public changeWarehouse: number = 93;
  public Codetransfer: number = 64;


  //ثبت سند مکانیزه رسید انبار قطعات
  public DirectReceiptAutoVoucher: string = "InventoryReceiptAutoVoucher";

  //========================رسید موقت===================================
  // انبار قطعات
  public TemporaryReceipt: string = "UtilitiesTemporaryReceipt";//5023
  //انبار مواد اولیه
  public MaterailInventoryTemporaryReceipt: string = "MaterailTemporaryReceipt";//5123

  //انبار اموال
  public EstateTemporaryReceipt: string = "EstateTemporaryReceipt";//5223

  //انبار مصرفی
  public ConsumptionInventoryTemporaryReceipt: string = "ConsumptionInventoryTemporaryReceipt";//5323
  //انبار امانی
  public loanTemporaryReceipt: string = "loanTemporaryReceipt";//5423
  //مرجوعی
  public ReturnTemporaryReceipt: string = "ReturnTemporaryReceipt";//5523
  //========================رسید مستقیم===================================
  // انبار قطعات
  public UtilitiesDirectReceipt: string = "UtilitiesDirectReceipt";//5033
  //انبار مواد اولیه
  public MaterailDirectReceipt: string = "MaterailDirectReceipt";//5133

  //انبار اموال
  public EstateDirectReceipt: string = "EstateDirectReceipt";//5133

  //انبار مصرفی
  public ConsumptionDirectReceipt: string = "ConsumptionDirectReceipt";//5333

  //انبار مواد نیم ساخته
  public SemiFinishedMaterialsDirectReceip: string = "Semi-finishedMaterialsDirectReceipt";//5334

  //انبار امانی
  public loanDirectReceipt: string = "loanDirectReceipt";//5433
  //مرجوعی
  public ReturnDirectReceipt: string = "ReturnDirectReceipt";//5533
  //========================رسید ریالی===================================
  // انبار قطعات
  public AmountUtilitiesReceipt: string = "AmountUtilitiesReceipt";//5043
  // انبار مواد اولیه
  public AmountMaterailReceipt: string = "AmountMaterailReceipt";//5143

  //انبار اموال
  public AmountEstateReceipt: string = "AmountEstateReceipt";//5143

  //انبار مصرفی
  public AmountConsumptionReceipt: string = "AmountConsumptionReceipt";//5343

  //انبار امانی
  public AmountloanReceipt: string = "AmountloanReceipt";//5443

  //مرجوعی
  public AmountDirectReceipt: string = "AmountDirectReceipt";//5543
  //========================سند افتتاحیه===================================
  // انبار قطعات
  public UtilitiesStartReceipt: string = "UtilitiesStartReceipt";//5045
  // انبار مواد اولیه
  public MaterailInventoryStartReceipt: string = "MaterailInventoryStartReceipt";//5145

  //انبار اموال
  public EstateStartReceipt: string = "EstateStartReceipt";//5145

  //انبار مصرفی
  public ConsumptionInventoryStartReceipt: string = "ConsumptionInventoryStartReceipt";//5345

  //انبار امانی
  public loanStartReceipt: string = "loanStartReceipt";//5445

  //مرجوعی
  public ReturnStartReceipt: string = "ReturnStartReceipt";//5545

  //========================سند کانیزه===================================
  // انبار قطعات
  public AccountingUtilitiesReceipt: string = "AccountingUtilitiesReceipt";//5053
  //انبار مواد اولیه
  public AccountingMaterailReceipt: string = "AccountingMaterailReceipt";//5153

  //انبار اموال
  public AccountingEstateReceipt: string = "AccountingEstateReceipt";//5253

  //انبار مصرفی
  public AccountingConsumptionReceipt: string = "AccountingConsumptionReceipt";//5353

  //مرجوعی
  public AccountingDirectReceipt: string = "AccountingDirectReceipt";//5553

  //========================سند کانیزه افتتاحیه===================================
  // انبار قطعات
  public AccountingUtilitieStart: string = "AccountingUtilitieStart";//5055
  //انبار مواد اولیه
  public AccountingMaterailStart: string = "AccountingMaterailStart";//5155

  //انبار اموال
  public AccountingEstateStart: string = "AccountingEstateStart";//5255

  //انبار مصرفی
  public AccountingConsumptionStart: string = "AccountingConsumptionStart";//5355

  //انبار امانی
  public AccountingloanReceiptStart: string = "AccountingloanReceiptStart";//5455
  //مرجوعی
  public AccountingReturnReceiptStart: string = "AccountingReturnReceiptStart";//5555
  //========================آرشیو===================================
  // انبار قطعات
  public ArchiveUtilitiesReceipt: string = "ArchiveUtilitiesReceipt";//5013
  //انبار مواد اولیه
  public ArchiveMaterailReceipt: string = "ArchiveMaterailReceipt";//5113

  //انبار اموال
  public ArchiveEstateReceipt: string = "ArchiveEstateReceipt";//5213

  //انبار مصرفی
  public ArchiveConsumptionReceipt: string = "ArchiveConsumptionReceipt";//5313

  //انبار امانی
  public ArchiveloanReceipt: string = "ArchiveloanReceipt";//5413

  //مرجوعی
  public ArchiveDirectReceipt: string = "ArchiveDirectReceipt";//5513


  //=================================خروج از انبار========================
  //-سند خروج قطعات کالا
  public RemoveCommodityWarhouse: string = "RemoveUtilitiesWarhouse";//5063
  //خروج کالا از انبار مواد اولیه
  public RemoveMaterialWarhouse: string = "RemoveMaterialWarhouse";//5163
  //خروج کالا از انبار مواد مصرفی
  public RemoveConsumptionWarhouse: string = "RemoveConsumptionWarhouse";//5263
  // خروج کالا از انبار اموال
  public RemoveAssetsWarhouse: string = "RemoveAssetsWarhouse";//5363

  // جابجایی کالا از انبار مواد اولیه
  public ChangeMaterialWarhouse: string = "ChangeMaterialWarhouse";//5193
  //رسید محصول انبار مواد اولیه
  //جابه جایی انبار با فرمول ساخت
  public ProductReceiptWarehouse: string = "ProductReceiptWarehouse";//5194

  //انبار امانی
  public RemoveloanReceipt: string = "RemoveloanReceipt";//5463
  //مرجوعی
  public RemoveReturnReceipt: string = "RemoveReturnReceipt";//5563


  //========================رسید ریالی خروج===================================
  // انبار قطعات
  public AmountUtilitiesLeave: string = "AmountUtilitiesLeave";//5044
  // انبار مواد اولیه
  public AmountMaterailLeave: string = "AmountMaterailLeave";//5144

  //انبار اموال
  public AmountEstateLeave: string = "AmountEstateLeave";//5144

  //انبار مصرفی
  public AmountConsumptionLeave: string = "AmountConsumptionLeave";//5344

  //انبار امانی
  public AmountloanReceiptLeave: string = "AmountloanReceiptLeave";//5444

  //مرجوعی
  public AmountReturnReceipt: string = "AmountReturnReceipt";//5544
  //========================سند کانیزه خروج===================================
  // انبار قطعات
  public AccountingUtilitieLeave: string = "AccountingUtilitieLeave";//5054
  //انبار مواد اولیه
  public AccountingMaterailLeave: string = "AccountingMaterailLeave";//5154

  //انبار اموال
  public AccountingEstateLeave: string = "AccountingEstateLeave";//5254

  //انبار مصرفی
  public AccountingConsumptionLeave: string = "AccountingConsumptionLeave";//5354

  //انبار امانی
  public AccountingloanReceiptLeave: string = "AccountingloanReceiptLeave";//5454
  //مرجوعی
  public AccountingReturnReceipt: string = "AccountingReturnReceipt";//5554
  //=======================درخواست دریافت کالا=============================

  //-درخواست دریافت کالا قطعات
  public RequestReceiveUtilities: string = "RequestReceiveUtilities";//5073
  //انبار مواد اولیه
  public RequestReceiveMaterail: string = "RequestReceiveMaterail";//5173
  //-درخواست دریافت کالا مصرفی
  public RequestReceiveConsumption: string = "RequestReceiveConsumption";//5373
  //-درخواست دریافت کالا اموال
  public RequesReceiveAssets: string = "RequesReceiveAssets";//5273
  //-درخواست دریافت کالا امانی
  public RequesReceiveBorrow: string = "RequesReceiveBorrow";//5473
  //-درخواست دریافت کالا مرجوعی
  public RequesReceiveReturnCommodity: string = "RequesReceiveReturnCommodity";//5573


  //=======================بایگانی درخواست دریافت کالا=============================

  //-درخواست دریافت کالا قطعات
  public ArchiveRequestUtilitiesReceipt: string = "ArchiveRequestUtilitiesReceipt";//5012
  //-درخواست دریافت کالا مصرفی
  public ArchiveRequestMaterailReceipt: string = "ArchiveRequestMaterailReceipt";//5152
  //-درخواست دریافت کالا اموال
  public ArchiveRequestEstateReceipt: string = "ArchiveRequestEstateReceipt";//5212
  //-درخواست دریافت کالا مصرفی
  public ArchiveRequestConsumptionReceipt: string = "ArchiveRequestConsumptionReceipt";//5312
  //-درخواست دریافت کالا امانی
  public ArchiveRequesReceiveBorrow: string = "ArchiveRequesReceiveBorrow";//5412

  //مرجوعی
  public ArchiveRequesReceiveReturnCommodity: string = "ArchiveRequesReceiveReturnCommodity";//5512

  //=======================درخواست خرید کالا=============================

  //- قطعات
  public RequestBuyUtilities: string = "RequestBuyUtilities";//5083
  //مواد اولیه
  public RequestBuyeMaterail: string = "RequestBuyeMaterail";//5183

  //-اموال
  public RequestBuyEstate: string = "RequestBuyEstate";//5283
  //-مصرفی
  public RequestBuyConsumption: string = "RequestBuyConsumption";//5383

  //=======================درخواست خرید کالا بایگانی=============================

  //- قطعات
  public ArchiveRequestBuyUtilitiesReceipt: string = "ArchiveRequestBuyUtilitiesReceipt";//5011
  //مواد اولیه
  public ArchiveRequestBuyMaterailReceipt: string = "ArchiveRequestBuyMaterailReceipt";//5111

  //-اموال
  public ArchiveRequestBuyEstateReceipt: string = "ArchiveRequestBuyEstateReceipt";//5211
  //-مصرفی
  public ArchiveRequestBuyConsumptionReceipt: string = "ArchiveRequestBuyConsumptionReceipt";//5311

  //----------------------------Purchase-------------------------------------
  //==============================خرید=======================================
  //ثبت قرارداد
  public ContractVoucherGroup: string = "RegistertheContract";
  //ثبت پیش فاکتور
  public PreInvoice: string = "RegistertheFactor";
  //حذف و بایگانی قرارداد
  public ArchiveContract: string = "ArchiveContract";
  //حذف و بایگانی پیش فاکتور
  public ArchiveFactor: string = "ArchiveFactor";


  //--------------------------------------------------------------------------


  //******************************Function**********************************
  //************************************************************************
  //************************************************************************


  DeleteRow(item: any, form: any) {


    var i: number = 0;
    var j: number = 0;

    form.controls.forEach((control: any) => {

      if ((control as FormGroup).controls.requestNo.value == item.controls.requestNo.value && (control as FormGroup).controls.commodityId.value == item.controls.commodityId.value) {
        j = i;

      }

      i = i + 1;
    })
    if (i > 1) {
      form.removeAt(j);

    } else {
      this.showWarrningMessage("در درخواست بایستی حداقل یک کالا وجود داشته باشد ، امکان حذف وجود ندارد")
    }

  }

  DeleteRowById(item: any, form: any) {
    var i: number = 0;
    var j: number = 0;

    form.controls.forEach((control: any) => {

      if ((control as FormGroup).controls.id.value == item.controls.id.value) {
        j = i;

      }

      i = i + 1;
    })
    if (i > 1) {
      form.removeAt(j);

    } else {
      this.showWarrningMessage("در درخواست بایستی حداقل یک کالا وجود داشته باشد ، امکان حذف وجود ندارد")
    }

  }

  showHttpFailMessage(message: string) {
    this._snackBar.openFromComponent(HttpSnackbarComponent,
      {
        data: message,
        panelClass: ['failure-snackbar'],
      });
  }

  showWarrningMessage(message: string) {
    this._snackBar.openFromComponent(HttpSnackbarComponent,
      {
        data: message,
        panelClass: ['warning-snackbar'],
      });
  }

  formatDate(inputDate: Data | undefined = undefined) {

    ;
    let date, month, year;
    if (inputDate != undefined) {

      if (inputDate._d != undefined) {
        date = inputDate._d.getDate();
        month = inputDate._d.getMonth() + 1;
        year = inputDate._d.getFullYear();
      } else {
        date = inputDate.getDate();
        month = inputDate.getMonth() + 1;
        year = inputDate.getFullYear();
      }

      date = date
        .toString()
        .padStart(2, '0');

      month = month
        .toString()
        .padStart(2, '0');

    } else {
      return '';
    }


    return `${year}/${month}/${date}`;
  }

  async TagConvert(documentTags: any) {
    var i: number = 1;
    var tagstring: string = '';
    for (let item of documentTags) {
      tagstring = tagstring + item;
      if (i != documentTags.length) {
        tagstring = tagstring + ","
      }
      i = i + 1;
    }
    return tagstring;
  }

  clearNumber(form: FormGroup) {
    var text = form.value
    text = text.replace(',', '').replace(',', '').replace(',', '').replace(',', '').replace(',', '').replace(',', '');
    form.setValue(text);
  }

  filterWord(filter: SearchQuery[], searchTerm: string, propertyNames: string[]): SearchQuery[] {


    const searchWords = searchTerm.split(" ");
    for (let i = 0; i < searchWords.length; i++) {

      for (let j = 0; j < propertyNames.length; j++) {
        if (searchWords[i] != '') {
          filter.push({
            propertyName: propertyNames[j],
            comparison: 'contains',
            values: [searchWords[i]],
            nextOperand: 'and'
          })
        }

      }

    }

    return filter;
  }

  onCurrency(c: number) {
    if (c != undefined) {
      var resultinr = (c).toLocaleString('fa-IR', {
        style: 'currency',
        currency: 'IRR'
      });
      return resultinr;
    } else
      return 'ریال'

  }
  async getTableHtml(classNam: string = 'mas-table', ) {
    let html: any = document.getElementsByClassName('current-tab');
    let data_field: any;
    for (let i = 0; i < html.length; i++) {
      const slide = html[i] as HTMLElement;
      let table = slide?.getElementsByTagName('table');
      for (let j = 0; j < table.length; j++) {
        const t = table[j] as HTMLElement;
        if (t.className == classNam) {

          data_field = t;
        }
      }
    }
    return data_field;

  }
  async GetMasTableHtml(classNam: string = 'mas-table', format: string = 'pdf') {
    var html: any = document.getElementsByClassName('current-tab');

    var data_field: any;

    for (let i = 0; i < html.length; i++) {
      const slide = html[i] as HTMLElement;
      var table = slide?.getElementsByTagName('table');
      for (let j = 0; j < table.length; j++) {

        const t = table[j] as HTMLElement;

        if (t.className == classNam ) {


          if (t.querySelectorAll("tr.background-primary-50").length > 0) {
            return await this.printTableOrRows(t,format)

          } else {
            data_field = t;
          }

        }
      }
    }
    return data_field;

  }
  calculateSumTableOrRows(accessKey: string ,accessKey2?: string): Promise<number> {
    return new Promise(async (resolve, reject) => {
      try {

        let element = await this.getTableHtml();
        element = element.cloneNode(true);
        let tbody = element.getElementsByTagName('tbody');
        let rows = tbody[0].querySelectorAll("tr.background-primary-50");
        let sum: number = 0;
        if(rows.length > 0){

         // check if accessKey2 is undefined or null or empty and accesskey
          if(accessKey2 === undefined || accessKey2 === null || accessKey2 === ''){
            rows.forEach((row: any) => {
              let cells = row.querySelectorAll(`td[accesskey="${accessKey}"]`);
              if (cells.length > 0) {
                // sum += Number(cells[0].innerText ? cells[0].innerText.replace(/,/g, '') : 0);
                let innerTextAccessKey = cells[0].innerText ? cells[0].innerText.replace(/,/g, '') : 0
                sum += Number(innerTextAccessKey);
              }
            });
          }else{
            //   sumAll += Number(a.itemUnitPrice) * Number(a.quantity);
            rows.forEach((row: any) => {
              let cells = row.querySelectorAll(`td[accesskey="${accessKey}"]`);
              let cells2 = row.querySelectorAll(`td[accesskey="${accessKey2}"]`);
              if (cells.length > 0 && cells2.length > 0) {
                // sum += Number(cells[0].innerText.replace(/,/g, '')) * Number(cells2[0].innerText.replace(/,/g, ''));
                let innerTextAccessKey = cells[0].innerText ? cells[0].innerText.replace(/,/g, '') : 0
                let innerTextAccessKey2 = cells2[0].innerText ? cells2[0].innerText.replace(/,/g, '') : 0
                sum += Number(innerTextAccessKey) * Number(innerTextAccessKey2);
                console.log('accessKey',accessKey ,'sum : ',sum)
              }
            });
          }
        }else {
          sum =0
        }


        resolve(sum);
      } catch (error) {
        reject(error);
      }
    });
  }

  calculateLengthTableOrRows(): Promise<number> {
    return new Promise(async (resolve, reject) => {
      try {

        let element = await this.getTableHtml();
        element = element.cloneNode(true);
        let tbody = element.getElementsByTagName('tbody');
        let rows = tbody[0].querySelectorAll("tr.background-primary-50");
        let length: number = rows.length;



        resolve(length);
      } catch (error) {
        reject(error);
      }
    });
  }
  async printTableOrRows(t: any,format:string): Promise<any> {
    return new Promise((resolve) => {
      let title = 'چاپ'
      if (format !== 'pdf'){
        title = 'اکسل'
      }
      const dialogRef = this.dialog.open(ConfirmDialogComponent, {
        data: {
          title: ` ${title} کل جدول یا ردیف‌های انتخابی `,
          message: 'آیا از انتخاب خود مطمئن هستید ؟',
          icon: ConfirmDialogIcons.warning,
          actions: {
            confirm: {title:'ردیف های انتخابی' , show: true}, cancel: {title: `${title} کل جدول` , show: true}
          }
        }
      });

      dialogRef.afterClosed().subscribe(result => {
        let data_field;
        if (result) {
          let thead = t.getElementsByTagName('thead')[0].cloneNode(true);
          let tfoot = t.getElementsByTagName('tfoot')[0].cloneNode(true);
          let newTable = document.createElement('table');
          let tbody = document.createElement('tbody');
          t.querySelectorAll("tr.background-primary-50").forEach((row: any) => {
            tbody.appendChild(row.cloneNode(true));
          });

          // join thead and selectedRows and tfoot
          newTable.appendChild(thead);
          newTable.appendChild(tbody);
          newTable.appendChild(tfoot);
          data_field = newTable;
        } else {
          data_field = t  ;
        }
        resolve(data_field);
      });
    });
  }

  onPrint(printContents: any, title: string, callBackMethod: any = undefined) {

    var htmlData = '';

    htmlData = `

                <html><head>
                    <link href="../../assets/fonts/iranyekanwebextraboldfanum.ttf" rel="stylesheet">
                    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@5.0.2/dist/css/bootstrap.rtl.min.css" integrity="sha384-gXt9imSW0VcJVHezoNQsP+TNrjYXoGcrqBZJpry9zJt8PCQjobwmhMGaDHTASo9N" crossorigin="anonymous">
                    <script>
                      window.onafterprint = (event) => {
                      ${callBackMethod}
                      };
                      // window.onbeforeprint = (event) => {
                      //   const removelement = document.getElementById("removelement");
                      //   removelement.remove();
                      // };
                    </script>
                    <style>

                     .mat-card{
                      padding: 15px !important;
                     }
                    .disabled {
                      pointer-events: none;

                    }
                     mat-label {
                      font-size: 14px;

                    }

                    .line {
                      line-height: 2.5
                    }


                     @font-face {
                      font-family: 'IranYekanBold';
                      src: url("../../assets/fonts/iranyekanwebboldfanum.ttf") format('truetype');
                     }

                      html {
                        overflow-x: hidden;
                        overflow-y: auto;

                      }
                    td{
                      border-style: solid;
                      border-width: 0.5px;
                      border-color:gray;
                      padding:5px;
                      font-size:12px;
                      word-break:break-all;
                    }
                    th{
                      border-style: solid;
                      border-width: 0.5px;
                      padding:5px;
                      border-color:gray;
                      font-size: 14px;

                    }
                    thead{
                      background-color:#e7e7eb;
                      font-size: 12px;
                    }
                    h3,h4{
                        text-align:center;

                        font-size: 18px;

                      }
                      h4{
                           margin-top: 5px;
                           font-size: 14px;
                      }
                      .text-center {
                        text-align: center !important;
                      }
                      .font16{
                          font-size: 16px !important;
                      }
                      .font14{
                        font-size: 12px;
                        color: #333333;
                      }

                    body{
                        direction: rtl;

                        font-family: "IranYekanBold" !important;

                     }
                    table {
                      width: 100%;
                      max-width: 100%;
                      border-spacing: 0px;

                     }
                  @media print
                  {
                    #removelement{
                        display:none;

                    }
                    table { page-break-after:auto }
                    tr    { page-break-inside:auto; page-break-after:auto }
                    td    { page-break-inside:auto; page-break-after:auto }
                    thead { display:table-header-group }

                  }
                  .font-12{
                    font-size: 12px;
                  }
                  .font-11{
                    font-size: 11px;
                  }
                  .btn-link{
                    color: #0ca331 !important;

                  }
                  .btn-print{
                    background-color: #6d72ed;
                    color: #ffffff;
                    border: 1px solid #6d72ed;
                    box-shadow: 0 2px 4px 0 rgba(0, 0, 0, 0.1);
                    padding: 4px 20px;
                    text-align: center;
                    font-size: 14px;
                    font-weight: 600;

                    cursor: pointer;
                    border-radius: 1rem;
                    font-family: 'IranYekanBold',serif;


                  }
                  .line-height{
                    line-height: 2.2;
                  }


              </style>
              <title>سیستم یکپارچه دانا</title></head>
                        <body>
                          <div>
                            <button onClick='window.print()' type="button" id="removelement" class="btn-print">

                            چاپ
                            </button>
<h3><b>
شرکت ایفا سرام (سهامی خاص)
</b>
</h3>
                           <h4><b>
                                ${title}
                                </b>
                           </h4>
                            ${printContents}
                          </div>
                      </body>
               </html>`;


    const WindowObject: any = window.open('', title, 'width=950,height=750,top=50,left=50,toolbars=no,scrollbars=1,status=1,resizable=1'
    );



// Check if the window was successfully opened
    if (WindowObject) {
      // Replace 'undefined' strings in htmlData
      while (htmlData.includes("undefined")) {
        htmlData = htmlData.replace('undefined', '');
      }
      WindowObject.document.writeln(htmlData);
      WindowObject.document.close();

      WindowObject.focus();

      // Set a timeout to print after a slight delay
      // setTimeout(() => {
      //   WindowObject.print();
      // }, 1100);
      // // close after  click print button
      // setTimeout(() => {
      //   WindowObject.close();
      // },1200);

    } else {
      console.error("Failed to open new window. Check for popup blockers or permissions.");
      // alert("Unable to open new window. Please disable popup blockers and try again.");
    }





  }

  onPrintLable(printContents: any) {
    var htmlData = '';

    htmlData = `

                <html><head>
                      <style>
                        @media print {


                          #section-to-print, #section-to-print * {
                            visibility: visible;
                          }

                          #section-to-print {
                            position: absolute;
                            left: 0;
                            top: 0;
                          }
                         }
                  </style>
                  <title>سیستم یکپارچه دانا</title>
                    </head>
                        <body>
                            <div>
                              ${printContents}
                            </div>
                      </body>
               </html>`;
    const WindowObject: any = window.open('', '', 'width=950,height=750,top=50,left=50,toolbars=no,scrollbars=1,status=1,resizable=1');
    while (htmlData.includes("undefined")) {
      htmlData = htmlData.replace('undefined', '');
    }
    WindowObject.document.writeln(htmlData);
    WindowObject.document.close();
    WindowObject.print();
    WindowObject.focus();
    setTimeout(() => {
      WindowObject.close();
    }, 0.5);
  }

  async onExportToExcel(data: any) {

    const parser = new DOMParser();
    var contains = await this.GetMasTableHtmlAndDataExportAllToExcel(data)
    var doc = parser.parseFromString(contains, "text/html");

    var fileName = `DanaReports.xlsx`;
    const ws: XLSX.WorkSheet = XLSX.utils.table_to_sheet(doc);

    const wb: XLSX.WorkBook = XLSX.utils.book_new();
    XLSX.utils.book_append_sheet(wb, ws, 'Sheet1');

    XLSX.writeFile(wb, fileName);

  }


  replaceSpecialChars(string: string): string {
    string = string == undefined ? '' : string
    if (string == '') {
      return string;
    }
    try {
      string = string.replace("&", "-").replace("&", "-").replace("&", "-");
      string = string.replace("<", "-").replace("<", "-").replace("<", "-");
      string = string.replace(">", "-").replace(">", "-").replace(">", "-");
    } catch {
    }


    return string;
  }

  async GetMasTableHtmlAndData(data: any) {
    var columns: any[] = [];

    var data_html = await this.GetMasTableHtml();

    var th: any[] = data_html.querySelectorAll("thead > tr > th");
    ;
    var tfoot = data_html.getElementsByTagName("tfoot");
    let tbody = data_html.getElementsByTagName("tbody");

    for (let i = 0; i < th.length; i++) {
      columns.push(
        {
          name: th[i]?.accessKey,
          title: th[i]?.innerText,
          type: th[i]?.abbr,
        }
      );

    }

    let printContents = '<table><thead><tr>';

    for (let i = 0; i < columns.length; i++) {

      printContents += `<th>${columns[i].title}</th>`;

    }

    printContents += `</tr></thead><tbody>`

    //  add tbody to printContents
    if (tbody[0]?.childNodes.length > 0) {
      for (let i = 0; i < tbody[0]?.childNodes.length; i++) {
        printContents += `<tr>${tbody[0]?.childNodes[i]?.innerHTML}</tr>`;
      }
    } else {
      printContents += '</tbody>'
    }
    if (tfoot[0]?.childNodes[0]?.innerHTML != undefined) {
      printContents += tfoot[0]?.childNodes[0]?.innerHTML;
    }

    printContents += '</table>'
    // if (tfoot[0]?.childNodes[0]?.innerHTML != undefined) {
    //   printContents += tfoot[0]?.childNodes[0]?.innerHTML;
    // } else {
    //   printContents += '</table>'
    // }


    // if (data.length > 0) {
    //
    //   let j = 0
    //   data.forEach((data: any) => {
    //     j++;
    //     printContents += '<tr>'
    //     for (let i = 0; i < columns.length; i++) {
    //
    //       let columnName = columns[i].name;
    //       let value = this.replaceSpecialChars(data[columnName])
    //       if ((columns[i].type == 'date' || columns[i].type == 'datetime') && value != '') {
    //
    //         value = this.toPersianDate(data[columnName])
    //       }
    //       if ((columns[i].type == 'money') && value != '') {
    //
    //         value = data[columnName].toLocaleString();
    //       }
    //       if (columns[i].title == 'ردیف') {
    //         value = Number(j).toString();
    //       }
    //       printContents += `<td>${value}</td>`;
    //
    //     }
    //     printContents += '</tr>'
    //   })
    //   if (tfoot[0]?.childNodes[0]?.innerHTML != undefined) {
    //     printContents += '</tbody>' + tfoot[0]?.childNodes[0]?.innerHTML;
    //   }
    //   else {
    //     printContents += '</table>'
    //   }
    //
    //
    //
    // }

    return printContents;
  }
  async GetMasTableHtmlAndDataExportAllToExcel(data: any) {
    var columns: any[] = [];

    let data_html
    data_html = await this.getTableHtml();

    var th: any[] = data_html.querySelectorAll("thead > tr > th");

    var tfoot = data_html.getElementsByTagName("tfoot");
    let tbody = data_html.getElementsByTagName("tbody");

    for (let i = 0; i < th.length; i++) {
      columns.push(
        {
          name: th[i]?.accessKey,
          title: th[i]?.innerText,
          type: th[i]?.abbr,
        }
      );

    }

    let printContents = '<table><thead><tr>';

    for (let i = 0; i < columns.length; i++) {

      printContents += `<th>${columns[i].title}</th>`;

    }

    printContents += `</tr></thead><tbody>`




    if (data.length > 0) {

      let j = 0
      data.forEach((data: any) => {
        j++;
        printContents += '<tr>'
        for (let i = 0; i < columns.length; i++) {

          let columnName = columns[i].name;
          let value = this.replaceSpecialChars(data[columnName])
          if ((columns[i].type == 'date' || columns[i].type == 'datetime') && value != '') {

            value = this.toPersianDate(data[columnName])
          }
          if ((columns[i].type == 'money') && value != '') {

            value = data[columnName].toLocaleString();
          }
          if (columns[i].title == 'ردیف') {
            value = Number(j).toString();
          }
          printContents += `<td>${value}</td>`;

        }
        printContents += '</tr>'
      })
      // if (tfoot[0]?.childNodes[0]?.innerHTML != undefined) {
      //   printContents += '</tbody>' + tfoot[0]?.childNodes[0]?.innerHTML;
      // }
      // else {}
      printContents += '</tbody></table>'




    }else {
      printContents += '</tbody></table>'
    }

    return printContents;
  }

  toPersianDate(date: Data | undefined): string {

    let result: any = new ToPersianDatePipe().transform(date, 'toPersianDate')
    return result.replace('00:00', '').replace('03:30', '')

  }

  async FilterTable(data: any[], SearchInput: string) {

    var input: any = document.getElementById(SearchInput);
    var filter: any = input.value.toLowerCase();

    if (data.length > 0 && filter) {
      var keys = Object.keys(data[0]);
      if (keys.filter(x => x.toLowerCase().includes(filter))?.length > 0) {
        return [];
      }
    }

    var filteredData = data.filter(x => {
      var doesIncludeSearchValue = JSON.stringify(x)?.toLowerCase()?.includes(filter)
      if (doesIncludeSearchValue) return x;
    })
    return filteredData;
  }


  RemoveId(SelectedReceipt: any) {
    SelectedReceipt.selected = false;
    this.ListId = this.ListId.filter(a => (a != SelectedReceipt.id.toString()))
  }

  clearListId(Receipts: any[]) {
    this.ListId = [];
    Receipts.forEach(a => {
      a.selected = false;
    })
  }

}



