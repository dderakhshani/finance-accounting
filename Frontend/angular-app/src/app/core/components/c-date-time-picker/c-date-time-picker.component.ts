import { PageDialogComponent, PageDialogConfig, SqDateTime, TimeSpan } from 'utilities';
import { Component, EventEmitter, forwardRef, Input, OnInit, Output, TemplateRef, ViewChild } from '@angular/core';
import { ControlValueAccessor, FormControl, AbstractControl, ValidationErrors, ValidatorFn, FormGroup, NG_VALUE_ACCESSOR } from '@angular/forms';
import { AuditLogSettings } from 'ngx-data-table';
import { MatDialog } from '@angular/material/dialog';
import { Utilities } from 'utilities';
import moment from 'moment';


@Component({
    selector: 'c-date-time-picker',
    templateUrl: './c-date-time-picker.component.html',
    providers: [
        {
            provide: NG_VALUE_ACCESSOR,
            useExisting: forwardRef(() => CDateTimePickerComponent),
            multi: true
        }
    ]
})
export class CDateTimePickerComponent implements ControlValueAccessor {


    public readonly dateTimeFormat = 'YYYY-MM-DD HH:mm';
    public readonly dateFormat = 'YYYY-MM-DD';
    public readonly timeFormat = 'HH:mm';


    @Input()
    label: string = "";

    @Input()
    labelType: 'inside' | 'outside' = "inside";

    @Input()
    minDate: SqDateTime | null | undefined = null;

    @Input()
    maxDate: SqDateTime | null | undefined = null;

    @Input()
    formControlName!: string;

    @Input()
    floatLabel: boolean = false;

    @Input()
    placeholder: string = "";

    @Input()
    autoPlaceholder = false;

    @Input()
    autocomplete: string = "off";

    @Input()
    enabled: boolean = true;

    @Input()
    mandatory: boolean = false;

    @Input()
    value: any;

    @Input()
    public error: string | null = null;

    @Input()
    readonly: boolean | "" = false;

    @Input()
    auditLogHistorySettings?: AuditLogSettings;

    @Input()
    excludeTimeZone: boolean = true;

    @Input()
    required: boolean = true;


    @ViewChild('selectDateTemplate', { static: false }) private selectDateTemplate?: TemplateRef<any>;


    private _selectedDate: SqDateTime | null | undefined;
    get selectedDate(): SqDateTime | null | undefined {
        return this._selectedDate;
    }
    set selectedDate(value: SqDateTime | null | undefined) {
        if (value instanceof SqDateTime) {
            this._selectedDate = value;
        } else {
            this._selectedDate = null;
        }

    }
    onChange: any = () => { };
    onTouched: any = () => { };

    writeValue(value: any): void {
        this.selectedDate = value;
        this.dateTimeInInputControl.setValue(this.selectedDate?.format(this.dateTimeFormat));
        this.setFormControlsInDialogue();
    }
    registerOnChange(fn: any): void {
        this.onChange = fn;
    }

    registerOnTouched(fn: any): void {
        this.onTouched = fn;
    }

    private setFormControlsInDialogue() {
        this.dateInDialogueControl.setValue(this.selectedDate?.format(this.dateFormat));
        this.timeInDialogueControl.setValue(Utilities.parseTimeSpan(this.selectedDate?.format(this.timeFormat)) ?? new TimeSpan(0, 0, 0));
        this._matCalendarDate = this.selectedDate;
    }

    dateTimeInInputControl = new FormControl<string | null | undefined>('', [this.dateTimeValidator()]);;

    dateInDialogueControl = new FormControl<string | null | undefined>('', [this.dateValidator()]);
    timeInDialogueControl = new FormControl<TimeSpan | null | undefined>(new TimeSpan(0, 0, 0), [this.timeValidator()]);

    dateTimeInDialogueForm: FormGroup = new FormGroup({
        'date': this.dateInDialogueControl,
        'time': this.timeInDialogueControl
    });

