import { Inject } from "@angular/core";
import { ServiceResult } from "../../../../../core/models/service-result";
import { IRequest, IRequestHandler } from "../../../../../core/services/mediator/interfaces";
import { HttpService } from "../../../../../core/services/http/http.service";
import { ApplicationError } from "../../../../../core/exceptions/application-error";
import { MediatorHandler } from "../../../../../core/services/mediator/decorator";
import { NotificationService } from "../../../../../shared/services/notification/notification.service";
import { ValidationRule } from "../../../../../core/validation/validation-rule";
import { HttpRequest } from "../../../../../core/services/http/http-request";

export class ReplyTicketCommand extends IRequest<ReplyTicketCommand, number>{

    public description: string | undefined = undefined;
    public ticketId: number | undefined = undefined;
    public attachedId: number | undefined = undefined;

    constructor() {
        super();
    }

    mapFrom(entity: number): ReplyTicketCommand {
        throw new ApplicationError(ReplyTicketCommand.name, this.mapTo.name, 'Not Implemented Yet')
    }

    mapTo(): number {
        throw new ApplicationError(ReplyTicketCommand.name, this.mapTo.name, 'Not Implemented Yet')
    }

    get url(): string {
        return "/ticketing/Ticket/ReplyTicket";
    }

    get validationRules(): ValidationRule[] {
        return [];
    }
}

@MediatorHandler(ReplyTicketCommandHandler.name)
export class ReplyTicketCommandHandler implements IRequestHandler<ReplyTicketCommand, number> {
    constructor(
        @Inject(HttpService) private _httpService: HttpService,
        @Inject(NotificationService) private _notificationService: NotificationService,
    ) {
    }

    async Handle(request: ReplyTicketCommand): Promise<number> {
        let httpRequest: HttpRequest<ReplyTicketCommand> = new HttpRequest<ReplyTicketCommand>(request.url, request);

        return await this._httpService.Post<ReplyTicketCommand, ServiceResult<number>>(httpRequest).toPromise().then(response => {
            this._notificationService.showSuccessMessage()
            return response.objResult
        })

    }
}
