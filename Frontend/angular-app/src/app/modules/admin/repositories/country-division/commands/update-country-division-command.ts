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

export class UpdateCountryDivisionCommand extends IRequest<UpdateCountryDivisionCommand, CountryDivision> {
  // TODO Add request body
  constructor() {
    super();
  }

  mapFrom(entity: CountryDivision): UpdateCountryDivisionCommand {
    throw new ApplicationError(UpdateCountryDivisionCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): CountryDivision {
    throw new ApplicationError(UpdateCountryDivisionCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/admin/countrydivision/update";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(UpdateCountryDivisionCommandHandler.name)
export class UpdateCountryDivisionCommandHandler implements IRequestHandler<UpdateCountryDivisionCommand, CountryDivision> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: UpdateCountryDivisionCommand): Promise<CountryDivision> {
    let httpRequest: HttpRequest<UpdateCountryDivisionCommand> = new HttpRequest<UpdateCountryDivisionCommand>(request.url, request);

    return await this._httpService.Put<UpdateCountryDivisionCommand, ServiceResult<CountryDivision>>(httpRequest).toPromise().then(response => {
      // this._notificationService.showHttpSuccessMessage()
      return response.objResult
    })
  }
}
