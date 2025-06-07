import {CountryDivision} from "../../../entities/countryDivision";
import {Inject} from "@angular/core";
import {ServiceResult} from "../../../../../core/models/service-result";
import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {HttpService} from "../../../../../core/services/http/http.service";
import {ApplicationError} from "../../../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {NotificationService} from "../../../../../shared/services/notification/notification.service";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {HttpRequest} from "../../../../../core/services/http/http-request";

export class DeleteCountryDivisionCommand extends IRequest<DeleteCountryDivisionCommand, CountryDivision> {

  constructor(public entityId:number) {
    super();
  }

  mapFrom(entity: CountryDivision): DeleteCountryDivisionCommand {
    throw new ApplicationError(DeleteCountryDivisionCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): CountryDivision {
    throw new ApplicationError(DeleteCountryDivisionCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/admin/countryDivision/delete";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(DeleteCountryDivisionCommandHandler.name)
export class DeleteCountryDivisionCommandHandler implements IRequestHandler<DeleteCountryDivisionCommand, CountryDivision> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: DeleteCountryDivisionCommand): Promise<CountryDivision> {
    let httpRequest: HttpRequest<DeleteCountryDivisionCommand> = new HttpRequest<DeleteCountryDivisionCommand>(request.url, request);
    httpRequest.Query  += `id=${request.entityId}`;

    return await this._httpService.Delete<ServiceResult<CountryDivision>>(httpRequest).toPromise().then(response => {
      // this._notificationService.showHttpSuccessMessage()
      return response.objResult
    })
  }
}
