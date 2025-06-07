import { ApplicationError } from "../../../../../core/exceptions/application-error";
import { SearchQuery } from "../../../../../shared/services/search/models/search-query";
import { Inject } from "@angular/core";
import { ServiceResult } from "../../../../../core/models/service-result";
import { IRequest, IRequestHandler } from "../../../../../core/services/mediator/interfaces";
import { HttpService } from "../../../../../core/services/http/http.service";
import { MediatorHandler } from "../../../../../core/services/mediator/decorator";
import { ValidationRule } from "../../../../../core/validation/validation-rule";
import { HttpRequest } from "../../../../../core/services/http/http-request";
import { PaginatedList } from "../../../../../core/models/paginated-list";
import { GetUserByRoleIdModel } from "../../../entities/get-user";


export class GetUserByRoleIdQuery extends IRequest<GetUserByRoleIdQuery, PaginatedList<GetUserByRoleIdModel>> {
    roleId: number = 0;

    constructor(roleId: number) {
        super();
        this.roleId = roleId;
    }


    mapFrom(entity: PaginatedList<GetUserByRoleIdModel>): GetUserByRoleIdQuery {
        throw new ApplicationError(GetUserByRoleIdQuery.name, this.mapTo.name, 'Not Implemented Yet')
    }

    mapTo(): PaginatedList<GetUserByRoleIdModel> {
        throw new ApplicationError(GetUserByRoleIdQuery.name, this.mapTo.name, 'Not Implemented Yet')
    }

    get url(): string {
        return "/ticketing/BasicInfo/GetUsersByRoleId";
    }

    get validationRules(): ValidationRule[] {
        return [];
    }
}

@MediatorHandler(GetUserByRoleIdQueryHandler.name)
export class GetUserByRoleIdQueryHandler implements IRequestHandler<GetUserByRoleIdQuery, PaginatedList<GetUserByRoleIdModel>> {
    constructor(
        @Inject(HttpService) private _httpService: HttpService) {
    }

    async Handle(request: GetUserByRoleIdQuery): Promise<PaginatedList<GetUserByRoleIdModel>> {
        let httpRequest: HttpRequest<GetUserByRoleIdQuery> = new HttpRequest<GetUserByRoleIdQuery>(request.url + "?RoleId=" + request.roleId, request);


        return await this._httpService.Get<ServiceResult<PaginatedList<GetUserByRoleIdModel>>>(httpRequest).toPromise().then(response => {
            return response.objResult
        })

        // return await this._httpService.Get<GetAllRoleQuery, ServiceResult<PaginatedList<GetAllRoleModel>>>(httpRequest).toPromise().then(response => {
        //   return response.objResult
        // })


    }
}
