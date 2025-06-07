import { Component, EventEmitter, Input, OnInit, Optional, Output, Pipe, PipeTransform, Self } from '@angular/core';
import { ControlValueAccessor, UntypedFormControl, FormControlDirective, FormControlName, FormGroupDirective, NgControl, NgModel, FormControl } from '@angular/forms';
import { catchError, debounceTime, distinctUntilChanged, filter, merge, Observable, of, Subscription, switchMap, tap } from 'rxjs';
import { getCurrencySymbol } from '@angular/common';
import { MatSnackBar } from '@angular/material/snack-bar';
import { TextValue } from 'src/app/modules/shared/models/common/text-value';
import { PageDialogComponent, PageDialogConfig } from 'utilities';
import { TextValueGridComponent } from 'src/app/modules/main/components/text-value-grid/text-value-grid.component';
import { MatDialog } from '@angular/material/dialog';
import { AuditLogSettings } from 'ngx-data-table';

export interface DuplicateCheckerConfig {
    fieldName: string,
    secondControl?: FormControl<any>,
    countDuplicatesObservableFn: () => Observable<number>,
    getDuplicatesListObservableFn: () => Observable<TextValue[]>,
    duplicatesListTitle: string,
    duplicatesListTextColumnCaption: string,
    duplicatesListValueColumnCaption: string,
    duplicatesListOtherColumns?: { name: string, caption: string }[]
}

@Component({
    selector: 'c-input',
    templateUrl: './c-input.component.html',
    styleUrls: ['./c-input.component.scss'],

})
export class CInputComponent implements OnInit, ControlValueAccessor {

    control!: UntypedFormControl;
    componentSubscriptions: Subscription = new Subscription();

    @Input()
    label: string = "";
    @Input() public description?: string;

    @Input()
    type: 'text' | 'number' | 'email' | 'tel' | 'password' | 'text-area' | 'time' | 'money' | 'percentage' = "text";

    @Input()
    labelType: 'inside' | 'outside' = "inside";

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
    value: any;
    @Input() public error: string | null = null;

    @Input()
    autocomplete: string = "off";

    @Output()
    valueChange = new EventEmitter<any>();

    @Input()
    readonly: boolean | "" = false;

    @Input()
    currencyCode: string | null | undefined = null;

    @Input()
    min: number = 0;

    @Input()
    max: number = 100;

    @Input()
    auditLogHistorySettings?: AuditLogSettings;

    duplicateMatchedCount = 0;
    @Input() duplicateCheckerConfig?: DuplicateCheckerConfig;

    showMatchedModal = () => {
        this.componentSubscriptions.add(this.duplicateCheckerConfig!.getDuplicatesListObservableFn().subscribe(result => {
            this.openTextValueGridModal(this.duplicateCheckerConfig!.duplicatesListTitle, result)
        })
        );
    }

    openTextValueGridModal(title: string, dataSource: any[]) {
        const data: PageDialogConfig = {
            title: title,
            component: TextValueGridComponent,
            afterComponentCreated: (componentInstance: any) => {
                const component = <TextValueGridComponent>componentInstance;
                component.dataSource = dataSource;
                component.textCaption = this.duplicateCheckerConfig!.duplicatesListTextColumnCaption;
                component.valueCaption = this.duplicateCheckerConfig!.duplicatesListValueColumnCaption;
                if (this.duplicateCheckerConfig!.duplicatesListOtherColumns) {
                    component.otherColumns = this.duplicateCheckerConfig!.duplicatesListOtherColumns;
                }
            },
            isFullScreen: false,
            customCssClasses: "md:w-full lg:w-full m-2"
        };
        this.dialog?.open(PageDialogComponent, {
            data: data
        });
    }

    constructor(@Optional() @Self() public ngControl: NgControl,
        private snackBar?: MatSnackBar,
        private dialog?: MatDialog) {
        if (this.ngControl != null) {
            // Setting the value accessor directly (instead of using the providers) to avoid running into a circular import.
            this.ngControl.valueAccessor = this;
        }
    }

    ngOnInit(): void {
        if (!this.placeholder && this.autoPlaceholder) {
            this.placeholder = "Type " + this.label;
        }

        if (this.ngControl instanceof FormControlName) {
            const formGroupDirective = this.ngControl.formDirective as FormGroupDirective;
            if (formGroupDirective && this.ngControl != null) {
                this.control = formGroupDirective.form.controls[this.ngControl.name ?? ''] as UntypedFormControl;
            }
        } else if (this.ngControl instanceof FormControlDirective) {
            this.control = this.ngControl.control;
        } else if (this.ngControl instanceof NgModel) {
            this.control = this.ngControl.control;
            this.componentSubscriptions.add(this.control.valueChanges.subscribe(x => this.ngControl.viewToModelUpdate(this.control.value)));
        } else if (!this.ngControl) {
            this.control = new UntypedFormControl();
        }

        this.control.valueChanges.subscribe(value => this.onValueChanged(value));

        if (this.duplicateCheckerConfig) {
            let subjectForDuplicateCheck = this.control.valueChanges;

            if (this.duplicateCheckerConfig.secondControl)
                subjectForDuplicateCheck = merge(this.control.valueChanges, this.duplicateCheckerConfig.secondControl.valueChanges);

            this.componentSubscriptions.add(
                subjectForDuplicateCheck.pipe(
                    tap(() => this.duplicateMatchedCount = 0),
                    debounceTime(500),
                    distinctUntilChanged(),
                    filter(value => Boolean(value) && this.control.valid && (Boolean(!this.duplicateCheckerConfig!.secondControl || this.duplicateCheckerConfig!.secondControl!.valid))),
                    switchMap(() => this.duplicateCheckerConfig!.countDuplicatesObservableFn()
                        .pipe(catchError(() => {
                            this.snackBar?.open(`There was an error in checking possible duplicate ${this.duplicateCheckerConfig!.fieldName}s.`, 'OK');
                            return of();
                        }))
                    ),
                ).subscribe((matchCount: number) => {
                    this.duplicateMatchedCount = matchCount;
                }));
        }
    }

    onValueChanged(value:any) {
        this.valueChange.emit(value);
    }

    ngOnDestroy() {
        if (this.componentSubscriptions)
            this.componentSubscriptions.unsubscribe();
    }

    // These are just to make Angular happy. Not needed since the control is passed to the child input
    writeValue(value: any): void { 
        if(this.type === 'money') {
            this.value = this.formatNumber(Number(value));            
        }
    }

    registerOnChange(fn: (_: any) => void): void { }
    registerOnTouched(fn: any): void { }


    // Called when the input changes (keyup)
    onKeyup(event: any): void {
        //this.value = event.target.value;
        this.onValueChanged(this.control.value); // Trigger the change event
    }
    
    formatNumber(value: number | string): string {
        const num = typeof value === 'string' ? parseFloat(value) : value;
        return isNaN(num) ? '' : num.toFixed(2);
    }

}

@Pipe({ name: 'currencySymbol' })
export class CurrencySymbolPipe implements PipeTransform {
    transform(currencyCode: string, format: 'wide' | 'narrow' = 'narrow', locale?: string): string {
        return getCurrencySymbol(currencyCode, format, locale);
    }
}
