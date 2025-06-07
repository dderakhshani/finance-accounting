
import {ApplicationError} from "../../../../../core/exceptions/application-error";
import {Inject} from "@angular/core";
import {ServiceResult} from "../../../../../core/models/service-result";
import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {HttpService} from "../../../../../core/services/http/http.service";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {HttpRequest} from "../../../../../core/services/http/http-request";
import { BomHeaderValue } from "../../../entities/bom-header-value";

export class CreateBomHeaderItemCommand extends IRequest<CreateBomHeaderItemCommand, BomHeaderValue> {
 
  public bomValueHeaderId: number | undefined = undefined;
  public usedCommodityId: number | undefined = undefined;
  public bomWarehouseId: number | undefined = undefined;
  public commodityCode: string | undefined = undefined;
  public value: number | undefined = undefined;
  public mainMeasureTitle: string | undefined = undefined;

  constructor() {
    super();
  }

  mapFrom(entity: BomHeaderValue): CreateBomHeaderItemCommand {
    this.mapBasics(entity, this);
    return this;
  }

  mapTo(): BomHeaderValue {
    throw new ApplicationError(CreateBomHeaderItemCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/commodity/BomValueHeader/add";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(CreateBomHeaderItemCommandHandler.name)
export class CreateBomHeaderItemCommandHandler implements IRequestHandler<CreateBomHeaderItemCommand, BomHeaderValue> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
  ) {
  }

  async Handle(request: CreateBomHeaderItemCommand): Promise<BomHeaderValue> {
    let httpRequest: HttpRequest<CreateBomHeaderItemCommand> = new HttpRequest<CreateBomHeaderItemCommand>(request.url, request);
    // httpRequest.Query  += ;
    // httpRequest.Headers = httpRequest.Headers.append()

    return await this._httpService.Post<CreateBomHeaderItemCommand, ServiceResult<BomHeaderValue>>(httpRequest).toPromise().then(response => {
      return response.objResult
    })
  }
}
