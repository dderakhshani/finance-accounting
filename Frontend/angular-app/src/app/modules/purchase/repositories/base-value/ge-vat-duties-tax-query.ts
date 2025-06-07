import { SearchQuery } from "../../../../shared/services/search/models/search-query";
import {Inject} from "@angular/core";
import { invoice } from "../../entities/invoice";
import { ServiceResult } from "../../../../core/models/service-result";
import { IRequest, IRequestHandler } from "../../../../core/services/mediator/interfaces";
import { HttpService } from "../../../../core/services/http/http.service";
import { ApplicationError } from "../../../../core/exceptions/application-error";
import { MediatorHandler } from "../../../../core/services/mediator/decorator";
import { NotificationService } from "../../../../shared/services/notification/notification.service";
import { ValidationRule } from "../../../../core/validation/validation-rule";
import { HttpRequest } from "../../../../core/services/http/http-request";
import { PaginatedList } from "../../../../core/models/paginated-list";



export class GeVatDutiesTaxValueQuery extends IRequest<GeVatDutiesTaxValueQuery, number> {

  constructor(public pageIndex: number = 0, public pageSize: number = 0, public conditions?: SearchQuery[], public orderByProperty: string = '' ) {
    super();
  }

  mapFrom(entity: number): GeVatDutiesTaxValueQuery {
    throw new ApplicationError(GeVatDutiesTaxValueQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): number {
    throw new ApplicationError(GeVatDutiesTaxValueQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/purchase/BaseValue/GeVatDutiesTaxValue";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}




@MediatorHandler(GeVatDutiesTaxValueQueryHandler.name)
export class GeVatDutiesTaxValueQueryHandler implements IRequestHandler<GeVatDutiesTaxValueQuery, number> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: GeVatDutiesTaxValueQuery): Promise<number> {
    let httpRequest: HttpRequest<GeVatDutiesTaxValueQuery> = new HttpRequest<GeVatDutiesTaxValueQuery>(request.url, request);
    

    return await this._httpService.Post<GeVatDutiesTaxValueQuery, ServiceResult<number>>(httpRequest).toPromise().then(response => {
      return response.objResult
    })

  }
}
