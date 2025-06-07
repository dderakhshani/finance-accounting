import {IRequest, IRequestHandler} from "../../../../core/services/mediator/interfaces";
import {Archive} from "../../entities/archive";
import {MediatorHandler} from "../../../../core/services/mediator/decorator";
import {HttpRequest} from "../../../../core/services/http/http-request";
import {Inject} from "@angular/core";
import {HttpService} from "../../../../core/services/http/http.service";
import {NotificationService} from "../../../../shared/services/notification/notification.service";


export class GetArchivesQuery extends IRequest<GetArchivesQuery, Archive[]> {
  constructor(public baseValueTypeId:number) {
    super();
  }

  mapFrom(entity: Archive[]): GetArchivesQuery {
    throw new Error("Method not implemented.");
  }

  mapTo(): Archive[] {
    throw new Error("Method not implemented.");
  }

  get url(): string {
    return "/fileTransfer/Archive/GetAll";
  }

  get validationRules(): any[] {
    return [];
  }
}

@MediatorHandler(GetArchivesQueryHandler.name)
export class GetArchivesQueryHandler implements IRequestHandler<GetArchivesQuery, Archive[]> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: GetArchivesQuery): Promise<Archive[]> {
    let httpRequest: HttpRequest<GetArchivesQuery> = new HttpRequest<GetArchivesQuery>(request.url, request);
    httpRequest.Query += `typeId=${request.baseValueTypeId}`
    return await this._httpService.Get<Archive[]>(httpRequest).toPromise().then(response => {
      return response
    })
  }
}
