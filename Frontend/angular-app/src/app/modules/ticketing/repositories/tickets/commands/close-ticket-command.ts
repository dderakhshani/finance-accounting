import { Inject } from "@angular/core";
import { ServiceResult } from "../../../../../core/models/service-result";
import { IRequest, IRequestHandler } from "../../../../../core/services/mediator/interfaces";
import { HttpService } from "../../../../../core/services/http/http.service";
import { ApplicationError } from "../../../../../core/exceptions/application-error";
import { MediatorHandler } from "../../../../../core/services/mediator/decorator";
import { NotificationService } from "../../../../../shared/services/notification/notification.service";
import { ValidationRule } from "../../../../../core/validation/validation-rule";
import { HttpRequest } from "../../../../../core/services/http/http-request";

export class CloseTicketCommand extends IRequest<CloseTicketCommand, number>{

    constructor(public TicketId:number) {
        super();
    }

    mapFrom(entity: number): CloseTicketCommand {
        throw new ApplicationError(CloseTicketCommand.name, this.mapTo.name, 'Not Implemented Yet')
    }

    mapTo(): number {
        throw new ApplicationError(CloseTicketCommand.name, this.mapTo.name, 'Not Implemented Yet')
    }

    get url(): string {
        return "/ticketing/Ticket/CloseTicket";
    }

    get validationRules(): ValidationRule[] {
        return [];
    }
}

@MediatorHandler(CloseTicketCommandHandler.name)
export class CloseTicketCommandHandler implements IRequestHandler<CloseTicketCommand, number> {
    constructor(
        @Inject(HttpService) private _httpService: HttpService,
        @Inject(NotificationService) private _notificationService: NotificationService,
    ) {
    }

    async Handle(request: CloseTicketCommand): Promise<number> {
        let httpRequest: HttpRequest<CloseTicketCommand> = new HttpRequest<CloseTicketCommand>(request.url, request);

        return await this._httpService.Post<CloseTicketCommand, ServiceResult<number>>(httpRequest).toPromise().then(response => {
            this._notificationService.showSuccessMessage()
            return response.objResult
        })

    }
}
