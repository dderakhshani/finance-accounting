

export enum DocumentState {
  Temp = 23,//رسید موقت
  Direct = 33,//رسید مستقیم
  invoiceAmount = 43,//ریالی کردن ورودی
  invoiceAmountLeave = 44,//ریالی کردن خروجی
  invoiceAmountStart = 45,//ریالی کردن افتتاحیه
  registrationAccounting = 53,//ثبت سند مکانیزه حسابداری
  registrationAccountingLeave = 54,//ثبت سند مکانیزه حسابداری خروجی
  registrationAccountingStart = 55,//ثبت سند مکانیزه افتتاحیه
  Leave = 63,//خروج کالا
  transfer = 64,//خروج جهت انتقال بین انبار ها 
  requestRecive = 73,//درخواست دریافت کالا
  requestBuy = 83,//درخواست خرید
  archiveReciept = 13,//آرشیو
  archiveRequest = 12,//آرشیو درخواست
  archiveBuy = 11,//آرشیو خرید
  changeWarehouse = 93
}
