import {IRequest, IRequestHandler} from "src/app/core/services/mediator/interfaces";
import {User} from "../../../entities/user";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {GetCompaniesQueryHandler} from "../../company/queries/get-companies-query";
import {HttpRequest} from "../../../../../core/services/http/http-request";
import {Inject} from "@angular/core";
import {HttpService} from "../../../../../core/services/http/http.service";
import {ServiceResult} from "../../../../../core/models/service-result";

export class GetUserQuery extends IRequest<GetUserQuery,User>{

  public id!:number;

  constructor(id:number) {
    super();
    this.id = id;
  }
  mapFrom(entity: User): GetUserQuery {
    return new GetUserQuery(0);
  }

  mapTo(): User {
    return new User();
  }

  get url(): string {
    return "/admin/User/get";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(GetUserQueryHandler.name)
export  class GetUserQueryHandler implements IRequestHandler<GetUserQuery,User> {
  constructor(
    @Inject(HttpService) private _httpService:HttpService
  ) {
  }
  async Handle(request: GetUserQuery): Promise<User> {
    let httpRequest = new HttpRequest(request.url,request)
    httpRequest.Query += `id=${request.id}`;
    return await this._httpService.Get<ServiceResult<User>>(httpRequest).toPromise().then(res => {
      return res.objResult
    })
  }

}
