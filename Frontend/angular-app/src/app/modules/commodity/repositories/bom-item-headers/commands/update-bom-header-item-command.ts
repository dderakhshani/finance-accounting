
import {ApplicationError} from "../../../../../core/exceptions/application-error";
import {Inject} from "@angular/core";
import {ServiceResult} from "../../../../../core/models/service-result";
import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {HttpService} from "../../../../../core/services/http/http.service";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {HttpRequest} from "../../../../../core/services/http/http-request";
import { BomHeaderValue } from "../../../entities/bom-header-value";

export class UpdateBomHeaderItemCommand extends IRequest<UpdateBomHeaderItemCommand, BomHeaderValue> {
  public id: number | undefined = undefined;;
  public bomValueHeaderId: number | undefined = undefined;
  public bomWarehouseId: number | undefined = undefined;
  public usedCommodityId: number | undefined = undefined;
  public commodityCode: string | undefined = undefined;
  public value: number | undefined = undefined;
  public mainMeasureTitle: string | undefined = undefined;
  constructor() {
    super();
  }

  mapFrom(entity: BomHeaderValue): UpdateBomHeaderItemCommand {
    this.mapBasics(entity,this);
    return this;
  }

  mapTo(): BomHeaderValue {
    throw new ApplicationError(UpdateBomHeaderItemCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/commodity/BomValueHeader/update";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(UpdateBomHeaderItemCommandHandler.name)
export class UpdateBomHeaderItemCommandHandler implements IRequestHandler<UpdateBomHeaderItemCommand, BomHeaderValue> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
  ) {
  }

  async Handle(request: UpdateBomHeaderItemCommand): Promise<BomHeaderValue> {
    let httpRequest: HttpRequest<UpdateBomHeaderItemCommand> = new HttpRequest<UpdateBomHeaderItemCommand>(request.url, request);
    // httpRequest.Query  += ;
    // httpRequest.Headers = httpRequest.Headers.append()

    return await this._httpService.Put<UpdateBomHeaderItemCommand, ServiceResult<BomHeaderValue>>(httpRequest).toPromise().then(response => {
      return response.objResult
    })
  }
}
