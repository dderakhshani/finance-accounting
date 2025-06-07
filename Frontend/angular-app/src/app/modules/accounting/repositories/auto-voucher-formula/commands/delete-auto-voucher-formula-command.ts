import { Inject } from "@angular/core";
import { ServiceResult } from "../../../../../core/models/service-result";
import { IRequest, IRequestHandler } from "../../../../../core/services/mediator/interfaces";
import { HttpService } from "../../../../../core/services/http/http.service";
import { ApplicationError } from "../../../../../core/exceptions/application-error";
import { MediatorHandler } from "../../../../../core/services/mediator/decorator";
import { NotificationService } from "../../../../../shared/services/notification/notification.service";
import { ValidationRule } from "../../../../../core/validation/validation-rule";
import { HttpRequest } from "../../../../../core/services/http/http-request";
import { CodeVoucherGroup } from "../../../entities/code-voucher-group";
import { AutoVoucherFormula } from "../../../entities/AutoVoucherFormula";

export class DeleteAutoVoucherFormulaCommand extends IRequest<DeleteAutoVoucherFormulaCommand, AutoVoucherFormula> {

  constructor(public id: any) {
    super();
  }

  mapFrom(entity: AutoVoucherFormula): DeleteAutoVoucherFormulaCommand {
    throw new ApplicationError(DeleteAutoVoucherFormulaCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): AutoVoucherFormula {
    throw new ApplicationError(DeleteAutoVoucherFormulaCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/accounting/autoVoucherFormula/delete";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(DeleteAutoVoucherFormulaCommandHandler.name)
export class DeleteAutoVoucherFormulaCommandHandler implements IRequestHandler<DeleteAutoVoucherFormulaCommand, AutoVoucherFormula> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: DeleteAutoVoucherFormulaCommand): Promise<AutoVoucherFormula> {
    let httpRequest: HttpRequest<DeleteAutoVoucherFormulaCommand> = new HttpRequest<DeleteAutoVoucherFormulaCommand>(request.url, request);
    httpRequest.Query += `id=${request.id}`;

    return await this._httpService.Delete<ServiceResult<AutoVoucherFormula>>(httpRequest).toPromise().then(response => {
      this._notificationService.showSuccessMessage()
      return response.objResult
    })
  }
}
