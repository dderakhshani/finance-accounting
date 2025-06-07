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
import { GetAllRoleModel } from "../../../entities/get-all-role";


export class GetAllRoleQuery extends IRequest<GetAllRoleQuery, PaginatedList<GetAllRoleModel>> {


    constructor() {
        super();
    }


    mapFrom(entity: PaginatedList<GetAllRoleModel>): GetAllRoleQuery {
        throw new ApplicationError(GetAllRoleQuery.name, this.mapTo.name, 'Not Implemented Yet')
    }

    mapTo(): PaginatedList<GetAllRoleModel> {
        throw new ApplicationError(GetAllRoleQuery.name, this.mapTo.name, 'Not Implemented Yet')
    }

    get url(): string {
        return "/ticketing/BasicInfo/GetAllRole";
    }

    get validationRules(): ValidationRule[] {
        return [];
    }
}

@MediatorHandler(GetAllRoleQueryHandler.name)
export class GetAllRoleQueryHandler implements IRequestHandler<GetAllRoleQuery, PaginatedList<GetAllRoleModel>> {
    constructor(
        @Inject(HttpService) private _httpService: HttpService) {
    }

    async Handle(request: GetAllRoleQuery): Promise<PaginatedList<GetAllRoleModel>> {
        let httpRequest: HttpRequest<GetAllRoleQuery> = new HttpRequest<GetAllRoleQuery>(request.url, request);


        return await this._httpService.Get<ServiceResult<PaginatedList<GetAllRoleModel>>>(httpRequest).toPromise().then(response => {
            return response.objResult
        })

        // return await this._httpService.Get<GetAllRoleQuery, ServiceResult<PaginatedList<GetAllRoleModel>>>(httpRequest).toPromise().then(response => {
        //   return response.objResult
        // })


    }
}
