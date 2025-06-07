import { MatAutocompleteSelectedEvent } from '@angular/material/autocomplete';
import { FormControl, FormGroup } from '@angular/forms';
import { Observable } from 'rxjs';
import { MatChipInputEvent } from '@angular/material/chips';
import { map, startWith } from 'rxjs/operators';
import { COMMA, ENTER, TAB } from '@angular/cdk/keycodes';
import { Component, ElementRef, EventEmitter, Input, OnChanges, Output, ViewChild } from '@angular/core';
import { Mediator } from '../../../../../core/services/mediator/mediator.service';
import { PagesCommonService } from '../../../../../shared/services/pages/pages-common.service';
import { GetDocumnetTagBaseValueQuery } from '../../../repositories/base-value/get-base-value-document-tag-query';





@Component({
  selector: 'app-combo-tag',
  templateUrl: './combo-tag.component.html',
  styleUrls: ['./combo-tag.component.scss']
})
export class ComboTagComponent implements OnChanges {

  //-----------------Tag----------------------------------------
  documentTagBaseValue: string[] = [];

  documentTagBaseValue_Filter: string[] = [];
  
  separatorKeysCodes: number[] = [COMMA, ENTER];
  TagCtrl = new FormControl();

  form = new FormGroup({

    tags: new FormControl(),
   
  });
  
  filteredTags: Observable<string[]>;
  @ViewChild('tagInput') tagInput: ElementRef<HTMLInputElement> | any;


  public nodes: any[] = [];
  @Input() isDisable: boolean = false;
  @Input() DefaultId: any = undefined
  @Input() lablelTitleCombo: string = "";
  @Input() isRequired: boolean = false;
  @Input() documentTags: string[] = [];
  @Output() SelectTags = new EventEmitter<any>();
  @Input() tabindex: number = 1;
  
  
  constructor(
    private _mediator: Mediator,
    private Service: PagesCommonService)
  {
    this.filteredTags = this.TagCtrl.valueChanges.pipe(
      startWith(null),
      map((tags: string | null) => (tags ? this._filter(tags) : this.documentTagBaseValue.slice())),
    );
  }
  async ngOnChanges() {

   
    
   
    //-----------------------نمایش مقدار اولیه----------------------
    if (this.documentTags != undefined) {
      
      var tagstring: string = await this.Service.TagConvert(this.documentTags);
      this.form.controls.tags.setValue(tagstring);

    }
    
  }

  async ngOnInit() {

    await this.resolve();
  }

  async resolve() {

    await this.initialize()
    this.filteredTags = this.TagCtrl.valueChanges.pipe(
      startWith(null),
      map((tags: string | null) => (tags ? this._filter(tags) : this.documentTagBaseValue.slice())),
    );
  }
  async initialize() {
    this.documentTagBaseValue =  await this._mediator.send(new GetDocumnetTagBaseValueQuery());

    this.documentTagBaseValue_Filter = this.documentTagBaseValue;
  }
  
  //---------------------TAG--------------------------
  addTag(event: MatChipInputEvent): void {

    const value = (event.value || '').trim();

    // Add our fruit
    if (value) {

      this.documentTags.push(value);
    }

    // Clear the input value
    event.chipInput!.clear();

    this.form.controls.tags.setValue(null);
  }

  remove(tags: string): void {

    const index = this.documentTags.indexOf(tags);

    if (index >= 0) {
      this.documentTags.splice(index, 1);

    }

  }
  selected(event: MatAutocompleteSelectedEvent): void {

    this.documentTags.push(event.option.viewValue);
    this.SelectTags.emit(this.documentTags);
    this.tagInput.nativeElement.value = '';
    this.form.controls.tags.setValue(null);
  }


  private _filter(value: string): string[] {
   
    const filterValue = value.toLowerCase();

    return this.documentTagBaseValue.filter(tag => tag.toLowerCase().includes(filterValue));
  }

  
  
}



