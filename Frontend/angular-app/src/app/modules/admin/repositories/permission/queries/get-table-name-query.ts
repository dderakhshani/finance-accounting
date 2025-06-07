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

export class GetTableNameQuery extends IRequest<GetTableNameQuery, string[]> {

    constructor() {
        super();
    }


    mapFrom(entity: string[]): GetTableNameQuery {
        throw new ApplicationError(GetTableNameQuery.name, this.mapTo.name, 'Not Implemented Yet')
    }

    mapTo(): string[] {
        throw new ApplicationError(GetTableNameQuery.name, this.mapTo.name, 'Not Implemented Yet')
    }

    get url(): string {
        return "/admin/table/getalltablename";
    }

    get validationRules(): ValidationRule[] {
        return [];
    }
}

@MediatorHandler(GetTableNameQueryHandler.name)
export class GetTableNameQueryHandler implements IRequestHandler<GetTableNameQuery, string[]> {
    constructor(
        @Inject(HttpService) private _httpService: HttpService,
        @Inject(NotificationService) private _notificationService: NotificationService,
    ) {
    }

    async Handle(request: GetTableNameQuery): Promise<string[]> {
        let httpRequest: HttpRequest<GetTableNameQuery> = new HttpRequest<GetTableNameQuery>(request.url, request);


        return await this._httpService.Get<ServiceResult<string[]>>(httpRequest).toPromise().then(response => {
            return response.objResult
        })
    }
}
