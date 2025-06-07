import {Inject} from "@angular/core";
import {ServiceResult} from "../../../../../core/models/service-result";
import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {HttpService} from "../../../../../core/services/http/http.service";
import {ApplicationError} from "../../../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {NotificationService} from "../../../../../shared/services/notification/notification.service";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {HttpRequest} from "../../../../../core/services/http/http-request";
import { DocumentHeadExtraCost } from "../../../entities/documentHeadExtraCost";

export class DeleteDocumentHeadExtraCostCommand extends IRequest<DeleteDocumentHeadExtraCostCommand, DocumentHeadExtraCost> {

  constructor(public entityId:number) {
    super();
  }

  mapFrom(entity: DocumentHeadExtraCost): DeleteDocumentHeadExtraCostCommand {
    throw new ApplicationError(DeleteDocumentHeadExtraCostCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): DocumentHeadExtraCost {
    throw new ApplicationError(DeleteDocumentHeadExtraCostCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/Inventory/DocumentHeadExtraCost/delete";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(DeleteDocumentHeadExtraCostCommandHandler.name)
export class DeleteDocumentHeadExtraCostCommandHandler implements IRequestHandler<DeleteDocumentHeadExtraCostCommand, DocumentHeadExtraCost> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: DeleteDocumentHeadExtraCostCommand): Promise<DocumentHeadExtraCost> {
    this._notificationService.isLoader = true;
    let httpRequest: HttpRequest<DeleteDocumentHeadExtraCostCommand> = new HttpRequest<DeleteDocumentHeadExtraCostCommand>(request.url, request);
    httpRequest.Query  += `id=${request.entityId}`;



    return await this._httpService.Delete<ServiceResult<DocumentHeadExtraCost>>(httpRequest).toPromise().then(response => {
       this._notificationService.showSuccessMessage()
      return response.objResult
    }).finally(() => {
      this._notificationService.isLoader = false;
    })
  }
}
