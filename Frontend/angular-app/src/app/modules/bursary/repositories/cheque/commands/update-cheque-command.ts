import {Inject} from "@angular/core";
import {ServiceResult} from "../../../../../core/models/service-result";
import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {HttpService} from "../../../../../core/services/http/http.service";
import {ApplicationError} from "../../../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {NotificationService} from "../../../../../shared/services/notification/notification.service";
import {Cheque} from "../../../entities/cheque";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {HttpRequest} from "../../../../../core/services/http/http-request";

export class UpdateChequeCommand extends IRequest<UpdateChequeCommand, Cheque> {

  public id: number | undefined = undefined;
  public sheba: string | undefined = undefined;
  public sheetsCount: number | undefined = undefined;
  public chequeNumberIdentification: string | undefined = undefined;
  public setOwnerTime: Date | undefined = undefined;
  public isFinished: boolean | undefined = undefined;
  public bankAccountId: number | undefined = undefined;


  constructor() {
    super();
  }

  mapFrom(entity: Cheque): UpdateChequeCommand {
    this.mapBasics(entity,this);
    return this;
  }

  mapTo(): Cheque {
    throw new ApplicationError(UpdateChequeCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/bursary/cheque/update";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(UpdateChequeCommandHandler.name)
export class UpdateChequeCommandHandler implements IRequestHandler<UpdateChequeCommand, Cheque> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: UpdateChequeCommand): Promise<Cheque> {
    let httpRequest: HttpRequest<UpdateChequeCommand> = new HttpRequest<UpdateChequeCommand>(request.url, request);

    return await this._httpService.Put<UpdateChequeCommand, ServiceResult<Cheque>>(httpRequest).toPromise().then(response => {
      this._notificationService.showSuccessMessage()
      return response.objResult
    })

  }
}
