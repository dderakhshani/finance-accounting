import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';


@Component({
    selector: 'c-button',
    templateUrl: './c-button.component.html',
    styleUrls: ['./c-button.component.scss']
})
export class CButtonComponent implements OnInit {


    @Input()
    label: string = "";

    @Input()
    isWaitng = false;

    @Input()
    enabled: boolean = true;

    @Input()
    isFlat: boolean = false;

    @Input()
    iconName!: string;

    @Input()
    color: string = 'primary';

    @Input()
    cssClass: string = "";

    @Input()
    persmissionName?: string;

    @Output()
    onClick = new EventEmitter<CButtonComponent>();

    constructor() {

    }

    ngOnInit(): void {
        // this.authenticationService.currentUser.pe
    }

    onButtonClicked() {
        this.onClick.emit(this);
    }

}
