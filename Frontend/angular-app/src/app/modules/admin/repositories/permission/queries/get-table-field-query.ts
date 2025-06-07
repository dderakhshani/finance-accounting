import { SearchQuery } from "../../../../../shared/services/search/models/search-query";
import { Inject } from "@angular/core";
import { ServiceResult } from "../../../../../core/models/service-result";
import { IRequest, IRequestHandler } from "../../../../../core/services/mediator/interfaces";
import { HttpService } from "../../../../../core/services/http/http.service";
import { Permission } from "../../../entities/permission";
import { ApplicationError } from "../../../../../core/exceptions/application-error";
import { MediatorHandler } from "../../../../../core/services/mediator/decorator";
import { NotificationService } from "../../../../../shared/services/notification/notification.service";
import { ValidationRule } from "../../../../../core/validation/validation-rule";
import { HttpRequest } from "../../../../../core/services/http/http-request";
import { PaginatedList } from "../../../../../core/models/paginated-list";

export class GetTableFieldQuery extends IRequest<GetTableFieldQuery, any[]> {

    constructor(public tablename: string) {
        super();
    }


    mapFrom(entity: any[]): GetTableFieldQuery {
        throw new ApplicationError(GetTableFieldQuery.name, this.mapTo.name, 'Not Implemented Yet')
    }

    mapTo(): any[] {
        throw new ApplicationError(GetTableFieldQuery.name, this.mapTo.name, 'Not Implemented Yet')
    }

    get url(): string {
        return "/admin/table/GetAllTableFieldsByName?tablename=";
    }

    get validationRules(): ValidationRule[] {
        return [];
    }
}

@MediatorHandler(GetTableFieldQueryHandler.name)
export class GetTableFieldQueryHandler implements IRequestHandler<GetTableFieldQuery, any[]> {
    constructor(
        @Inject(HttpService) private _httpService: HttpService,
        @Inject(NotificationService) private _notificationService: NotificationService,
    ) {
    }

    async Handle(request: GetTableFieldQuery): Promise<any[]> {
        let httpRequest: HttpRequest<GetTableFieldQuery> = new HttpRequest<GetTableFieldQuery>(request.url + request.tablename, request);


        return await this._httpService.Get<ServiceResult<any[]>>(httpRequest).toPromise().then(response => {
            return response.objResult
        })
    }
}
