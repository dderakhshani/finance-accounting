import { Inject } from "@angular/core";
import { ServiceResult } from "../../../../../core/models/service-result";
import { IRequest, IRequestHandler } from "../../../../../core/services/mediator/interfaces";
import { HttpService } from "../../../../../core/services/http/http.service";
import { ApplicationError } from "../../../../../core/exceptions/application-error";
import { MediatorHandler } from "../../../../../core/services/mediator/decorator";
import { NotificationService } from "../../../../../shared/services/notification/notification.service";
import { ValidationRule } from "../../../../../core/validation/validation-rule";
import { HttpRequest } from "../../../../../core/services/http/http-request";

export class ForwardTicketCommand extends IRequest<ForwardTicketCommand, number>{

    public message: string | undefined = undefined;
    public ticketDetailId: number | undefined = undefined;
    public secondaryRoleId: number | undefined = undefined;

    constructor() {
        super();
    }

    mapFrom(entity: number): ForwardTicketCommand {
        throw new ApplicationError(ForwardTicketCommand.name, this.mapTo.name, 'Not Implemented Yet')
    }

    mapTo(): number {
        throw new ApplicationError(ForwardTicketCommand.name, this.mapTo.name, 'Not Implemented Yet')
    }

    get url(): string {
        return "/ticketing/Ticket/ForwardTicket";
    }

    get validationRules(): ValidationRule[] {
        return [];
    }
}

@MediatorHandler(ForwardTicketCommandHandler.name)
export class ForwardTicketCommandHandler implements IRequestHandler<ForwardTicketCommand, number> {
    constructor(
        @Inject(HttpService) private _httpService: HttpService,
        @Inject(NotificationService) private _notificationService: NotificationService,
    ) {
    }

    async Handle(request: ForwardTicketCommand): Promise<number> {
        let httpRequest: HttpRequest<ForwardTicketCommand> = new HttpRequest<ForwardTicketCommand>(request.url, request);

        return await this._httpService.Post<ForwardTicketCommand, ServiceResult<number>>(httpRequest).toPromise().then(response => {
            this._notificationService.showSuccessMessage()
            return response.objResult
        })

    }
}
