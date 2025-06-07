import { IRequest, IRequestHandler } from "src/app/core/services/mediator/interfaces";
import { ProfitAndLossRiportModel } from "../../profit-and-loss/profit-and-loss-report-model";
import { HttpService } from "src/app/core/services/http/http.service";
import { ApplicationError } from "src/app/core/exceptions/application-error";
import { ValidationRule } from "src/app/core/validation/validation-rule";


import { Inject } from "@angular/core";
import { MediatorHandler } from "../../../../../core/services/mediator/decorator";
import { HttpRequest } from "../../../../../core/services/http/http-request";
import { ServiceResult } from "src/app/core/models/service-result";

export class ProfitAndLossReportQuery extends IRequest<ProfitAndLossReportQuery, ProfitAndLossRiportModel> {

    constructor(public companyId: number | undefined, public yearIds: any[], public voucherDateFrom: Date | undefined, public voucherDateTo: Date | undefined) {
        super();
    }
    mapFromSelf(query: ProfitAndLossReportQuery) {
        this.mapBasics(query, this);
        return this;
    }

    mapFrom(entity: ProfitAndLossRiportModel): ProfitAndLossReportQuery {
        throw new ApplicationError(ProfitAndLossReportQuery.name, this.mapTo.name, 'Not Implemented Yet')
    }

    mapTo(): ProfitAndLossRiportModel {
        throw new ApplicationError(ProfitAndLossReportQuery.name, this.mapTo.name, 'Not Implemented Yet')
    }

    get url(): string {
        return "/accounting/Reporting/ProfitAndLossReport";
    }

    get validationRules(): ValidationRule[] {
        return [];
    }
}

@MediatorHandler(ProfitAndLossReportQueryHandler.name)
export class ProfitAndLossReportQueryHandler implements IRequestHandler<ProfitAndLossReportQuery, ProfitAndLossRiportModel> {
    constructor(
        @Inject(HttpService) private _httpService: HttpService
    ) {
    }

    async Handle(request: ProfitAndLossReportQuery): Promise<ProfitAndLossRiportModel> {
        let httpRequest: HttpRequest<ProfitAndLossReportQuery> = new HttpRequest<ProfitAndLossReportQuery>(request.url, request);
        return await this._httpService.Post<ProfitAndLossReportQuery, ServiceResult<ProfitAndLossRiportModel>>(httpRequest).toPromise().then(response => {
            return response.objResult;
        })
    }
}