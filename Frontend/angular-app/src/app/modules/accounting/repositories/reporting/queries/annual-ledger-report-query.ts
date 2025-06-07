import { IRequest, IRequestHandler } from "src/app/core/services/mediator/interfaces";
import { HttpService } from "src/app/core/services/http/http.service";
import { ApplicationError } from "src/app/core/exceptions/application-error";
import { ValidationRule } from "src/app/core/validation/validation-rule";
import { Inject } from "@angular/core";
import { MediatorHandler } from "../../../../../core/services/mediator/decorator";
import { HttpRequest } from "../../../../../core/services/http/http-request";
import { ServiceResult } from "src/app/core/models/service-result";
import { AnnualLedgerReportModel } from "../../annual-ledger/annual-ledger-report-model";

export class AnnualLedgerReportQuery extends IRequest<AnnualLedgerReportQuery, AnnualLedgerReportModel> {

    constructor(public companyId: number | undefined, public yearIds: any[], public voucherDateFrom: Date,
        public voucherDateTo: Date | undefined, public accountHeadIds: number[]) {
        super();
    }
    mapFromSelf(query: AnnualLedgerReportQuery) {
        this.mapBasics(query, this);
        return this;
    }

    mapFrom(entity: AnnualLedgerReportModel): AnnualLedgerReportQuery {
        throw new ApplicationError(AnnualLedgerReportQuery.name, this.mapTo.name, 'Not Implemented Yet')
    }

    mapTo(): AnnualLedgerReportModel {
        throw new ApplicationError(AnnualLedgerReportQuery.name, this.mapTo.name, 'Not Implemented Yet')
    }

    get url(): string {
        return "/accounting/Reporting/AnnualLedgerReport";
    }

    get validationRules(): ValidationRule[] {
        return [];
    }
}

@MediatorHandler(AnnualLedgerReportQueryHandler.name)
export class AnnualLedgerReportQueryHandler implements IRequestHandler<AnnualLedgerReportQuery, AnnualLedgerReportModel> {
    constructor(
        @Inject(HttpService) private _httpService: HttpService
    ) {
    }

    async Handle(request: AnnualLedgerReportQuery): Promise<AnnualLedgerReportModel> {
        let httpRequest: HttpRequest<AnnualLedgerReportQuery> = new HttpRequest<AnnualLedgerReportQuery>(request.url, request);
        return await this._httpService.Post<AnnualLedgerReportQuery, ServiceResult<AnnualLedgerReportModel>>(httpRequest).toPromise().then(response => {
            return response.objResult;
        })
    }
}