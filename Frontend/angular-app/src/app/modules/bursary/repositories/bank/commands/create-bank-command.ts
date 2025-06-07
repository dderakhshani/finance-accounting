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

export class CreateBankCommand extends IRequest<CreateBankCommand, Bank> {
  public code: string | undefined = undefined;
  public title: string | undefined = undefined;
  public globalCode: string | undefined = undefined;
  public typeBaseId: number | undefined = undefined;
  public descriptions: string | undefined = undefined;
  public bankImage: string | undefined = undefined;


  constructor() {
    super();
  }

  mapFrom(entity: Bank): CreateBankCommand {
    throw new ApplicationError(CreateBankCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): Bank {
    throw new ApplicationError(CreateBankCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/bursary/banks/add";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(CreateBankCommandHandler.name)
export class CreateBankCommandHandler implements IRequestHandler<CreateBankCommand, Bank> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService
  ) {
  }

  async Handle(request: CreateBankCommand): Promise<Bank> {
    let httpRequest: HttpRequest<CreateBankCommand> = new HttpRequest<CreateBankCommand>(request.url, request);
    // httpRequest.Query  += ;
    // httpRequest.Headers = httpRequest.Headers.append()

    return await this._httpService.Post<CreateBankCommand, ServiceResult<Bank>>(httpRequest).toPromise().then(response => {
      this._notificationService.showSuccessMessage()
      return response.objResult
    })
  }
}
