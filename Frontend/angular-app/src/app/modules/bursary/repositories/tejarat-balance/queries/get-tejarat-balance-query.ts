import { PaginatedList } from "src/app/core/models/paginated-list";
import { IRequest, IRequestHandler } from "src/app/core/services/mediator/interfaces";
import { ResponseTejaratModel } from "../../../entities/response-tejarat-model";
import { SearchQuery } from "src/app/shared/services/search/models/search-query";
import { ApplicationError } from "src/app/core/exceptions/application-error";
import { ValidationRule } from "src/app/core/validation/validation-rule";
import { MediatorHandler } from "src/app/core/services/mediator/decorator";
import { NotificationService } from "src/app/core/services/Notification/Notification.Service";
import { HttpService } from "src/app/core/services/http/http.service";
import { Inject } from "@angular/core";
import { HttpRequest } from "src/app/core/services/http/http-request";
 

export class GetTejaratBalanceQuery extends IRequest<GetTejaratBalanceQuery, any> {


  fromPersianDate:Date | undefined = undefined;
  toPersianDate:Date | undefined = undefined;
  accountNumber:string |undefined = undefined;
 
  constructor(pageIndex:number = 0, pageSize:number = 10, searchQueries?:SearchQuery[], orderByProperty?:string, fromPersianDate?:Date ,toPersianDate?:Date,accountNumber?:string) {
    super();

    this.pageIndex = pageIndex;
    this.pageSize = pageSize;
    this.conditions = searchQueries;
    this.orderByProperty = orderByProperty ?? '';
    this.fromPersianDate = fromPersianDate;
    this.toPersianDate = toPersianDate;
    this.accountNumber = accountNumber;
    
  }




  mapFrom(entity:  any ): GetTejaratBalanceQuery {
    throw new ApplicationError(GetTejaratBalanceQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo():any {
    throw new ApplicationError(GetTejaratBalanceQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/bursary/TejaratBankAccount/GetAll";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(GetTejaratBalanceQueryHandler.name)
export class GetTejaratBalanceQueryHandler implements IRequestHandler<GetTejaratBalanceQuery, any > {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: GetTejaratBalanceQuery): Promise<any> {
    let httpRequest: HttpRequest<GetTejaratBalanceQuery> = new HttpRequest<GetTejaratBalanceQuery>(request.url, request);
    // httpRequest.Query  += ;
    // httpRequest.Headers = httpRequest.Headers.append()

    return await this._httpService.Post<GetTejaratBalanceQuery,  any>(httpRequest).toPromise().then(response => {
      return response.result.objResult;
    })


  }
}
