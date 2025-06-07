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
import { PaginatedList } from "../../../../core/models/paginated-list";



export class GetDocumnetTagBaseValueQuery extends IRequest<GetDocumnetTagBaseValueQuery, string[]> {

  constructor(public pageIndex: number = 0, public pageSize: number = 0, public conditions?: SearchQuery[], public orderByProperty: string = '' ) {
    super();
  }

  mapFrom(entity: string[]): GetDocumnetTagBaseValueQuery {
    throw new ApplicationError(GetDocumnetTagBaseValueQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): string[] {
    throw new ApplicationError(GetDocumnetTagBaseValueQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/purchase/BaseValue/GetDocumentTagBaseValue";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}




@MediatorHandler(GetDocumnetTagBaseValueQueryHandler.name)
export class GetDocumnetTagBaseValueQueryHandler implements IRequestHandler<GetDocumnetTagBaseValueQuery, string[]> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: GetDocumnetTagBaseValueQuery): Promise<string[]> {
    let httpRequest: HttpRequest<GetDocumnetTagBaseValueQuery> = new HttpRequest<GetDocumnetTagBaseValueQuery>(request.url, request);
    

    return await this._httpService.Get<ServiceResult<string[]>>(httpRequest).toPromise().then(response => {

      return response.objResult
    })

  }
}
