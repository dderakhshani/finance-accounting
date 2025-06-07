
import {Inject} from "@angular/core";
import { ApplicationError } from "src/app/core/exceptions/application-error";
import { ServiceResult } from "src/app/core/models/service-result";
import { HttpRequest } from "src/app/core/services/http/http-request";
import { HttpService } from "src/app/core/services/http/http.service";

import { MediatorHandler } from "src/app/core/services/mediator/decorator";
import { IRequest, IRequestHandler } from "src/app/core/services/mediator/interfaces";
import { ValidationRule } from "src/app/core/validation/validation-rule";
import { ChequeSheet } from "src/app/modules/bursary/entities/cheque-sheet";
import { NotificationService } from "src/app/shared/services/notification/notification.service";


export class UpdateChequeSheetCommand extends IRequest<UpdateChequeSheetCommand, ChequeSheet> {

  public id: number | undefined = undefined;
  public sheba: string | undefined = undefined;
  public sheetsCount: number | undefined = undefined;
  public chequeNumberIdentification: string | undefined = undefined;
  public setOwnerTime: Date | undefined = undefined;
  public isFinished: boolean | undefined = undefined;
  public bankAccountId: number | undefined = undefined;

  constructor() {
    super();
  }

  mapFrom(entity: ChequeSheet): UpdateChequeSheetCommand {
    this.mapBasics(entity,this);
    return this;
  }

  mapTo(): ChequeSheet {
    throw new ApplicationError(UpdateChequeSheetCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/bursary/ChequeSheet/update";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(UpdateChequeSheetCommandHandler.name)
export class UpdateChequeSheetCommandHandler implements IRequestHandler<UpdateChequeSheetCommand, ChequeSheet> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: UpdateChequeSheetCommand): Promise<ChequeSheet> {
    let httpRequest: HttpRequest<UpdateChequeSheetCommand> = new HttpRequest<UpdateChequeSheetCommand>(request.url, request);

    return await this._httpService.Put<UpdateChequeSheetCommand, ServiceResult<ChequeSheet>>(httpRequest).toPromise().then(response => {
      this._notificationService.showSuccessMessage()
      return response.objResult
    })

  }
}
