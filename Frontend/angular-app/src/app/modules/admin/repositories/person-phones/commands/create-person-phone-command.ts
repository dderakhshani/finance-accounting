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

export class CreatePersonPhoneCommand extends IRequest<CreatePersonPhoneCommand, PersonPhone> {

  public personId: number | undefined = undefined;
  public phoneTypeBaseId: number | undefined = undefined;
  public phoneNumber: string | undefined = undefined;
  public description: string | undefined = undefined;
  public isDefault: boolean | undefined = undefined;

  constructor() {
    super();
  }

  mapFrom(entity: PersonPhone): CreatePersonPhoneCommand {
    throw new ApplicationError(CreatePersonPhoneCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): PersonPhone {
    throw new ApplicationError(CreatePersonPhoneCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/admin/personPhones/add";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(CreatePersonPhoneCommandHandler.name)
export class CreatePersonPhoneCommandHandler implements IRequestHandler<CreatePersonPhoneCommand, PersonPhone> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,

  ) {
  }

  async Handle(request: CreatePersonPhoneCommand): Promise<PersonPhone> {
    let httpRequest: HttpRequest<CreatePersonPhoneCommand> = new HttpRequest<CreatePersonPhoneCommand>(request.url, request);


    return await this._httpService.Post<CreatePersonPhoneCommand, ServiceResult<PersonPhone>>(httpRequest).toPromise().then(response => {
      this._notificationService.showSuccessMessage()
      return response.objResult
    })

  }
}
