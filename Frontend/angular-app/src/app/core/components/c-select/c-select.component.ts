import { Component, EventEmitter, Input, OnInit, Optional, Output, Self } from '@angular/core';
import { Subscription, fromEvent } from 'rxjs';
import { ControlValueAccessor, DefaultValueAccessor, UntypedFormControl, FormControlDirective, FormControlName, FormGroupDirective, NG_VALUE_ACCESSOR, NgControl, NgModel } from '@angular/forms';
import { AuditLogSettings } from 'ngx-data-table';

@Component({
    selector: 'c-select',
    templateUrl: './c-select.component.html',
    styleUrls: ['./c-select.component.scss']
})
export class CSelectComponent implements OnInit, ControlValueAccessor {

    control!: UntypedFormControl;
    valueChangesSubscription: Subscription = new Subscription();
    @Output() public onSelectionChange: EventEmitter<any> = new EventEmitter<any>();
    onSelectionChanged(event: any) {
        this.onSelectionChange.emit(event);
    }
    private _data: any[] = [];
    _displayedData: any[] = [];

    @Input()
    label: string = "";

    @Input() public description?: string;

    @Input()
    type: 'text' | 'number' | 'email' | 'tel' | 'password' = "text";

    @Input()
    labelType: 'inside' | 'outside' = "inside";

    @Input()
    formControlName!: string;

    @Input()
    floatLabel: boolean = false;

    @Input()
    placeholder: string = "";

    @Input()
    isLoading = false;

    @Input()
    enabled: boolean = true;

    @Input()
    mandatory: boolean = false;

    @Input()
    hasError: boolean = false;

    @Input()
    set data(value_data: any[]) {

        this._data = value_data;
        this._displayedData = value_data;
        this.resetIfValueIsNotInData(this.control?.value);
    }

    @Input()
    fieldName: string = "text";

    @Input()
    valueFieldName: string = "value";

    @Input()
    useDataItemAsValue: boolean = false;

    @Input()
    unselectOptionEnabled: boolean = false;

    @Input()
    auditLogHistorySettings?: AuditLogSettings;

    get selected(): any {
        return this._data.find(value => value[this.valueFieldName] === (this.useDataItemAsValue ? this.control?.value[this.valueFieldName] : this.control?.value));
    }

    constructor(@Optional() @Self() public ngControl: NgControl) {
        if (this.ngControl != null) {
            // Setting the value accessor directly (instead of using the providers) to avoid running into a circular import.
            this.ngControl.valueAccessor = this;
        }
    }

    ngOnInit(): void {
        if (this.ngControl instanceof FormControlName) {
            const formGroupDirective = this.ngControl.formDirective as FormGroupDirective;
            if (formGroupDirective && this.ngControl != null) {
                this.control = formGroupDirective.form.controls[this.ngControl.name ?? ''] as UntypedFormControl;
            }
        } else if (this.ngControl instanceof FormControlDirective) {
            this.control = this.ngControl.control;
        } else if (this.ngControl instanceof NgModel) {
            this.control = this.ngControl.control;
            this.valueChangesSubscription.add(this.control.valueChanges.subscribe(x => this.ngControl.viewToModelUpdate(this.control.value)));
        } else if (!this.ngControl) {
            this.control = new UntypedFormControl();
        }

        //Prevent set an invalid value for the form control, e.g. in reply form we set the from email address field to an email address that it may not be included in the list, because it is not a send enabled email address, therefore we need to prevent such value for the from field and reset it to null.
        this.valueChangesSubscription.add(this.control.valueChanges.subscribe((value: any) => {
            this.resetIfValueIsNotInData(value);
        }));
    }

    private resetIfValueIsNotInData(value: any) {
        if (this.control && value !== this.control.defaultValue && this._data && this._data.length && !this.selected)
            this.control.reset();
    }

    onSearch($event: any) {

    }

    compareValue = (object1: any, object2: any) => {
        return object1 && object2 && object1[this.valueFieldName] == object2[this.valueFieldName];
    }

    ngOnDestroy() {
        this.valueChangesSubscription.unsubscribe();
    }

    writeValue(obj: any): void { }
    registerOnChange(fn: (_: any) => void): void { }
    registerOnTouched(fn: any): void { }

}
