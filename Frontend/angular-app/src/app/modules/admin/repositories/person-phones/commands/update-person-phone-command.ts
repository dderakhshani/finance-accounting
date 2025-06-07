import {Inject} from "@angular/core";
import {ServiceResult} from "../../../../../core/models/service-result";
import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {HttpService} from "../../../../../core/services/http/http.service";
import {ApplicationError} from "../../../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {PersonPhone} from "../../../entities/person-phone";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {HttpRequest} from "../../../../../core/services/http/http-request";
import {NotificationService} from "../../../../../shared/services/notification/notification.service";

export class UpdatePersonPhoneCommand extends IRequest<UpdatePersonPhoneCommand, PersonPhone> {
  public id: number | undefined = undefined;
  public personId: number | undefined = undefined;
  public phoneTypeBaseId: number | undefined = undefined;
  public phoneNumber: string | undefined = undefined;
  public description: string | undefined = undefined;
  public isDefault: boolean | undefined = undefined;

  constructor() {
    super();
  }

  mapFrom(entity: PersonPhone): UpdatePersonPhoneCommand {
    this.mapBasics(entity,this)
    return this;
  }

  mapTo(): PersonPhone {
    throw new ApplicationError(UpdatePersonPhoneCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/admin/personPhones/update";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(UpdatePersonPhoneCommandHandler.name)
export class UpdatePersonPhoneCommandHandler implements IRequestHandler<UpdatePersonPhoneCommand, PersonPhone> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,

  ) {
  }

  async Handle(request: UpdatePersonPhoneCommand): Promise<PersonPhone> {
    let httpRequest: HttpRequest<UpdatePersonPhoneCommand> = new HttpRequest<UpdatePersonPhoneCommand>(request.url, request);
    // httpRequest.Query  += ;
    // httpRequest.Headers = httpRequest.Headers.append()

    return await this._httpService.Put<UpdatePersonPhoneCommand, ServiceResult<PersonPhone>>(httpRequest).toPromise().then(response => {
      this._notificationService.showSuccessMessage()
      return response.objResult
    })

  }
}
