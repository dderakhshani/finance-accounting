import {Inject} from "@angular/core";
import {ServiceResult} from "../../../../../core/models/service-result";
import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {HttpService} from "../../../../../core/services/http/http.service";
import {ApplicationError} from "../../../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {HttpRequest} from "../../../../../core/services/http/http-request";
import {NotificationService} from "../../../../../shared/services/notification/notification.service";
import {Bank} from "../../../entities/bank";

export class UpdateBankCommand extends IRequest<UpdateBankCommand, Bank> {
  public id:number | undefined = undefined;
  public code: string | undefined = undefined;
  public title: string | undefined = undefined;
  public globalCode: string | undefined = undefined;
  public typeBaseId: number | undefined = undefined;
  public descriptions: string | undefined = undefined;
  public bankImage: string | undefined = undefined;
  constructor() {
    super();
  }

  mapFrom(entity: Bank): UpdateBankCommand {
    this.mapBasics(entity, this);
    return this;
  }

  mapTo(): Bank {
    throw new ApplicationError(UpdateBankCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/bursary/banks/update";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(UpdateBankCommandHandler.name)
export class UpdateBankCommandHandler implements IRequestHandler<UpdateBankCommand, Bank> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService
  ) {
  }

  async Handle(request: UpdateBankCommand): Promise<Bank> {
    let httpRequest: HttpRequest<UpdateBankCommand> = new HttpRequest<UpdateBankCommand>(request.url, request);
    // httpRequest.Query  += ;
    // httpRequest.Headers = httpRequest.Headers.append()

    return await this._httpService.Put<UpdateBankCommand, ServiceResult<Bank>>(httpRequest).toPromise().then(response => {
      this._notificationService.showSuccessMessage()
      return response.objResult
    })
  }
}
