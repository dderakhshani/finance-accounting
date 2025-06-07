import {AccountHead} from "../../../../../modules/accounting/entities/account-head";
import {AccountReference} from "../../../../../modules/bursary/entities/account-reference";

export class PrintOptions {
  reportType : string = '';
  reportTitle : string = 'گزارش';
  dateFrom : any;
  dateTo : any;
  callBackMethod! : any ;
  hasCustomizeHeaderPage!:boolean;
  hasCustomizeFooterPage!:boolean;
  customizeHtmlHeader!:string;
  customizeHtmlFooter!:string;
  accountHeads!:AccountHead[];
  accountReferences!:AccountReference[];

   constructor( reportTitle : string = 'گزارش' , reportType : string = '' ,dateFrom ?: any , dateTo ?: any  ,callBackMethod= undefined,hasCustomizeHeaderPage:boolean=false,hasCustomizeFooterPage:boolean=false
     ,customizeHeaderPage:string='',customizeFooterPage:string='') {
    this.reportType = reportType;
    this.reportTitle = reportTitle;
    this.dateFrom = dateFrom ;
    this.dateTo = dateTo ;
    this.hasCustomizeHeaderPage = hasCustomizeHeaderPage;
    this.hasCustomizeFooterPage = hasCustomizeFooterPage;
    this.customizeHtmlHeader=customizeHeaderPage;
    this.customizeHtmlFooter=customizeFooterPage;
    this.callBackMethod = callBackMethod;
   }
}
