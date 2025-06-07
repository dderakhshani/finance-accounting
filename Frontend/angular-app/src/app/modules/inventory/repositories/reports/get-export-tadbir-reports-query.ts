import { SearchQuery } from "../../../../shared/services/search/models/search-query";
import {Inject} from "@angular/core";
import { ServiceResult } from "../../../../core/models/service-result";
import { IRequest, IRequestHandler } from "../../../../core/services/mediator/interfaces";
import { HttpService } from "../../../../core/services/http/http.service";
import { ApplicationError } from "../../../../core/exceptions/application-error";
import { MediatorHandler } from "../../../../core/services/mediator/decorator";
import { NotificationService } from "../../../../shared/services/notification/notification.service";
import { ValidationRule } from "../../../../core/validation/validation-rule";
import { HttpRequest } from "../../../../core/services/http/http-request";

import { spGetDocumentItemForTadbir } from "../../entities/spGetDocumentItemForTadbir";


export class GeTadbirExportQuery extends IRequest<GeTadbirExportQuery, spGetDocumentItemForTadbir[]> {


  constructor(
    public fromDate: Date | undefined = undefined,
    public toDate: Date | undefined = undefined,
    public CodeVoucherGroupId: number | undefined = undefined,
    public DocumentNo: number | undefined = undefined,
    public WarehouseId: number | undefined = undefined
    

  ) {
    super();
  }

  mapFrom(entity: spGetDocumentItemForTadbir[]): GeTadbirExportQuery {
    throw new ApplicationError(GeTadbirExportQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): spGetDocumentItemForTadbir[] {
    throw new ApplicationError(GeTadbirExportQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/inventory/Reports/GetDocumentItemForTadbir";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(GeTadbirExportQueryHandler.name)
export class GeTadbirExportQueryHandler implements IRequestHandler<GeTadbirExportQuery, spGetDocumentItemForTadbir[]> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: GeTadbirExportQuery): Promise<spGetDocumentItemForTadbir[]> {
    this._notificationService.isLoader = true;
    let httpRequest: HttpRequest<GeTadbirExportQuery> = new HttpRequest<GeTadbirExportQuery>(request.url, request);
    
    if (request.fromDate != undefined) {
      httpRequest.Query += `&fromDate=${request.fromDate?.toUTCString()}`;
    }
    if (request.toDate != undefined) {
      httpRequest.Query += `&toDate=${request.toDate?.toUTCString()}`;
    }
    if (request.CodeVoucherGroupId != undefined) {
      httpRequest.Query += `&CodeVoucherGroupId=${request.CodeVoucherGroupId}`;
    }
    if (request.DocumentNo != undefined) {
      httpRequest.Query += `&DocumentNo=${request.DocumentNo}`;
    }
    if (request.WarehouseId != undefined) {
      httpRequest.Query += `&WarehouseId=${request.WarehouseId}`;
    }
    return await this._httpService.Get<ServiceResult<spGetDocumentItemForTadbir[]>>(httpRequest).toPromise().then(response => {
     
      return response.objResult
    }).finally(() => {
      this._notificationService.isLoader = false;
    })

  }
}
