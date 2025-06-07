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

export class CreateCustomerReceiptCommand extends IRequest<CreateCustomerReceiptCommand, DocumentHead> {


  public accountReferencesGroupId : number|undefined = undefined;
  public codeVoucherGroupId : number|undefined = undefined;
  public yearId : number|undefined = undefined;
  public documentHeadFullDescriptions : string|undefined = undefined;
  public warehouseId : number|undefined = undefined;
  public warehouseTitle : string|undefined = undefined;
  public parentId : number|undefined = undefined;
  public referenceId : number|undefined = undefined;
  public referenceTitle : string|undefined = undefined;
  public documentNo : string|undefined = undefined;
  public documentDate : string|undefined = undefined;
  public documentDescription : string|undefined = undefined;
  public documentStateBaseId : number|undefined = undefined;
  public isManual : boolean|undefined = undefined;
  public voucherHeadId : number|undefined = undefined;
  public totalWeight : number|undefined = undefined;
  public totalQuantity : number|undefined = undefined;
  public totalItemPrice : number|undefined = undefined;
  public vatTax : number|undefined = undefined;
  public vatDutiesTax : number|undefined = undefined;
  public healthTax : number|undefined = undefined;
  public totalItemsDiscount : number|undefined = undefined;
  public totalProductionCost : number|undefined = undefined;
  public discountPercent : number|undefined = undefined;
  public documentDiscount : number|undefined = undefined;
  public priceMinusDiscount : number|undefined = undefined;
  public priceMinusDiscountPlusTax : number|undefined = undefined;
  public paymentTypeBaseId : number|undefined = undefined;
  public expireDate : Date|undefined = undefined;
  public partNumber : string|undefined = undefined;

  constructor() {
    super();
  }

  mapFrom(entity: DocumentHead): CreateCustomerReceiptCommand {
    throw new ApplicationError(CreateCustomerReceiptCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): DocumentHead {
    throw new ApplicationError(CreateCustomerReceiptCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "bursary/CustomerReceive/add";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(CreateCustomerReceiptCommandHandler.name)
export class CreateCustomerReceiptCommandHandler implements IRequestHandler<CreateCustomerReceiptCommand, DocumentHead> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: CreateCustomerReceiptCommand): Promise<DocumentHead> {
    let httpRequest: HttpRequest<CreateCustomerReceiptCommand> = new HttpRequest<CreateCustomerReceiptCommand>(request.url, request);

    return await this._httpService.Post<CreateCustomerReceiptCommand, ServiceResult<DocumentHead>>(httpRequest).toPromise().then(response => {
      this._notificationService.showSuccessMessage()
      return response.objResult
    })

  }
}
