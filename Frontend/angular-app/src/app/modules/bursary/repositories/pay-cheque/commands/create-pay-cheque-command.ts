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

export class CreatePayChequeCommand extends IRequest<CreatePayChequeCommand, PayCheque> {
  public bankAccountId: number | undefined = undefined;
  public bankAccountTitle: string | undefined = undefined;
  public title: string | undefined = undefined;
  public chequeTypeBaseId: number | undefined = undefined;
  public sheetsCount: number | undefined = undefined;
  public chequeNumberIdentification: string | undefined = undefined;
  public ownerEmployeeId: number | undefined = undefined;
  public setOwnerTime: Date | undefined = undefined;
  public isFinished: boolean | undefined = undefined;

  public sheetSeriNumber : string | undefined = undefined;
  public sheetUniqueNumberStart : string | undefined = undefined;
  public chequeSheetTypeBaseId : number | undefined = undefined;


  constructor() {
    super();
  }

  mapFrom(entity: PayCheque): CreatePayChequeCommand {
    throw new ApplicationError(CreatePayChequeCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): PayCheque {
    throw new ApplicationError(CreatePayChequeCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/bursary/payCheques/add";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(CreatePayChequeCommandHandler.name)
export class CreatePayChequeCommandHandler implements IRequestHandler<CreatePayChequeCommand, PayCheque> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService
  ) {
  }

  async Handle(request: CreatePayChequeCommand): Promise<PayCheque> {
    let httpRequest: HttpRequest<CreatePayChequeCommand> = new HttpRequest<CreatePayChequeCommand>(request.url, request);
    // httpRequest.Query  += ;
    // httpRequest.Headers = httpRequest.Headers.append()

    return await this._httpService.Post<CreatePayChequeCommand, ServiceResult<PayCheque>>(httpRequest).toPromise().then(response => {
      this._notificationService.showSuccessMessage()
      return response.objResult
    })
  }
}
