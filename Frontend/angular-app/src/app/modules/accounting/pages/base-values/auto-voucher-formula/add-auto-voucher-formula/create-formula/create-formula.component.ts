import {Component, Input, OnInit} from '@angular/core';
import {FormArray, FormControl, FormGroup} from "@angular/forms";

@Component({
  selector: 'app-create-formula',
  templateUrl: './create-formula.component.html',
  styleUrls: ['./create-formula.component.scss']
})
export class CreateFormulaComponent implements OnInit {
  propertyNameList = [
    {
      title: 'VoucherDate',
      persianName: 'تاریخ سند'
    },
    {
      title: 'AccountHeadId',
      persianName: 'سرفصل حساب'
    },
    {
      title: 'AccountReferencesGroupId',
      persianName: 'گروه تفصیل'
    },
    {
      title: 'VoucherRowDescription',
      persianName: 'شرح آرتیکل'
    },
    {
      title: 'Credit',
      persianName: 'بستانکار'
    },
    {
      title: 'Debit',
      persianName: 'بدهکار'
    },
    {
      title: 'RowIndex',
      persianName: 'ردیف'
    },
    {
      title: 'DocumentId',
      persianName: 'شناسه سند مرتبط'
    },
    {
      title: 'DocumentNo',
      persianName: 'شماره سند مرتبط'
    },
    {
      title: 'DocumentIds',
      persianName: 'فهرست شماره اسناد مرتبط'
    },
    {
      title: 'ReferenceDate',
      persianName: 'تاریخ مرجع'
    },
    {
      title: 'FinancialOperationNumber',
      persianName: 'شماره فرم عملیات مالی'
    },
    {
      title: 'RequestNo',
      persianName: 'شماره درخواست'
    },
    {
      title: 'InvoiceNo',
      persianName: 'شماره فاکتور مشتری'
    },
    {
      title: 'Tag',
      persianName: 'تگ'
    },
    {
      title: 'Weight',
      persianName: 'وزن'
    },
    {
      title: 'ReferenceId1',
      persianName: 'تفصیل شناور 1'
    },
    {
      title: 'ReferenceId2',
      persianName: 'تفصیل شناور 2'
    },
    {
      title: 'ReferenceId3',
      persianName: 'تفصیل شناور 3'
    },
    {
      title: 'DebitCreditStatus',
      persianName: 'وضعیت مانده حساب'
    },
    {
      title: 'CurrencyTypeBaseId',
      persianName: 'نوع ارز'
    },
    {
      title: 'CurrencyFee',
      persianName: 'نرخ ارزبه ریال'
    },
    {
      title: 'CurrencyAmount',
      persianName: 'مبلغ ارز'
    },
    {
      title: 'TraceNumber',
      persianName: 'شماره پیگیری'
    },
    {
      title: 'Quantity',
      persianName: 'تعداد'
    },
    {
      title: 'Remain',
      persianName: 'مانده'
    },
    {
      title: 'ChequeSheetId',
      persianName: 'شناسه برگ چک'
    }
  ]
  formControl: FormControl = new FormControl();
  formulaArray = new FormArray([]);

  @Input() set JsonFormulaSetter(value: FormControl) {
    this.formControl = value;
    this.formulaArray = new FormArray([])
    this.generateForm()
      }

  constructor() {
  }

  ngOnInit(): void {
  }

  generateJson() {
    let formulaData = this.formulaArray.getRawValue()
    let generatedFormula: any[] = []
    if (formulaData.length <= 0) return;
    formulaData?.forEach((x: any) => {
      generatedFormula.push({
        Property: x.property,
        Value: {
          Text: x.formula,
          Properties: x.parameters?.map((y: any) => {
            return {Name: y}
          })
        }
      })
    })
    this.formControl.setValue(JSON.stringify(generatedFormula))
  }

  generateForm() {
    let formData = JSON.parse(this.formControl.value)
    formData?.forEach((x: any) => {
      this.generateNewRowFormula({
        property: x.Property,
        parameters: x.Value.Properties.map((y: any) => {
          return y.Name
        }),
        formula: x.Value.Text
      })
    })
  }

  generateNewRowFormula(formula?: any) {
    // debugger
    let form !: FormGroup;
    if (formula) {
      form = new FormGroup({
        property: new FormControl(formula?.property),
        parameters: new FormArray([...formula?.parameters.map((x:any) => new FormControl(x))]),
        param: new FormControl(),
        formula: new FormControl(formula?.formula)
      })
    } else {
      form = new FormGroup({
        property: new FormControl(),
        parameters: new FormArray([]),
        formula: new FormControl(),
        param: new FormControl(),
      })
    }

    this.formulaArray.controls.push(<FormGroup>form);
  }

  addParameter(form: FormGroup) {
    if (form.controls['param'].value) {
      (<FormArray>form.controls['parameters']).controls.push(new FormControl(form.controls['param'].value))
      form.controls['param'].setValue(undefined)
    }
  }
  removeRawFormula(i:number){
    this.formulaArray.removeAt(i);
  }
  removeParam(form: FormGroup,i:number){
    (<FormArray>form.controls['parameters']).removeAt(i);
  }
}
