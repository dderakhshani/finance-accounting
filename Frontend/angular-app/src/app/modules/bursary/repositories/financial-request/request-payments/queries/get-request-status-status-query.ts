 
import { Inject } from "@angular/core";
import { ApplicationError } from "src/app/core/exceptions/application-error";
import { HttpRequest } from "src/app/core/services/http/http-request";
import { HttpService } from "src/app/core/services/http/http.service";
import { MediatorHandler } from "src/app/core/services/mediator/decorator";
import { IRequest, IRequestHandler } from "src/app/core/services/mediator/interfaces";
import { NotificationService } from "src/app/core/services/Notification/Notification.Service";
import { ValidationRule } from "src/app/core/validation/validation-rule";

export class GetReqeustCountByStatusQuery extends IRequest<GetReqeustCountByStatusQuery,number> {


  public status!:number;

  constructor(status:number ) {
    super();
    this.status = status;
  }




  mapFrom(entity: number): GetReqeustCountByStatusQuery {
    throw new ApplicationError(GetReqeustCountByStatusQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): number {
    throw new ApplicationError(GetReqeustCountByStatusQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/bursary/FinancialRequest/GetReqeustCountByStatus";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(GetReqeustCountByStatusQueryHandler.name)
export class GetReqeustCountByStatusQueryHandler implements IRequestHandler<GetReqeustCountByStatusQuery, number> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: GetReqeustCountByStatusQuery): Promise<number> {
    let httpRequest: HttpRequest<GetReqeustCountByStatusQuery> = new HttpRequest<GetReqeustCountByStatusQuery>(request.url, request);
    httpRequest.Query += `status=${request.status}`;
    // httpRequest.Headers = httpRequest.Headers.append()

    return await this._httpService.Get<number>(httpRequest).toPromise().then(response => {
      return response;
    })


  }
}