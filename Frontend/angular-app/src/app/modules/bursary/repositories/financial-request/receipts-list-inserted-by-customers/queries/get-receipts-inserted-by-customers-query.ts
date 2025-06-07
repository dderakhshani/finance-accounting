import { Inject } from "@angular/core";
import { ApplicationError } from "src/app/core/exceptions/application-error";
import { PaginatedList } from "src/app/core/models/paginated-list";
import { ServiceResult } from "src/app/core/models/service-result";
import { HttpRequest } from "src/app/core/services/http/http-request";
import { HttpService } from "src/app/core/services/http/http.service";
import { MediatorHandler } from "src/app/core/services/mediator/decorator";
import { IRequest, IRequestHandler } from "src/app/core/services/mediator/interfaces";
import { ValidationRule } from "src/app/core/validation/validation-rule";

import { DocumentHead } from "src/app/modules/bursary/entities/document-head";
import { ReceiptsInsertedByCustomers } from "src/app/modules/bursary/entities/receipts-inserted-by-customers";
import { NotificationService } from "src/app/shared/services/notification/notification.service";
import { SearchQuery } from "src/app/shared/services/search/models/search-query";
import { environment } from "src/environments/environment";
import * as moment from "jalali-moment";

export class GetReceiptsInsertedByCustomersQuery extends IRequest<GetReceiptsInsertedByCustomersQuery, ReceiptsInsertedByCustomers[]> {


  constructor(public pageIndex: number = 0, public pageSize: number = 10, public conditions?: SearchQuery[], public orderByProperty: string = '') {
    super();
  }


  mapFrom(entity: ReceiptsInsertedByCustomers[]): GetReceiptsInsertedByCustomersQuery {
    throw new ApplicationError(GetReceiptsInsertedByCustomersQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): ReceiptsInsertedByCustomers[] {
    throw new ApplicationError(GetReceiptsInsertedByCustomersQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return environment.crmServerAddress + "/api/Deposits/all";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(GetReceiptsInsertedByCustomersQueryHandler.name)
export class GetReceiptsInsertedByCustomersQueryHandler implements IRequestHandler<GetReceiptsInsertedByCustomersQuery, ReceiptsInsertedByCustomers[]> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: GetReceiptsInsertedByCustomersQuery): Promise<ReceiptsInsertedByCustomers[]> {
    let httpRequest: HttpRequest<GetReceiptsInsertedByCustomersQuery> = new HttpRequest<GetReceiptsInsertedByCustomersQuery>(request.url, request, false);
    httpRequest.Headers = httpRequest.Headers.append("Authorization", "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiI5Iiwic2lkIjoiOSIsInN1YiI6ImFkbWluIiwiZW1haWwiOiJzdHJpbmciLCJqdGkiOiI2NjcxOGNlOS0yODM5LTRiOTYtODdkNC1jZTk3M2Q4NjhjNTQiLCJleHAiOjE3MDYxNDUwODMsImlzcyI6IkVlZmFDZXJhbS5jb20iLCJhdWQiOiJFRUZBLUNSTSJ9.wQ8mnBw4KyDGM4BBE3X7XRbhN5ppLDC4EigXARdJzNE");

    return await this._httpService.Get<ReceiptsInsertedByCustomers[]>(httpRequest).toPromise().then(response => {

      return response.map(x => {

        // @ts-ignore
        x.paymentDate = x.paymentDate + "Z"

        return x;
      });
    })

  }
}
