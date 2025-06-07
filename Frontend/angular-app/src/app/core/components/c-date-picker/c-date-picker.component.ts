import { SqDateTime } from 'utilities';
import { Component, Input, OnInit, Optional, Output, Self } from '@angular/core';
import { Subscription, fromEvent } from 'rxjs';
import { ControlValueAccessor, DefaultValueAccessor, UntypedFormControl, FormControlDirective, FormControlName, FormGroupDirective, NG_VALUE_ACCESSOR, NgControl, NgModel } from '@angular/forms';
import { AuditLogSettings } from 'ngx-data-table';



@Component({
    selector: 'c-date-picker',
    templateUrl: './c-date-picker.component.html',
    styleUrls: ['./c-date-picker.component.scss']
})
export class CDatePickerComponent implements OnInit, ControlValueAccessor {

    control!: UntypedFormControl;
    valueChangesSubscription!: Subscription;

    @Input()
    label: string = "";

    @Input()
    labelType: 'inside' | 'outside' = "inside";

    @Input()
    min!: Date | SqDateTime;

    @Input()
    max!: Date | SqDateTime;

    @Input()
    formControlName!: string;

    @Input()
    floatLabel: boolean = false;

    @Input()
    placeholder: string = "";

    @Input()
    autoPlaceholder = false;

    @Input()
    enabled: boolean = true;

    @Input()
    mandatory: boolean = false;

    @Input()
    readonly: boolean | "" = false;

    @Input() public description?: string;

    @Input()
    auditLogHistorySettings?: AuditLogSettings;

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
            this.valueChangesSubscription = this.control.valueChanges.subscribe(x => this.ngControl.viewToModelUpdate(this.control.value));
        } else if (!this.ngControl) {
            this.control = new UntypedFormControl();
        }
    }

    ngOnDestroy() {
        this.valueChangesSubscription?.unsubscribe();
    }

    writeValue(obj: any): void { }
    registerOnChange(fn: (_: any) => void): void { }
    registerOnTouched(fn: any): void { }

}
