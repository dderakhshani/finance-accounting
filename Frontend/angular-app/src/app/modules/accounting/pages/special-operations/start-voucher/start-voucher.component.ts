import { Component } from '@angular/core';
import { Year } from "../../../../admin/entities/year";
import { Mediator } from "../../../../../core/services/mediator/mediator.service";
import {
    CreateStartVoucherHeadCommand
} from "../../../repositories/voucher-head/commands/create-start-voucher-head-command";
import { GetYearsQuery } from "../../../../admin/repositories/year/queris/get-years-query";
import { BaseComponent } from "../../../../../core/abstraction/base.component";

@Component({
    selector: 'app-start-voucher',
    templateUrl: './start-voucher.component.html',
    styleUrls: ['./start-voucher.component.scss']
})
export class StartVoucherComponent extends BaseComponent {


    previousYear!: Year;
    years: Year[] = [];
    me !: any;

    constructor(
        private _mediator: Mediator
    ) {
        super();
        this.request = new CreateStartVoucherHeadCommand();
    }

    async ngOnInit() {

        await this.resolve();
    }
    async resolve() {
        await this.initialize();

    }


    async initialize(params?: any) {
        await this._mediator.send(new GetYearsQuery()).then(res => {
            this.years = res.data
        });
        this.request = new CreateStartVoucherHeadCommand();
    }

    async add(param?: any) {

        var me = this;
        this.isLoading = true;
        try {
            await me._mediator.send((<CreateStartVoucherHeadCommand>me.request));
            this.isLoading = false;

        } catch {
            this.isLoading = false;

        }
    }
    get(param?: any) {

    }
    update(param?: any) {

    }
    delete(param?: any) {

    }
    close() {

    }
}
