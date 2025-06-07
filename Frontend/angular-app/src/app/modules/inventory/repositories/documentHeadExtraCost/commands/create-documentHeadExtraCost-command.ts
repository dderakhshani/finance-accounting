import {Inject} from "@angular/core";

import {ServiceResult} from "../../../../../core/models/service-result";
import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {HttpService} from "../../../../../core/services/http/http.service";
import {ApplicationError} from "../../../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {NotificationService} from "../../../../../shared/services/notification/notification.service";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {HttpRequest} from "../../../../../core/services/http/http-request";
import { CommodityCategory } from "../../../../commodity/entities/commodity-category";
import { ReceiptAllStatusModel } from "../../../entities/receipt-all-status";
import { DocumentHeadExtraCost } from "../../../entities/documentHeadExtraCost";

export class ModifyDocumentHeadExtraCostCommand extends IRequest<ModifyDocumentHeadExtraCostCommand, DocumentHeadExtraCost> {

  documentHeadExtraCosts: DocumentHeadExtraCost[] = [];  
  constructor() {
    super();
  }

  mapFrom(entity: DocumentHeadExtraCost): ModifyDocumentHeadExtraCostCommand {
    throw new ApplicationError(ModifyDocumentHeadExtraCostCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): DocumentHeadExtraCost {
    throw new ApplicationError(ModifyDocumentHeadExtraCostCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/Inventory/DocumentHeadExtraCost/Modify";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(ModifyDocumentHeadExtraCostCommandHandler.name)
export class ModifyDocumentHeadExtraCostCommandHandler implements IRequestHandler<ModifyDocumentHeadExtraCostCommand, DocumentHeadExtraCost> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: ModifyDocumentHeadExtraCostCommand): Promise<DocumentHeadExtraCost> {
    this._notificationService.isLoader = true;
    let httpRequest: HttpRequest<ModifyDocumentHeadExtraCostCommand> = new HttpRequest<ModifyDocumentHeadExtraCostCommand>(request.url, request);


    return await this._httpService.Post<ModifyDocumentHeadExtraCostCommand, ServiceResult<DocumentHeadExtraCost>>(httpRequest).toPromise().then(response => {
      this._notificationService.showSuccessMessage()

      return response.objResult
    }).finally(() => {
      this._notificationService.isLoader = false;
    })


  }
}
