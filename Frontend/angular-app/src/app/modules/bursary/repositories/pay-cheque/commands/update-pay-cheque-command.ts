import {Inject} from "@angular/core";
import {ServiceResult} from "../../../../../core/models/service-result";
import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {HttpService} from "../../../../../core/services/http/http.service";
import {ApplicationError} from "../../../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {HttpRequest} from "../../../../../core/services/http/http-request";
import {NotificationService} from "../../../../../shared/services/notification/notification.service";
import {PayCheque} from "../../../entities/pay-cheque";


export class UpdatePayChequeCommand extends IRequest<UpdatePayChequeCommand, PayCheque> {
  public id: number | undefined = undefined;
  public bankAccountId: number | undefined = undefined;
  public bankAccountTitle: string | undefined = undefined;
  public title: string | undefined = undefined;
  public chequeTypeBaseId: number | undefined = undefined;
  public sheetsCount: number | undefined = undefined;
  public chequeNumberIdentification: string | undefined = undefined;
  public ownerEmployeeId: number | undefined = undefined;
  public setOwnerTime: Date | undefined = undefined;
  public isFinished: boolean | undefined = undefined;
  constructor() {
    super();
  }

  mapFrom(entity: PayCheque): UpdatePayChequeCommand {
    this.mapBasics(entity, this);
    return this;
  }

  mapTo(): PayCheque {
    throw new ApplicationError(UpdatePayChequeCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/bursary/payCheques/update";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(UpdatePayChequeCommandHandler.name)
export class UpdatePayChequeCommandHandler implements IRequestHandler<UpdatePayChequeCommand, PayCheque> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService
  ) {
  }

  async Handle(request: UpdatePayChequeCommand): Promise<PayCheque> {
    let httpRequest: HttpRequest<UpdatePayChequeCommand> = new HttpRequest<UpdatePayChequeCommand>(request.url, request);
    // httpRequest.Query  += ;
    // httpRequest.Headers = httpRequest.Headers.append()

    return await this._httpService.Put<UpdatePayChequeCommand, ServiceResult<PayCheque>>(httpRequest).toPromise().then(response => {
      this._notificationService.showSuccessMessage()
      return response.objResult
    })
  }
}
