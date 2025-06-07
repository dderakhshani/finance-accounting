import { IRequest, IRequestHandler } from "../../../../../core/services/mediator/interfaces";
import { Help } from "../../../entities/help";
import { ApplicationError } from "../../../../../core/exceptions/application-error";
import { ValidationRule } from "../../../../../core/validation/validation-rule";
import { MediatorHandler } from "../../../../../core/services/mediator/decorator";
import { Inject } from "@angular/core";
import { HttpService } from "../../../../../core/services/http/http.service";
import { HttpRequest } from "../../../../../core/services/http/http-request";
import { ServiceResult } from "../../../../../core/models/service-result";
import {NotificationService} from "../../../../../shared/services/notification/notification.service";

export class CreateHelpCommand extends IRequest<CreateHelpCommand, Help>{

  public menuItemId: number | undefined = undefined;
  public contents: string | undefined = undefined;

  constructor() {
    super();
  }

  mapFrom(entity: Help): CreateHelpCommand {
    throw new ApplicationError(CreateHelpCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): Help {
    throw new ApplicationError(CreateHelpCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/admin/help/add"
  }

  get validationRules() : ValidationRule[] {
    return [];
  }
}

@MediatorHandler(CreateHelpCommandHandler.name)
export class CreateHelpCommandHandler implements IRequestHandler<CreateHelpCommand, Help> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService
  ) {
  }
  async Handle(request: CreateHelpCommand): Promise<Help>{
    let httpRequest : HttpRequest<CreateHelpCommand> = new HttpRequest<CreateHelpCommand>(request.url, request);

    return await this._httpService.Post<CreateHelpCommand, ServiceResult<Help>>(httpRequest).toPromise().then(response => {
      this._notificationService.showSuccessMessage()
      return response.objResult
    })
  }
}
