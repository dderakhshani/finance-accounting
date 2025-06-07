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


export class DeleteChequeSheetCommand extends IRequest<DeleteChequeSheetCommand, ChequeSheet> {

  constructor(public id: number) {
    super();
  }

  mapFrom(entity: ChequeSheet): DeleteChequeSheetCommand {
    throw new ApplicationError(DeleteChequeSheetCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): ChequeSheet {
    throw new ApplicationError(DeleteChequeSheetCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/bursary/chequeSheets/delete";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(DeleteChequeSheetCommandHandler.name)
export class DeleteChequeSheetCommandHandler implements IRequestHandler<DeleteChequeSheetCommand, ChequeSheet> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService
  ) {
  }

  async Handle(request: DeleteChequeSheetCommand): Promise<ChequeSheet> {
    let httpRequest: HttpRequest<DeleteChequeSheetCommand> = new HttpRequest<DeleteChequeSheetCommand>(request.url, request);
    httpRequest.Query  += `id=${request.id}`;

    return await this._httpService.Delete<ServiceResult<ChequeSheet>>(httpRequest).toPromise().then(response => {
      this._notificationService.showSuccessMessage()
      return response.objResult
    })
  }
}
