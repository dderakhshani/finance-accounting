import {Component, Input, OnInit, ViewChild} from '@angular/core';
import {BaseComponent} from "../../../../../../../../core/abstraction/base.component";
import {DatabaseMetadata} from "../../../../../../../admin/entities/DatabaseMetadata";
import {Observable} from "rxjs";
import {FormControl} from "@angular/forms";
import {map, startWith} from "rxjs/operators";
import {MatAutocomplete} from "@angular/material/autocomplete";
import {Data} from "@angular/router";
import {VoucherFormula} from "../../../../../../entities/AutoVoucherFormula";

@Component({
  selector: 'app-voucher-formula',
  templateUrl: './voucher-formula.component.html',
  styleUrls: ['./voucher-formula.component.scss']
})
export class VoucherFormulaComponent extends BaseComponent {

  @Input() formControl!:FormControl;
  label!:string;
  @Input() entity!:VoucherFormula;
  chips:any[] = []

  parsedFormula:any;

  @Input() databaseMetadata:DatabaseMetadata[] = []
  filteredDatabaseMetadata: Observable<DatabaseMetadata[]> = new Observable<DatabaseMetadata[]>()


  @ViewChild('formulaChipsAuto') matAutocomplete!: MatAutocomplete;

  constructor() {
    super();
  }

  ngOnInit(): void {


    // let str = "One [1] Two [2] Three [3]"
    // let splitterStr = str.split(' ')
    // splitterStr.forEach((value:any) => {
    //   if (value.match('\\[\\d]')) {
    //   }
    // })
    this.filteredDatabaseMetadata = this.formControl.valueChanges.pipe(
      startWith(null),
      map((searchTerm: string | null) => searchTerm ? this.filterDatabaseMetadata(searchTerm) : this.databaseMetadata.slice())
    )
    this.label = this.databaseMetadata.find(x => x.property.toLowerCase() == this.entity.property.toLowerCase())?.translated ?? ''
  }
  filterDatabaseMetadata(searchTerm:string) {
    return this.databaseMetadata.filter(x => x.translated?.includes(searchTerm))
  }

  add(event: any): any {
    if ((event.value || '').trim()) {
      const isOptionSelected = this.matAutocomplete.options.some(option => option.selected);
      if (!isOptionSelected) {
        this.chips.push(event.value.trim());
      }
    }
    // Reset the input value
    if (event.input) {
      event.input.value = '';
    }
    this.formControl.setValue(null);
  }
  selected(metadata:DatabaseMetadata) {
    this.chips.push(metadata.translated)
    this.formControl.setValue(null);
  }

  close(): any {
  }

  delete(metadata: any): any {
    const index = this.chips.indexOf(metadata);

    if (index >= 0) {
      this.chips.splice(index, 1);
    }
  }

  get(param?: any): any {
  }

  initialize(params?: any): any {
  }

  resolve(params?: any): any {
  }

  update(param?: any): any {
  }

}
