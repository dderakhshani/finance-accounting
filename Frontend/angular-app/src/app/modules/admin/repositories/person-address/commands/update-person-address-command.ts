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

export class UpdatePersonAddressCommand extends IRequest<UpdatePersonAddressCommand, PersonAddress> {

  public id:number | undefined = undefined;
  public personId:number | undefined = undefined;
  public typeBaseId:number | undefined = undefined;
  public address:string | undefined = undefined;
  public countryDivisionId:number | undefined = undefined;
  public postalCode:string | undefined = undefined;
  // Todo Add Mobile Model
  public mobile:any[] = [];
  constructor() {
    super();
  }

  mapFrom(entity: PersonAddress): UpdatePersonAddressCommand {
    this.mapBasics(entity,this)
    return this;
  }

  mapTo(): PersonAddress {
    throw new ApplicationError(UpdatePersonAddressCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/admin/personAddress/update";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(UpdatePersonAddressCommandHandler.name)
export class UpdatePersonAddressCommandHandler implements IRequestHandler<UpdatePersonAddressCommand, PersonAddress> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: UpdatePersonAddressCommand): Promise<PersonAddress> {
    let httpRequest: HttpRequest<UpdatePersonAddressCommand> = new HttpRequest<UpdatePersonAddressCommand>(request.url, request);


    return await this._httpService.Put<UpdatePersonAddressCommand, ServiceResult<PersonAddress>>(httpRequest).toPromise().then(response => {
      this._notificationService.showSuccessMessage()
      return response.objResult
    })

  }
}
