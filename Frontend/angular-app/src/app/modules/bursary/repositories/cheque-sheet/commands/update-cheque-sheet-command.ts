import {Inject} from "@angular/core";
import {ServiceResult} from "../../../../../core/models/service-result";
import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {HttpService} from "../../../../../core/services/http/http.service";
import {ApplicationError} from "../../../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {HttpRequest} from "../../../../../core/services/http/http-request";
import {NotificationService} from "../../../../../shared/services/notification/notification.service";
import {ChequeSheet} from "../../../entities/cheque-sheet";


export class UpdateChequeSheetCommand extends IRequest<UpdateChequeSheetCommand, ChequeSheet> {

  constructor() {
    super();
  }

  mapFrom(entity: ChequeSheet): UpdateChequeSheetCommand {
    this.mapBasics(entity, this);
    return this;
  }

  mapTo(): ChequeSheet {
    throw new ApplicationError(UpdateChequeSheetCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/bursary/chequeSheets/update";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(UpdateChequeSheetCommandHandler.name)
export class UpdateChequeSheetCommandHandler implements IRequestHandler<UpdateChequeSheetCommand, ChequeSheet> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService
  ) {
  }

  async Handle(request: UpdateChequeSheetCommand): Promise<ChequeSheet> {
    let httpRequest: HttpRequest<UpdateChequeSheetCommand> = new HttpRequest<UpdateChequeSheetCommand>(request.url, request);
    // httpRequest.Query  += ;
    // httpRequest.Headers = httpRequest.Headers.append()

    return await this._httpService.Put<UpdateChequeSheetCommand, ServiceResult<ChequeSheet>>(httpRequest).toPromise().then(response => {
      this._notificationService.showSuccessMessage()
      return response.objResult
    })
  }
}
