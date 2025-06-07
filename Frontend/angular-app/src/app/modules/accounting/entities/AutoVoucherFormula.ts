export class AutoVoucherFormula {
  public id!:number
  public voucherTypeId!:number
  public sourceVoucherTypeId!:number
  public voucherTypeTitle!:string
  public sourceVoucherTypeTitle!:string
  public orderIndex!:number
  public debitCreditStatus!:number
  public accountHeadId!:number
  public rowDescription!:string
  public formula!:string
  public conditions!:string
  public parsedFormula!:VoucherFormula[]
}


export class VoucherFormula {
  public property!:string
  public value!:Value
}

export class Value {
  public text!:string
  public properties!:Property[]
}

export class Property {
  public name!:string
  public aggregateFunction!:string
}

export class Condition {
  public expression!:string
  public properties!:Property[]
}
