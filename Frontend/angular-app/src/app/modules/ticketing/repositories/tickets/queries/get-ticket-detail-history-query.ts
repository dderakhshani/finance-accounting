import { Inject } from "@angular/core";
import { ServiceResult } from "../../../../../core/models/service-result";
import { IRequest, IRequestHandler } from "../../../../../core/services/mediator/interfaces";
import { HttpService } from "../../../../../core/services/http/http.service";
import { ApplicationError } from "../../../../../core/exceptions/application-error";
import { MediatorHandler } from "../../../../../core/services/mediator/decorator";
import { NotificationService } from "../../../../../shared/services/notification/notification.service";
import { ValidationRule } from "../../../../../core/validation/validation-rule";
import { HttpRequest } from "../../../../../core/services/http/http-request";
import { PaginatedList } from "src/app/core/models/paginated-list";
import { TicketDetailHistoryModel } from "../../../entities/ticket-detail-history";

export class GetTicketDetailHistoryQuery extends IRequest<GetTicketDetailHistoryQuery, TicketDetailHistoryModel[]> {
    // public ticketId: number = 0;
    constructor(public ticketDetailId: number) {
        super();
    }


    mapFrom(entity: TicketDetailHistoryModel[]): GetTicketDetailHistoryQuery {
        throw new ApplicationError(GetTicketDetailHistoryQuery.name, this.mapTo.name, 'Not Implemented Yet')
    }

    mapTo(): TicketDetailHistoryModel[] {
        throw new ApplicationError(GetTicketDetailHistoryQuery.name, this.mapTo.name, 'Not Implemented Yet')
    }

    get url(): string {
        return "/ticketing/Ticket/GetTicketDetailHistoryList/";
    }

    get validationRules(): ValidationRule[] {
        return [];
    }
}

@MediatorHandler(GetTicketDetailHistoryQueryHandler.name)
export class GetTicketDetailHistoryQueryHandler implements IRequestHandler<GetTicketDetailHistoryQuery, TicketDetailHistoryModel[]> {
    constructor(
        @Inject(HttpService) private _httpService: HttpService,
        @Inject(NotificationService) private _notificationService: NotificationService,
    ) {
    }

    async Handle(request: GetTicketDetailHistoryQuery): Promise<TicketDetailHistoryModel[]> {
        let httpRequest: HttpRequest<GetTicketDetailHistoryQuery> = new HttpRequest<GetTicketDetailHistoryQuery>(request.url + request.ticketDetailId, request);
        // httpRequest.Query  += ;
        // httpRequest.Headers = httpRequest.Headers.append()



        return await this._httpService.Get<ServiceResult<TicketDetailHistoryModel[]>>(httpRequest).toPromise().then(response => {
            return response.objResult
        })
    }
}
