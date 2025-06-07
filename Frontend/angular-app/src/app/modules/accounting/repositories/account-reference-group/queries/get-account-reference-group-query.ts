import {Inject} from "@angular/core";
import {ServiceResult} from "../../../../../core/models/service-result";
import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {HttpService} from "../../../../../core/services/http/http.service";
import {ApplicationError} from "../../../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {HttpRequest} from "../../../../../core/services/http/http-request";
import {AccountReferencesGroup} from "../../../entities/account-references-group";

export class GetAccountReferencesGroupQuery extends IRequest<GetAccountReferencesGroupQuery, AccountReferencesGroup> {
  constructor(public entityId: number) {
    super();
  }



  mapFrom(entity: AccountReferencesGroup): GetAccountReferencesGroupQuery {
    throw new ApplicationError(GetAccountReferencesGroupQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): AccountReferencesGroup {
    throw new ApplicationError(GetAccountReferencesGroupQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/accounting/accountReferencesGroup/get";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(GetAccountReferencesGroupQueryHandler.name)
export class GetAccountReferencesGroupQueryHandler implements IRequestHandler<GetAccountReferencesGroupQuery, AccountReferencesGroup> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService
  ) {
  }

  async Handle(request: GetAccountReferencesGroupQuery): Promise<AccountReferencesGroup> {
    let httpRequest: HttpRequest<GetAccountReferencesGroupQuery> = new HttpRequest<GetAccountReferencesGroupQuery>(request.url, request);
    httpRequest.Query  += `id=${request.entityId}`;
    // httpRequest.Headers = httpRequest.Headers.append()

    return await this._httpService.Get<ServiceResult<AccountReferencesGroup>>(httpRequest).toPromise().then(response => {
      return response.objResult
    })
  }
}
