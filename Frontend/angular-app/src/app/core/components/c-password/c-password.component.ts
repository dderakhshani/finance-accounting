import { Attribute, Component, EventEmitter, Input, OnInit, Optional, Output, Self } from '@angular/core';
import { ControlValueAccessor, UntypedFormControl, FormControlDirective, FormControlName, FormGroupDirective, NgControl, NgModel } from '@angular/forms';
import { Subscription } from 'rxjs';
import { CInputComponent } from '../c-input/c-input.component';

@Component({
    selector: 'c-password',
    templateUrl: './c-password.component.html',
    styleUrls: ['./c-password.component.scss'],

})
export class CPasswordComponent extends CInputComponent implements OnInit, ControlValueAccessor {  

    @Input()
    visibilityPasswordEnabled = true;

    @Input()
    generatePasswordButtonEnabled = true;

    showPassword = false;

    constructor(@Optional() @Self() ngControl: NgControl) {
        super(ngControl);
        this.type = "password";
        
        if (this.ngControl != null) {
            // Setting the value accessor directly (instead of using the providers) to avoid running into a circular import.
            this.ngControl.valueAccessor = this;
        }
    }
       
    generatePassword = () => {
        var generate = (
            length = 20,
            characters = '0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz~!@-#$'
        ) =>
            Array.from(crypto.getRandomValues(new Uint32Array(length)))
                .map((x) => characters[x % characters.length])
                .join('');

        let randomPass = generate(10);
        this.control.setValue(randomPass);
        this.showPassword = true;
        this.visibilityPasswordEnabled = true;
    }
}
