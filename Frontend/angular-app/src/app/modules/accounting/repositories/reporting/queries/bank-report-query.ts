import { IRequest, IRequestHandler } from "src/app/core/services/mediator/interfaces";
import { HttpService } from "src/app/core/services/http/http.service";
import { ApplicationError } from "src/app/core/exceptions/application-error";
import { ValidationRule } from "src/app/core/validation/validation-rule";
import { Inject } from "@angular/core";
import { MediatorHandler } from "../../../../../core/services/mediator/decorator";
import { HttpRequest } from "../../../../../core/services/http/http-request";
import { ServiceResult } from "src/app/core/models/service-result";
import { BankReportModel } from "../../bank-report/bank-report-model";

export class BankReportQuery extends IRequest<BankReportQuery, BankReportModel[]> {

    constructor(public voucherDateFrom: Date, public voucherDateTo: Date | undefined) {
        super();
    }
    mapFromSelf(query: BankReportQuery) {
        this.mapBasics(query, this);
        return this;
    }

    mapFrom(entity: BankReportModel[]): BankReportQuery {
        throw new ApplicationError(BankReportQuery.name, this.mapTo.name, 'Not Implemented Yet')
    }

    mapTo(): BankReportModel[] {
        throw new ApplicationError(BankReportQuery.name, this.mapTo.name, 'Not Implemented Yet')
    }

    get url(): string {
        return "/accounting/Reporting/GetBankReport";
    }

    get validationRules(): ValidationRule[] {
        return [];
    }
}

@MediatorHandler(BankReportQueryHandler.name)
export class BankReportQueryHandler implements IRequestHandler<BankReportQuery, BankReportModel[]> {
    constructor(
        @Inject(HttpService) private _httpService: HttpService
    ) {
    }

    async Handle(request: BankReportQuery): Promise<BankReportModel[]> {
        let httpRequest: HttpRequest<BankReportQuery> = new HttpRequest<BankReportQuery>(request.url, request);
        return await this._httpService.Post<BankReportQuery, ServiceResult<BankReportModel[]>>(httpRequest).toPromise().then(response => {
            return response.objResult;
        })
    }
}