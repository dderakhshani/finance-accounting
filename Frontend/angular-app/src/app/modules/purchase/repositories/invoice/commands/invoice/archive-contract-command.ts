import {Inject} from "@angular/core";
import { ServiceResult } from "../../../../../../core/models/service-result";
import { IRequest, IRequestHandler } from "../../../../../../core/services/mediator/interfaces";
import { HttpService } from "../../../../../../core/services/http/http.service";
import { ApplicationError } from "../../../../../../core/exceptions/application-error";
import { MediatorHandler } from "../../../../../../core/services/mediator/decorator";
import { NotificationService } from "../../../../../../shared/services/notification/notification.service";
import { ValidationRule } from "../../../../../../core/validation/validation-rule";
import { HttpRequest } from "../../../../../../core/services/http/http-request";
import { invoice } from "../../../../entities/invoice";


export class ArchiveContractCommand extends IRequest<ArchiveContractCommand, invoice> {

  constructor(public entityId:number) {
    super();
  }

  mapFrom(entity: invoice): ArchiveContractCommand {
    throw new ApplicationError(ArchiveContractCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): invoice {
    throw new ApplicationError(ArchiveContractCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/purchase/Invoice/ArchiveContract";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(ArchiveContractCommandHandler.name)
export class ArchiveContractCommandHandler implements IRequestHandler<ArchiveContractCommand, invoice> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: ArchiveContractCommand): Promise<invoice> {
    this._notificationService.isLoader = true;
    let httpRequest: HttpRequest<ArchiveContractCommand> = new HttpRequest<ArchiveContractCommand>(request.url, request);
    httpRequest.Query  += `id=${request.entityId}`;



    return await this._httpService.Delete<ServiceResult<invoice>>(httpRequest).toPromise().then(response => {
       this._notificationService.showSuccessMessage()
      return response.objResult
    }).finally(() => {
      this._notificationService.isLoader = false;
    })
  }
}
