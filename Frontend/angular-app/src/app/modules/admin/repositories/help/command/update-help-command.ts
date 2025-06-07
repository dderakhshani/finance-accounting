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

export class UpdateHelpCommand extends IRequest<UpdateHelpCommand, Help>{
  public id: number | undefined = undefined;
  public menuItemId: number | undefined = undefined;
  public contents: string | undefined = undefined;

  constructor() {
    super();
  }

  mapFrom(entity: Help): UpdateHelpCommand {
    this.mapBasics(entity, this);

    return this;
  }

  mapTo(): Help {
    throw new ApplicationError(UpdateHelpCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/admin/help/update"
  }

  get validationRules() : ValidationRule[] {
    return [];
  }
}

@MediatorHandler(UpdateHelpCommandHandler.name)
export class UpdateHelpCommandHandler implements IRequestHandler<UpdateHelpCommand, Help> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: UpdateHelpCommand): Promise<Help> {
    let httpRequest: HttpRequest<UpdateHelpCommand> = new HttpRequest<UpdateHelpCommand>(request.url, request);

    return await this._httpService.Put<UpdateHelpCommand, ServiceResult<Help>>(httpRequest).toPromise().then(response => {
      this._notificationService.showSuccessMessage()
      return response.objResult
    })
  }
}
