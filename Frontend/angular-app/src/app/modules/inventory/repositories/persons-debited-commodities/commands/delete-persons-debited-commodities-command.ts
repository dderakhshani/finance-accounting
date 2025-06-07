import {Inject} from "@angular/core";

import {ServiceResult} from "../../../../../core/models/service-result";
import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {HttpService} from "../../../../../core/services/http/http.service";
import {ApplicationError} from "../../../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {NotificationService} from "../../../../../shared/services/notification/notification.service";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {HttpRequest} from "../../../../../core/services/http/http-request";
import { PersonsDebitedCommodities } from '../../../entities/persons-debited-commodities';

export class DeletePersonsDebitedCommand extends IRequest<DeletePersonsDebitedCommand, PersonsDebitedCommodities> {

  constructor(public entityId:number) {
    super();
  }

  mapFrom(entity: PersonsDebitedCommodities): DeletePersonsDebitedCommand {
    throw new ApplicationError(DeletePersonsDebitedCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): PersonsDebitedCommodities {
    throw new ApplicationError(DeletePersonsDebitedCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/Inventory/PersonsDebited/delete";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(DeletePersonsDebitedCommandHandler.name)
export class DeletePersonsDebitedCommandHandler implements IRequestHandler<DeletePersonsDebitedCommand, PersonsDebitedCommodities> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: DeletePersonsDebitedCommand): Promise<PersonsDebitedCommodities> {
    this._notificationService.isLoader = true;
    let httpRequest: HttpRequest<DeletePersonsDebitedCommand> = new HttpRequest<DeletePersonsDebitedCommand>(request.url, request);
    httpRequest.Query  += `id=${request.entityId}`;



    return await this._httpService.Delete<ServiceResult<PersonsDebitedCommodities>>(httpRequest).toPromise().then(response => {
       this._notificationService.showSuccessMessage()
      return response.objResult
    }).finally(() => {
      this._notificationService.isLoader = false;
    })
  }
}
