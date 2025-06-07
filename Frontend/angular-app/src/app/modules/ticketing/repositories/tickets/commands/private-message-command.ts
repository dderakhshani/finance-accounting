import { Inject } from "@angular/core";
import { ServiceResult } from "../../../../../core/models/service-result";
import { IRequest, IRequestHandler } from "../../../../../core/services/mediator/interfaces";
import { HttpService } from "../../../../../core/services/http/http.service";
import { ApplicationError } from "../../../../../core/exceptions/application-error";
import { MediatorHandler } from "../../../../../core/services/mediator/decorator";
import { NotificationService } from "../../../../../shared/services/notification/notification.service";
import { ValidationRule } from "../../../../../core/validation/validation-rule";
import { HttpRequest } from "../../../../../core/services/http/http-request";

export class PrivetMessageCommand extends IRequest<PrivetMessageCommand, number>{

    public message: string | undefined = undefined;
    public ticketDetailId: number | undefined = undefined;

    constructor() {
        super();
    }

    mapFrom(entity: number): PrivetMessageCommand {
        throw new ApplicationError(PrivetMessageCommand.name, this.mapTo.name, 'Not Implemented Yet')
    }

    mapTo(): number {
        throw new ApplicationError(PrivetMessageCommand.name, this.mapTo.name, 'Not Implemented Yet')
    }

    get url(): string {
        return "/ticketing/Ticket/AddPrivetMessage";
    }

    get validationRules(): ValidationRule[] {
        return [];
    }
}

@MediatorHandler(PrivetMessageCommandHandler.name)
export class PrivetMessageCommandHandler implements IRequestHandler<PrivetMessageCommand, number> {
    constructor(
        @Inject(HttpService) private _httpService: HttpService,
        @Inject(NotificationService) private _notificationService: NotificationService,
    ) {
    }

    async Handle(request: PrivetMessageCommand): Promise<number> {
        let httpRequest: HttpRequest<PrivetMessageCommand> = new HttpRequest<PrivetMessageCommand>(request.url, request);

        return await this._httpService.Post<PrivetMessageCommand, ServiceResult<number>>(httpRequest).toPromise().then(response => {
            this._notificationService.showSuccessMessage()
            return response.objResult
        })

    }
}
