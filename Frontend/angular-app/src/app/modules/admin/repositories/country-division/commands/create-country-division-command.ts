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

export class CreateCountryDivisionCommand extends IRequest<CreateCountryDivisionCommand, CountryDivision> {
  // todo Add request body
  constructor() {
    super();
  }

  mapFrom(entity: CountryDivision): CreateCountryDivisionCommand {
    throw new ApplicationError(CreateCountryDivisionCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): CountryDivision {
    throw new ApplicationError(CreateCountryDivisionCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/admin/countryDivision/add";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(CreateCountryDivisionCommandHandler.name)
export class CreateCountryDivisionCommandHandler implements IRequestHandler<CreateCountryDivisionCommand, CountryDivision> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: CreateCountryDivisionCommand): Promise<CountryDivision> {
    let httpRequest: HttpRequest<CreateCountryDivisionCommand> = new HttpRequest<CreateCountryDivisionCommand>(request.url, request);

    return await this._httpService.Post<CreateCountryDivisionCommand, ServiceResult<CountryDivision>>(httpRequest).toPromise().then(response => {
      this._notificationService.showSuccessMessage()
      return response.objResult
    })
  }
}
