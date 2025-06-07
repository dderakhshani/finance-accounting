import { ApplicationError } from "../../../../../core/exceptions/application-error";
import { Inject } from "@angular/core";
import { ServiceResult } from "../../../../../core/models/service-result";
import { IRequest, IRequestHandler } from "../../../../../core/services/mediator/interfaces";
import { HttpService } from "../../../../../core/services/http/http.service";
import { MediatorHandler } from "../../../../../core/services/mediator/decorator";
import { ValidationRule } from "../../../../../core/validation/validation-rule";
import { HttpRequest } from "../../../../../core/services/http/http-request";
import { PaginatedList } from "../../../../../core/models/paginated-list";
import { GetUserByRoleIdModel } from "../../../entities/get-user";
import { TicketModel } from "../../../entities/ticket";


export class GetTicketById extends IRequest<GetTicketById, TicketModel> {
    ticketId: number = 0;

    constructor(ticketId: number) {
        super();
        this.ticketId = ticketId;
    }


    mapFrom(entity: TicketModel): GetTicketById {
        throw new ApplicationError(GetTicketById.name, this.mapTo.name, 'Not Implemented Yet')
    }

    mapTo(): TicketModel {
        throw new ApplicationError(GetTicketById.name, this.mapTo.name, 'Not Implemented Yet')
    }

    get url(): string {
        return "/ticketing/Ticket/GetTicketById/";
    }

    get validationRules(): ValidationRule[] {
        return [];
    }
}

@MediatorHandler(GetTicketByIdHandler.name)
export class GetTicketByIdHandler implements IRequestHandler<GetTicketById, TicketModel> {
    constructor(
        @Inject(HttpService) private _httpService: HttpService) {
    }

    async Handle(request: GetTicketById): Promise<TicketModel> {
        let httpRequest: HttpRequest<GetTicketById> = new HttpRequest<GetTicketById>(request.url + request.ticketId, request);


        return await this._httpService.Get<ServiceResult<TicketModel>>(httpRequest).toPromise().then(response => {
            return response.objResult
        })

        // return await this._httpService.Get<GetAllRoleQuery, ServiceResult<PaginatedList<GetAllRoleModel>>>(httpRequest).toPromise().then(response => {
        //   return response.objResult
        // })


    }
}
