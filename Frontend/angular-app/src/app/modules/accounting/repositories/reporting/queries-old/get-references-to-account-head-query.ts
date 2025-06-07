import {IRequest, IRequestHandler} from "src/app/core/services/mediator/interfaces";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {Inject} from "@angular/core";
import {HttpService} from "../../../../../core/services/http/http.service";
import {ServiceResult} from "../../../../../core/models/service-result";
import {HttpRequest} from "../../../../../core/services/http/http-request";

export class GetReferencesToAccountHeadQuery extends IRequest<GetReferencesToAccountHeadQuery, any[]> {


  public referenceId: number | undefined = undefined;
  public yearId: number | undefined = undefined;
  public level: number | undefined = undefined;

  mapFrom(entity: any[]): GetReferencesToAccountHeadQuery {
    return new GetReferencesToAccountHeadQuery;
  }

  mapTo(): any[] {
    return [];
  }

  get url(): string {
    return "/accounting/Reporting/GetReportReference2AccountHead";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(GetReferencesToAccountHeadQueryHandler.name)
export class GetReferencesToAccountHeadQueryHandler implements IRequestHandler<GetReferencesToAccountHeadQuery, any[]> {
  constructor(
    @Inject(HttpService) private httpService: HttpService
  ) {
  }

  async Handle(request: GetReferencesToAccountHeadQuery) {
    let httpRequest = new HttpRequest(request.url, request);
    return await this.httpService.Post<GetReferencesToAccountHeadQuery, ServiceResult<any[]>>(httpRequest).toPromise().then(res => {
      return res.objResult
    })
  }
}
