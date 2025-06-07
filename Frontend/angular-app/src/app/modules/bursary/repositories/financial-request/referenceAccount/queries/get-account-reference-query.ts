import {Inject} from "@angular/core";
import { ApplicationError } from "src/app/core/exceptions/application-error";
import { ServiceResult } from "src/app/core/models/service-result";
import { HttpRequest } from "src/app/core/services/http/http-request";
import { HttpService } from "src/app/core/services/http/http.service";
import { MediatorHandler } from "src/app/core/services/mediator/decorator";
import { IRequest, IRequestHandler } from "src/app/core/services/mediator/interfaces";
import { ValidationRule } from "src/app/core/validation/validation-rule";
import { AccountReference } from "src/app/modules/bursary/entities/account-reference";
import { NotificationService } from "src/app/shared/services/notification/notification.service";


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
    return "/bursary/AccountReference/get";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(GetAccountReferenceQueryHandler.name)
export class GetAccountReferenceQueryHandler implements IRequestHandler<GetAccountReferenceQuery, AccountReference> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: GetAccountReferenceQuery): Promise<AccountReference> {
    let httpRequest: HttpRequest<GetAccountReferenceQuery> = new HttpRequest<GetAccountReferenceQuery>(request.url, request);
    httpRequest.Query  += `id=${request.entityId}`;

    return await this._httpService.Get<ServiceResult<AccountReference>>(httpRequest).toPromise().then(response => {
      return response.objResult
    })
  }
}
