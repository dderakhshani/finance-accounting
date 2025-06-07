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

import { spTejaratReportImportCommodity } from "../../entities/spTejaratReportImportCommodity";


export class GetjaratReportImportQuery extends IRequest<GetjaratReportImportQuery, spTejaratReportImportCommodity[]> {


  constructor(
   public documentDate: string | undefined = undefined,
    public documentStauseBaseValue: number | undefined = undefined,

  ) {
    super();
  }

  mapFrom(entity: spTejaratReportImportCommodity[]): GetjaratReportImportQuery {
    throw new ApplicationError(GetjaratReportImportQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): spTejaratReportImportCommodity[] {
    throw new ApplicationError(GetjaratReportImportQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/inventory/Reports/TejaratReports";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(GetjaratReportImportQueryHandler.name)
export class GetjaratReportImportQueryHandler implements IRequestHandler<GetjaratReportImportQuery, spTejaratReportImportCommodity[]> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: GetjaratReportImportQuery): Promise<spTejaratReportImportCommodity[]> {
    this._notificationService.isLoader = true;
    let httpRequest: HttpRequest<GetjaratReportImportQuery> = new HttpRequest<GetjaratReportImportQuery>(request.url, request);
    
    httpRequest.Query += `documentDate=${request.documentDate}&documentStauseBaseValue=${request.documentStauseBaseValue}`;
    return await this._httpService.Get<ServiceResult<spTejaratReportImportCommodity[]>>(httpRequest).toPromise().then(response => {
     
      return response.objResult
    }).finally(() => {
      this._notificationService.isLoader = false;
    })

  }
}
