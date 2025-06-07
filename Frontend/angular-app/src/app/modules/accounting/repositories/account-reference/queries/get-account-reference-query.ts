import {Inject} from "@angular/core";
import {ServiceResult} from "../../../../../core/models/service-result";
import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {HttpService} from "../../../../../core/services/http/http.service";
import {ApplicationError} from "../../../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {HttpRequest} from "../../../../../core/services/http/http-request";
import {AccountReference} from "../../../entities/account-reference";

export class GetAccountReferenceQuery extends IRequest<GetAccountReferenceQuery, AccountReference> {
  constructor(public entityId: number) {
    super();
  }



  mapFrom(entity: AccountReference): GetAccountReferenceQuery {
    throw new ApplicationError(GetAccountReferenceQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): AccountReference {
    throw new ApplicationError(GetAccountReferenceQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/accounting/accountReference/get";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(GetAccountReferenceQueryHandler.name)
export class GetAccountReferenceQueryHandler implements IRequestHandler<GetAccountReferenceQuery, AccountReference> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService
  ) {
  }

  async Handle(request: GetAccountReferenceQuery): Promise<AccountReference> {
    let httpRequest: HttpRequest<GetAccountReferenceQuery> = new HttpRequest<GetAccountReferenceQuery>(request.url, request);
    httpRequest.Query  += `id=${request.entityId}`;
    // httpRequest.Headers = httpRequest.Headers.append()

    return await this._httpService.Get<ServiceResult<AccountReference>>(httpRequest).toPromise().then(response => {
      return response.objResult
    })
  }
}
