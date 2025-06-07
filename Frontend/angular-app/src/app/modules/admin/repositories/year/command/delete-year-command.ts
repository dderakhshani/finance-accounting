import {HttpRequest} from "../../../../../core/services/http/http-request";
import {ServiceResult} from "../../../../../core/models/service-result";
import {Year} from "../../../entities/year";
import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {HttpService} from "../../../../../core/services/http/http.service";
import {ApplicationError} from "../../../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {NotificationService} from "../../../../../shared/services/notification/notification.service";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import { Inject } from "@angular/core";

export class DeleteYearCommand extends IRequest<DeleteYearCommand, Year> {

  constructor(public entityId:number) {
    super();
  }

  mapFrom(entity: Year): DeleteYearCommand {
    throw new ApplicationError(DeleteYearCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): Year {
    throw new ApplicationError(DeleteYearCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/admin/year/delete";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(DeleteYearCommandHandler.name)
export class DeleteYearCommandHandler implements IRequestHandler<DeleteYearCommand, Year> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: DeleteYearCommand): Promise<Year> {
    let httpRequest: HttpRequest<DeleteYearCommand> = new HttpRequest<DeleteYearCommand>(request.url, request);
    httpRequest.Query  += `id=${request.entityId}`;


    return await this._httpService.Delete<ServiceResult<Year>>(httpRequest).toPromise().then(response => {
      this._notificationService.showSuccessMessage()
      return response.objResult
    })
  }
}
