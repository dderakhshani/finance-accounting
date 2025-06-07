import {Component, Inject} from '@angular/core';
import {MAT_DIALOG_DATA, MatDialogRef} from "@angular/material/dialog";
import {BaseComponent} from "../../../../../../core/abstraction/base.component";
import {Company} from "../../../../entities/company";
import {Year} from "../../../../entities/year";
import {FormArray, FormControl, FormGroup} from "@angular/forms";
import {PageModes} from "../../../../../../core/enums/page-modes";

@Component({
  selector: 'app-add-user-year-dialog',
  templateUrl: './add-user-year-dialog.component.html',
  styleUrls: ['./add-user-year-dialog.component.scss']
})
export class AddUserYearDialogComponent extends BaseComponent {


  companies: Company[] = [];
  years: Year[] = [];
  pageModes = PageModes;
  selectedCompanyYears: Year[] = [];

  constructor(
    public dialogRef: MatDialogRef<AddUserYearDialogComponent>,
    @Inject(MAT_DIALOG_DATA) data: any
  ) {
    super()
    this.pageMode = data.pageMode;

    this.years = data.years;
    this.companies = this.pageMode === PageModes.Add ? data.selectableCompanies : data.companies;

    this.form = new FormGroup({
      companyId: new FormControl(data.companyId ?? undefined),
      yearIds: new FormArray(data.userYears?.map((x: Year) => new FormControl(x.id)) ?? [])
    })
  }


  async ngOnInit() {
    await this.resolve()
  }

  async resolve() {
    await this.initialize()
  }

  async initialize() {
    if (this.pageMode === PageModes.Add) {

    }
    if (this.pageMode === PageModes.Update) {
      this.form.controls['companyId'].disable();
      this.handleCompanySelection()
    }
  }


  handleCompanySelection() {
    this.selectedCompanyYears = this.years.filter(x => x.companyId === this.form.controls['companyId']?.value)
  }

  add(param?: any) {
    this.dialogRef.close(this.form.getRawValue())
  }

  update(param?: any) {
    this.dialogRef.close(this.form.getRawValue())
  }

  handleYearSelections(selected: boolean, yearId: number) {
    let formControl = this.form.controls['yearIds'].controls.find((x: FormControl) => x.value === yearId)
    if (selected && !formControl) {
      this.form.controls['yearIds'].controls.push(new FormControl(yearId))
    }
    if(!selected && formControl) {
      this.form.controls['yearIds'].controls.splice(this.form.controls['yearIds'].controls.indexOf(formControl),1);
    }
    this.form.patchValue(this.form.getRawValue())
  }

  isYearPreChecked(yearId: number) {
    return this.form.controls['yearIds'].controls.includes((x: FormControl) => x.value === yearId)
  }


  close(): any {
  }

  delete(param?: any): any {
  }

  get(param?: any): any {
  }


}
