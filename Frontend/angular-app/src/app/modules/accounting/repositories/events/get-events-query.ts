import {IRequest, IRequestHandler} from "../../../../core/services/mediator/interfaces";
import {ValidationRule} from "../../../../core/validation/validation-rule";
import {Inject} from "@angular/core";
import {HttpService} from "../../../../core/services/http/http.service";
import {HttpRequest} from "../../../../core/services/http/http-request";
import {ServiceResult} from "../../../../core/models/service-result";
import {MediatorHandler} from "../../../../core/services/mediator/decorator";
import {ApplicationError} from "../../../../core/exceptions/application-error";
import {EntityEvent} from "./entity-event";

export class GetEventsQuery extends IRequest<GetEventsQuery, EntityEvent[]>{
  constructor(public entityId: number, public entityType: string) {
    super();
  }
  mapFrom(entity: any): GetEventsQuery {
    throw new ApplicationError(GetEventsQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): any {
    throw new ApplicationError(GetEventsQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/accounting/events/getall";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}
@MediatorHandler(GetEventsQueryHandler.name)
export class GetEventsQueryHandler implements IRequestHandler<GetEventsQuery, EntityEvent[]> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
  ) { }

  async Handle(request: GetEventsQuery): Promise<EntityEvent[]> {
    let httpRequest = new HttpRequest<GetEventsQuery>(request.url, request);
    httpRequest.Query += `entityId=${request.entityId}&entityType=${request.entityType}`;

    return await this._httpService.Get<EntityEvent[]>(httpRequest).toPromise().then(res => {
      return res
    });

  }
}

