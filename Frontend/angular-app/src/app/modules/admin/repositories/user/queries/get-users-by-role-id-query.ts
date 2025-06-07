import {IRequest, IRequestHandler} from "src/app/core/services/mediator/interfaces";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";

import {User} from "../../../entities/user";
import {Inject} from "@angular/core";
import { HttpService } from "src/app/core/services/http/http.service";
import {PaginatedList} from "../../../../../core/models/paginated-list";
import {ServiceResult} from "../../../../../core/models/service-result";
import {HttpRequest} from "../../../../../core/services/http/http-request";

export class GetUsersByRoleIdQuery extends IRequest<GetUsersByRoleIdQuery,PaginatedList<User>> {

  public roleId!:number;

  constructor(roleId:number) {
    super();
    this.roleId = roleId;
  }

  mapFrom(entity: PaginatedList<User>): GetUsersByRoleIdQuery {
    return new GetUsersByRoleIdQuery(this.roleId);
  }

  mapTo(): any {
  }

  get url(): string {
    return "/admin/User/GetAllByRoleId";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}


@MediatorHandler(GetUsersByRoleIdQueryHandler.name)
export class GetUsersByRoleIdQueryHandler implements IRequestHandler<GetUsersByRoleIdQuery, PaginatedList<User>> {
  constructor(
    @Inject(HttpService) private httpService: HttpService
  ) {
  }
  async Handle(request: GetUsersByRoleIdQuery): Promise<PaginatedList<User>> {
    let httpRequest = new HttpRequest(request.url,request)
    httpRequest.Query  += `roleId=${request.roleId}`;
    return await this.httpService.Get<ServiceResult<PaginatedList<User>>>(httpRequest).toPromise().then(res => {
      return res.objResult
    })
  }

}




