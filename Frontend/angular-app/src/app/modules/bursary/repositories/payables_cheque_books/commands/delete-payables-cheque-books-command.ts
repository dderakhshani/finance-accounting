import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {ApplicationError} from "../../../../../core/exceptions/application-error";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {Inject} from "@angular/core";
import {HttpService} from "../../../../../core/services/http/http.service";
import {NotificationService} from "../../../../../shared/services/notification/notification.service";
import {HttpRequest} from "../../../../../core/services/http/http-request";
import {ServiceResult} from "../../../../../core/models/service-result";
import {ChequeBook} from "../../../../bursary/entities/cheque-book";

export class DeletePayablesChequeBooksCommand  extends IRequest<DeletePayablesChequeBooksCommand, ChequeBook> {

  constructor(public id: number) {
    super();
  }

  mapFrom(entity: ChequeBook): DeletePayablesChequeBooksCommand {
    throw new ApplicationError(DeletePayablesChequeBooksCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): ChequeBook {
    throw new ApplicationError(DeletePayablesChequeBooksCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/bursary/Payables_ChequeBooks/delete";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(DeletePayablesChequeBooksCommandHandler.name)
export class DeletePayablesChequeBooksCommandHandler implements IRequestHandler<DeletePayablesChequeBooksCommand, ChequeBook> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService
  ) {
  }

  async Handle(request: DeletePayablesChequeBooksCommand): Promise<ChequeBook> {
    let httpRequest: HttpRequest<DeletePayablesChequeBooksCommand> = new HttpRequest<DeletePayablesChequeBooksCommand>(request.url, request);
    httpRequest.Query += `id=${request.id}`;

    return await this._httpService.Delete<ServiceResult<ChequeBook>>(httpRequest).toPromise().then(response => {
      this._notificationService.showSuccessMessage()
      return response.objResult
    })
  }
}
