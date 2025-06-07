export enum Documents {
  Remittance = 28509,
  ChequeSheet = 28511,
  Cash = 28513,
}

export enum CurrencyType {
  Rial = 1,
  NonRial = 0,
}

export enum CodeVouchers {
  BursaryReceiveDocument = 2259,
}

export enum AccountHeads {
  //طرف سفته های تضمینی ما نزد دیگران
  AccountHeadCode_9202 = 1878,
  //--موجودی ریالی نزد بانکها
  AccountHeadCode_2601 = 1900,
  //حسابهای دریافتنی تجاری
  AccountHeadCode_2304 = 1901,
  //اسناد دریافتنی نزد صندوق
  AccountHeadCode_2301 = 1879,
  //موجودی نزد بانكها - ارزی
  AccountHeadCode_2602 = 1899,
  //حسابهای دریافتنی غیر تجاری - شرکتهاوموسسات وسازمانها
  AccountHeadCode_2401 = 1904,
  // سایر بدهکاران
  AccountHeadCode_2410 = 1913,
  // تنخواه گردان
  AccountHeadCode_2605 = 1881,
  // حساب های پرداختنی تجاری
  AccountHeadCode_5102 = 1780,
 // اسناد در جریان وصول نزد بانکها
  AccountHeadCode_2303 = 1880,
// حسابهای انتظامی
 AccountHeadCode_9102 = 1813,


}

export enum FinantialReferences {
  PreInvoice = 28514,
  Invoice = 28515,
  PaymentReceipt = 28516,

}

export enum AccountReferencesGroupEnums {
  // خریداران داخلی
  AccountReferencesGroupCode_31 = 28,
  // خریداران خارجی
  AccountReferencesGroupCode_32 = 29,
  // بانک ها
  AccountReferencesGroupCode_02 = 4,

  AccountReferencesGroupCode_1001 = 94,
  AccountReferencesGroupCode_1002 = 95,
  // سایر
  AccountReferencesGroupCode_06004 = 53,
  // تنخواه گردان ارزی
  AccountReferencesGroupCode_01002002 = 110,


}

export enum AccountTypes {
  Credit = 1,
  Debit = 2
}

export enum ReceiptInsertedByCustomerStatus {
  Add = 1,
  AddToBursary = 2,
  AddToAccounting = 3,
  Remove = 4
}