    private _matCalendarDate: SqDateTime | null | undefined;
    public get matCalendarDate(): SqDateTime | null | undefined {
        return this._matCalendarDate;
    }
    public set matCalendarDate(date: SqDateTime | null | undefined) {
        this.dateInDialogueControl.setValue(date?.format(this.dateFormat));
    }


    buttonDisabled = () => this.dateInDialogueControl.invalid || this.timeInDialogueControl.invalid;

    constructor(
        private dialog: MatDialog) {


    }

    public openCalendar() {

        this.setFormControlsInDialogue();

        const data: PageDialogConfig = {
            title: 'Pick Date & Time',
            mainActionButtonConfigs: {
                title: 'SET',
                visible: true,
                color: 'primary',
            },
            onConfirm: this.setDateTimeFromDialogue,
            component: this.selectDateTemplate,
            isFullScreen: false,
            customCssClasses: "md:w-full lg:w-full",
            mainButtonDisabled: this.buttonDisabled,
        };
        this.dialog.open(PageDialogComponent, {
            data: data
        });
    }

    dateTimeValidator(): ValidatorFn {
        return (control: AbstractControl): ValidationErrors | null => {
            const value: string | null | undefined = control.value;
            if (!value?.length) {
                if (this.required) {
                    return { message: 'Date is required' };
                } else {
                    return null;
                }
            }

            const date = moment(`${value}`, this.dateTimeFormat, true);
            if (!date.isValid()) {
                return { message: 'Invalid date' };
            }
            return null;
        };
    }
    dateValidator(): ValidatorFn {
        return (control: AbstractControl): ValidationErrors | null => {
            const value: string | null | undefined = control.value;
            if (!value?.length) {
                if (this.required) {
                    return { message: 'Date is required' };
                } else {
                    return null;
                }
            }

            const date = moment(`${value}`, this.dateFormat, true);
            if (!date.isValid()) {
                return { message: 'Invalid date' };
            }
            return null;
        };
    }

    timeValidator(): ValidatorFn {
        return (control: AbstractControl): ValidationErrors | null => {
            let time: TimeSpan | string | null | undefined = control.value;
            if (!time) {
                if (this.required) {
                    return { message: 'Time is required' };
                } else {
                    return null;
                }
            }

            if (time instanceof TimeSpan) {
                return null;
            }

            const timeParsed = moment(time, this.timeFormat, true);
            if (!timeParsed.isValid()) {
                return { message: 'Invalid time format' };
            }
            return null;
        };
    }

    dateTimeChanged = () => {
        if (this.dateTimeInInputControl.invalid)
            return;

        console.log(this.dateTimeInInputControl.value);
        if (this.dateTimeInInputControl.value) {
            this.selectedDate = new SqDateTime(this.excludeTimeZone, moment(`${this.dateTimeInInputControl.value!}`, this.dateTimeFormat, true).toDate());
        } else {
            this.selectedDate = null;
        }

        this.setFormControlsInDialogue();

        this.onChange(this.selectedDate);
        this.onTouched();


    }
    dateChanged = () => {
        if (this.dateInDialogueControl.invalid || !this.dateInDialogueControl.value)
            return;

        let dateStr = this.dateInDialogueControl.value!;
        const date = new SqDateTime(this.excludeTimeZone, moment(`${this.dateInDialogueControl.value!}`, this.dateFormat, true).toDate());

        this._matCalendarDate = date;
    }

    setDateTimeFromDialogue = () => {
        if (this.dateInDialogueControl.valid && this.timeInDialogueControl.valid) {

            if (this.dateInDialogueControl.value && this.timeInDialogueControl.value) {
                const date = moment(`${this.dateInDialogueControl.value!} ${this.timeInDialogueControl.value!.toString()}`, this.dateTimeFormat, true).toDate();
                this.selectedDate = new SqDateTime(this.excludeTimeZone, date);
            } else {
                this.selectedDate = null;
            }

            this.dateTimeInInputControl.setValue(this.selectedDate?.format(this.dateTimeFormat));

            this.onChange(this._selectedDate);
            this.onTouched();
        }
    }
}
