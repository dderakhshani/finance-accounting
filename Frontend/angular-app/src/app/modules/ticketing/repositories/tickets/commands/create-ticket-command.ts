import { Inject } from "@angular/core";
import { ServiceResult } from "../../../../../core/models/service-result";
import { IRequest, IRequestHandler } from "../../../../../core/services/mediator/interfaces";
import { HttpService } from "../../../../../core/services/http/http.service";
import { ApplicationError } from "../../../../../core/exceptions/application-error";
import { MediatorHandler } from "../../../../../core/services/mediator/decorator";
import { NotificationService } from "../../../../../shared/services/notification/notification.service";
import { ValidationRule } from "../../../../../core/validation/validation-rule";
import { HttpRequest } from "../../../../../core/services/http/http-request";

export class CreateTicketCommand extends IRequest<CreateTicketCommand, number>{

    public title: string | undefined = undefined;
    public topicId: number | undefined = undefined;
    public roleId: number | undefined = undefined;
    public priority: number | undefined = undefined;
    public receiverUserId: number | undefined = undefined;
    public description: string | undefined = undefined;
    public attachedId: number | undefined = undefined;
    public pageUrl: string | undefined = undefined;
    public attachmentIds: string | undefined = undefined;
    constructor() {
        super();
    }

    mapFrom(entity: number): CreateTicketCommand {
        throw new ApplicationError(CreateTicketCommand.name, this.mapTo.name, 'Not Implemented Yet')
    }

    mapTo(): number {
        throw new ApplicationError(CreateTicketCommand.name, this.mapTo.name, 'Not Implemented Yet')
    }

    get url(): string {
        return "/ticketing/Ticket/AddTicket";
    }

    get validationRules(): ValidationRule[] {
        return [];
    }
}

@MediatorHandler(CreateTicketCommandHandler.name)
export class CreateTicketCommandHandler implements IRequestHandler<CreateTicketCommand, number> {
    constructor(
        @Inject(HttpService) private _httpService: HttpService,
        @Inject(NotificationService) private _notificationService: NotificationService,
    ) {
    }

    async Handle(request: CreateTicketCommand): Promise<number> {
        let httpRequest: HttpRequest<CreateTicketCommand> = new HttpRequest<CreateTicketCommand>(request.url, request);

        return await this._httpService.Post<CreateTicketCommand, ServiceResult<number>>(httpRequest).toPromise().then(response => {
            this._notificationService.showSuccessMessage()
            return response.objResult
        })

    }
}
