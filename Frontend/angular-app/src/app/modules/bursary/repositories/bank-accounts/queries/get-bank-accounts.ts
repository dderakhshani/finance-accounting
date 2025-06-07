import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {PaginatedList} from "../../../../../core/models/paginated-list";
import {BankAccounts} from "../../../entities/bank-accounts";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {ApplicationError} from "../../../../../core/exceptions/application-error";
import {SearchQuery} from "../../../../../shared/services/search/models/search-query";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {HttpService} from "../../../../../core/services/http/http.service";
import {NotificationService} from "../../../../../shared/services/notification/notification.service";
import {HttpRequest} from "../../../../../core/services/http/http-request";
import {Inject} from "@angular/core";

export class GetBankAccounts extends IRequest<GetBankAccounts, PaginatedList<BankAccounts>>{
  constructor(pageIndex:number = 0, pageSize:number = 10, searchQueries?:SearchQuery[], orderByProperty?:string) {
    super();

    this.pageIndex = pageIndex;
    this.pageSize = pageSize;
    this.conditions = searchQueries;
    this.orderByProperty = orderByProperty ?? '';
  }

  mapFrom(entity: PaginatedList<BankAccounts>): GetBankAccounts {
    throw new ApplicationError(GetBankAccounts.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): PaginatedList<BankAccounts> {
    throw new ApplicationError(GetBankAccounts.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/bursary/bankAccounts/getAll";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}
  @MediatorHandler(GetBankAccountsHandler.name)
  export class GetBankAccountsHandler implements IRequestHandler<GetBankAccounts, PaginatedList<BankAccounts>> {
    constructor(
      @Inject(HttpService) private _httpService: HttpService,
      @Inject(NotificationService) private _notificationService: NotificationService,
    ) {
    }

    async Handle(request: GetBankAccounts): Promise<PaginatedList<BankAccounts>> {
      let httpRequest: HttpRequest<GetBankAccounts> = new HttpRequest<GetBankAccounts>(request.url, request);

      return await this._httpService.Post<GetBankAccounts,  PaginatedList<BankAccounts>>(httpRequest).toPromise().then(response => {
        return response
      })

    }
  }

