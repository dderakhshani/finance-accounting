import { Inject } from "@angular/core";
import { ServiceResult } from "../../../../../core/models/service-result";
import { IRequest, IRequestHandler } from "../../../../../core/services/mediator/interfaces";
import { HttpService } from "../../../../../core/services/http/http.service";
import { ApplicationError } from "../../../../../core/exceptions/application-error";
import { MediatorHandler } from "../../../../../core/services/mediator/decorator";
import { NotificationService } from "../../../../../shared/services/notification/notification.service";
import { ValidationRule } from "../../../../../core/validation/validation-rule";
import { HttpRequest } from "../../../../../core/services/http/http-request";
import { PrivateMessageModel } from "../../../entities/private-message";
import { PaginatedList } from "src/app/core/models/paginated-list";

export class GetPrivateMessageQuery extends IRequest<GetPrivateMessageQuery, PaginatedList<PrivateMessageModel>> {
    // public ticketId: number = 0;
    constructor(public ticketDetailId: number) {
        super();
    }


    mapFrom(entity: PaginatedList<PrivateMessageModel>): GetPrivateMessageQuery {
        throw new ApplicationError(GetPrivateMessageQuery.name, this.mapTo.name, 'Not Implemented Yet')
    }

    mapTo(): PaginatedList<PrivateMessageModel> {
        throw new ApplicationError(GetPrivateMessageQuery.name, this.mapTo.name, 'Not Implemented Yet')
    }

    get url(): string {
        return "/ticketing/Ticket/GetPrivetMessageList";
    }

    get validationRules(): ValidationRule[] {
        return [];
    }
}

@MediatorHandler(GetPrivateMessageQueryHandler.name)
export class GetPrivateMessageQueryHandler implements IRequestHandler<GetPrivateMessageQuery, PaginatedList<PrivateMessageModel>> {
    constructor(
        @Inject(HttpService) private _httpService: HttpService,
        @Inject(NotificationService) private _notificationService: NotificationService,
    ) {
    }

    async Handle(request: GetPrivateMessageQuery): Promise<PaginatedList<PrivateMessageModel>> {
        let httpRequest: HttpRequest<GetPrivateMessageQuery> = new HttpRequest<GetPrivateMessageQuery>(request.url + "?ticketDitailId=" + request.ticketDetailId, request);
        return await this._httpService.Get<ServiceResult<PaginatedList<PrivateMessageModel>>>(httpRequest).toPromise().then(response => {
            return response.objResult
        })
    }
}
