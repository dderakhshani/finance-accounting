import {Inject} from "@angular/core";
import {ServiceResult} from "../../../../../../core/models/service-result";
import {IRequest, IRequestHandler} from "../../../../../../core/services/mediator/interfaces";
import {MoadianInvoiceHeader} from "../../../../entities/moadian-invoice-header";
import {HttpService} from "../../../../../../core/services/http/http.service";
import {ApplicationError} from "../../../../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../../../../core/services/mediator/decorator";
import {ValidationRule} from "../../../../../../core/validation/validation-rule";
import {HttpRequest} from "../../../../../../core/services/http/http-request";
import {
  CreateMoadianInvoiceDetailCommand
} from "../../moadian-invoice-detail/commands/create-moadian-invoice-detail-command";
import {
  UpdateMoadianInvoiceDetailCommand
} from "../../moadian-invoice-detail/commands/update-moadian-invoice-detail-command";
import {NotificationService} from "../../../../../../shared/services/notification/notification.service";

export class UpdateMoadianInvoicesStatusByIdsCommand extends IRequest<UpdateMoadianInvoicesStatusByIdsCommand, MoadianInvoiceHeader> {

  public ids: number[] = [];
  public status: string| undefined = undefined;

  constructor() {
    super();
  }

  mapFrom(entity: MoadianInvoiceHeader): UpdateMoadianInvoicesStatusByIdsCommand {
    throw new ApplicationError(UpdateMoadianInvoicesStatusByIdsCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): MoadianInvoiceHeader {
    throw new ApplicationError(UpdateMoadianInvoicesStatusByIdsCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/accounting/moadian/updateInvoicesStatusByIds";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(UpdateMoadianInvoicesStatusByIdsCommandHandler.name)
export class UpdateMoadianInvoicesStatusByIdsCommandHandler implements IRequestHandler<UpdateMoadianInvoicesStatusByIdsCommand, MoadianInvoiceHeader> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService
  ) {
  }

  async Handle(request: UpdateMoadianInvoicesStatusByIdsCommand): Promise<MoadianInvoiceHeader> {
    let httpRequest: HttpRequest<UpdateMoadianInvoicesStatusByIdsCommand> = new HttpRequest<UpdateMoadianInvoicesStatusByIdsCommand>(request.url, request);
    // httpRequest.Query  += ;
    // httpRequest.Headers = httpRequest.Headers.append()

    return await this._httpService.Put<UpdateMoadianInvoicesStatusByIdsCommand, ServiceResult<MoadianInvoiceHeader>>(httpRequest).toPromise().then(response => {
      this._notificationService.showSuccessMessage()
      return response.objResult
    })
  }
}
