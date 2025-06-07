import {HttpRequest} from "../../../../../../../core/services/http/http-request";
import { Inject } from "@angular/core";
import { ApplicationError } from "src/app/core/exceptions/application-error";
import { ServiceResult } from "src/app/core/models/service-result";
import { HttpService } from "src/app/core/services/http/http.service";
import { MediatorHandler } from "src/app/core/services/mediator/decorator";
import { IRequest, IRequestHandler } from "src/app/core/services/mediator/interfaces";
import { ValidationRule } from "src/app/core/validation/validation-rule";
import { DocumentHead } from "src/app/modules/bursary/entities/document-head";

import { NotificationService } from "src/app/shared/services/notification/notification.service";

export class DeleteCustomerReceiptCommand extends IRequest<DeleteCustomerReceiptCommand, DocumentHead> {

  public ids:number[] = [];

  constructor() {
    super();
  }

  mapFrom(entity: DocumentHead): DeleteCustomerReceiptCommand {
    throw new ApplicationError(DeleteCustomerReceiptCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): DocumentHead {
    throw new ApplicationError(DeleteCustomerReceiptCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/bursary/CustomerReceipt/Delete";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(DeleteCustomerReceiptCommandHandler.name)
export class DeleteCustomerReceiptCommandHandler implements IRequestHandler<DeleteCustomerReceiptCommand, DocumentHead> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: DeleteCustomerReceiptCommand): Promise<DocumentHead> {
    let httpRequest: HttpRequest<DeleteCustomerReceiptCommand> = new HttpRequest<DeleteCustomerReceiptCommand>(request.url, request);

    return await this._httpService.Delete<ServiceResult<DocumentHead>>(httpRequest).toPromise().then(response => {
      this._notificationService.showSuccessMessage()
      return response.objResult
    })

  }
}
