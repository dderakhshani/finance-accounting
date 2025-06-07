import { SearchQuery } from "../../../../../shared/services/search/models/search-query";
import {Inject} from "@angular/core";
import { Receipt } from "../../../entities/receipt";
import { ServiceResult } from "../../../../../core/models/service-result";
import { IRequest, IRequestHandler } from "../../../../../core/services/mediator/interfaces";
import { HttpService } from "../../../../../core/services/http/http.service";
import { ApplicationError } from "../../../../../core/exceptions/application-error";
import { MediatorHandler } from "../../../../../core/services/mediator/decorator";
import { NotificationService } from "../../../../../shared/services/notification/notification.service";
import { ValidationRule } from "../../../../../core/validation/validation-rule";
import { HttpRequest } from "../../../../../core/services/http/http-request";
import { PaginatedList } from "../../../../../core/models/paginated-list";

import { PagesCommonService } from "../../../../../shared/services/pages/pages-common.service";
import { AutoVoucherResults, warehouseReceiptToAutoVoucher } from "../../../entities/autoVoucher";


export class CreateAndUpdateAutoVoucher2Command extends IRequest<CreateAndUpdateAutoVoucher2Command, AutoVoucherResults> {
  public ActionType: string | undefined = undefined;
  public VocherHeadId: string | undefined = undefined;
  dataList: any[] = []
  constructor() {
    super();
  }

  mapFrom(entity: any): CreateAndUpdateAutoVoucher2Command {
    throw new ApplicationError(CreateAndUpdateAutoVoucher2Command.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): any {
    throw new ApplicationError(CreateAndUpdateAutoVoucher2Command.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/accounting/VouchersHead/AutoVoucher";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}




@MediatorHandler(CreateAndUpdateAutoVoucher2CommandHandler.name)
export class CreateAndUpdateAutoVoucher2CommandHandler implements IRequestHandler<CreateAndUpdateAutoVoucher2Command, AutoVoucherResults> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
    @Inject(PagesCommonService) private inventoryService: PagesCommonService

  ) {
  }

  async Handle(request: CreateAndUpdateAutoVoucher2Command): Promise<AutoVoucherResults> {
    this._notificationService.isLoader = true;
    let httpRequest: HttpRequest<CreateAndUpdateAutoVoucher2Command> = new HttpRequest<CreateAndUpdateAutoVoucher2Command>(request.url, request);


    return await this._httpService.Post<CreateAndUpdateAutoVoucher2Command, ServiceResult<AutoVoucherResults>>(httpRequest).toPromise().then(response => {
     
      return response.objResult
    }).finally(() => {
      this._notificationService.isLoader = false;
    })

  }

}
