import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {HttpRequest} from "../../../../../core/services/http/http-request";
import {NotificationService} from "../../../../../shared/services/notification/notification.service";
import {Inject} from "@angular/core";
import {ServiceResult} from "../../../../../core/models/service-result";
import {HttpService} from "../../../../../core/services/http/http.service";
import {ApplicationError} from "../../../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {Cheque} from "../../../entities/cheque";

export class CreateChequeCommand extends IRequest<CreateChequeCommand, Cheque> {
  public sheba: string | undefined = undefined;
  public sheetsCount: number | undefined = undefined;
  public chequeNumberIdentification: string | undefined = undefined;
  public setOwnerTime: Date | undefined = undefined;
  public isFinished: boolean | undefined = undefined;
  public bankAccountId: number | undefined = undefined;

  constructor() {
    super();
  }

  mapFrom(entity: Cheque): CreateChequeCommand {
    throw new ApplicationError(CreateChequeCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): Cheque {
    throw new ApplicationError(CreateChequeCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/bursary/cheque/add";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(CreateChequeCommandHandler.name)
export class CreateChequeCommandHandler implements IRequestHandler<CreateChequeCommand, Cheque> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: CreateChequeCommand): Promise<Cheque> {
    let httpRequest: HttpRequest<CreateChequeCommand> = new HttpRequest<CreateChequeCommand>(request.url, request);

    return await this._httpService.Post<CreateChequeCommand, ServiceResult<Cheque>>(httpRequest).toPromise().then(response => {
      this._notificationService.showSuccessMessage()
      return response.objResult
    })

  }
}
