import { Inject } from "@angular/core";
import { ServiceResult } from "../../../../../core/models/service-result";
import { IRequest, IRequestHandler } from "../../../../../core/services/mediator/interfaces";
import { HttpService } from "../../../../../core/services/http/http.service";
import { ApplicationError } from "../../../../../core/exceptions/application-error";
import { MediatorHandler } from "../../../../../core/services/mediator/decorator";
import { NotificationService } from "../../../../../shared/services/notification/notification.service";
import { ValidationRule } from "../../../../../core/validation/validation-rule";
import { HttpRequest } from "../../../../../core/services/http/http-request";
import { TicketDetailModel } from "../../../entities/ticket-detail";
import { PaginatedList } from "src/app/core/models/paginated-list";

export class GetTicketDetailQuery extends IRequest<GetTicketDetailQuery, PaginatedList<TicketDetailModel>> {
    // public ticketId: number = 0;
    constructor(public ticketId: number) {
        super();
    }


    mapFrom(entity: PaginatedList<TicketDetailModel>): GetTicketDetailQuery {
        throw new ApplicationError(GetTicketDetailQuery.name, this.mapTo.name, 'Not Implemented Yet')
    }

    mapTo(): PaginatedList<TicketDetailModel> {
        throw new ApplicationError(GetTicketDetailQuery.name, this.mapTo.name, 'Not Implemented Yet')
    }

    get url(): string {
        return "/ticketing/Ticket/GetTicketDetailList/";
    }

    get validationRules(): ValidationRule[] {
        return [];
    }
}

@MediatorHandler(GetTicketDetailQueryHandler.name)
export class GetTicketDetailQueryHandler implements IRequestHandler<GetTicketDetailQuery, PaginatedList<TicketDetailModel>> {
    constructor(
        @Inject(HttpService) private _httpService: HttpService,
        @Inject(NotificationService) private _notificationService: NotificationService,
    ) {
    }

    async Handle(request: GetTicketDetailQuery): Promise<PaginatedList<TicketDetailModel>> {
        let httpRequest: HttpRequest<GetTicketDetailQuery> = new HttpRequest<GetTicketDetailQuery>(request.url + request.ticketId, request);
        // httpRequest.Query  += ;
        // httpRequest.Headers = httpRequest.Headers.append()



        return await this._httpService.Get<ServiceResult<PaginatedList<TicketDetailModel>>>(httpRequest).toPromise().then(response => {
            return response.objResult
        })
    }
}
