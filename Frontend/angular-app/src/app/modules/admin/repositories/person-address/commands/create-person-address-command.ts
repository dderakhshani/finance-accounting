import {Inject} from "@angular/core";
import {ServiceResult} from "../../../../../core/models/service-result";
import {PersonAddress} from "../../../entities/person-address";
import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {HttpService} from "../../../../../core/services/http/http.service";
import {ApplicationError} from "../../../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {NotificationService} from "../../../../../shared/services/notification/notification.service";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {HttpRequest} from "../../../../../core/services/http/http-request";

export class CreatePersonAddressCommand extends IRequest<CreatePersonAddressCommand, PersonAddress> {
  public personId:number | undefined = undefined;
  public typeBaseId:number | undefined = undefined;
  public address:string | undefined = undefined;
  public countryDivisionId:number | undefined = undefined;
  public postalCode:string | undefined = undefined;
  // Todo Add Mobile Model
  // public mobile:any[] = [];
  constructor() {
    super();
  }

  mapFrom(entity: PersonAddress): CreatePersonAddressCommand {
    throw new ApplicationError(CreatePersonAddressCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): PersonAddress {
    throw new ApplicationError(CreatePersonAddressCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/admin/personAddress/add";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(CreatePersonAddressCommandHandler.name)
export class CreatePersonAddressCommandHandler implements IRequestHandler<CreatePersonAddressCommand, PersonAddress> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: CreatePersonAddressCommand): Promise<PersonAddress> {
    let httpRequest: HttpRequest<CreatePersonAddressCommand> = new HttpRequest<CreatePersonAddressCommand>(request.url, request);

    return await this._httpService.Post<CreatePersonAddressCommand, ServiceResult<PersonAddress>>(httpRequest).toPromise().then(response => {
      this._notificationService.showSuccessMessage()
      return response.objResult
    })
  }
}
