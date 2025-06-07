import {BomItem} from "../../../entities/bom-Item";
import {ApplicationError} from "../../../../../core/exceptions/application-error";
import {Inject} from "@angular/core";
import {ServiceResult} from "../../../../../core/models/service-result";
import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {HttpService} from "../../../../../core/services/http/http.service";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {HttpRequest} from "../../../../../core/services/http/http-request";

export class UpdateBomItemCommand extends IRequest<UpdateBomItemCommand, BomItem> {
  public id: number | undefined = undefined;
  public bomId: number | undefined = undefined;
  public subCategoryId: number | undefined = undefined;
  public commodityId: number | undefined = undefined;
  public commodityCode: string | undefined = undefined;

  constructor() {
    super();
  }

  mapFrom(entity: BomItem): UpdateBomItemCommand {
    this.mapBasics(entity,this);
    return this;
  }

  mapTo(): BomItem {
    throw new ApplicationError(UpdateBomItemCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/commodity/bomItem/update";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(UpdateBomItemCommandHandler.name)
export class UpdateBomItemCommandHandler implements IRequestHandler<UpdateBomItemCommand, BomItem> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
  ) {
  }

  async Handle(request: UpdateBomItemCommand): Promise<BomItem> {
    let httpRequest: HttpRequest<UpdateBomItemCommand> = new HttpRequest<UpdateBomItemCommand>(request.url, request);
    // httpRequest.Query  += ;
    // httpRequest.Headers = httpRequest.Headers.append()

    return await this._httpService.Put<UpdateBomItemCommand, ServiceResult<BomItem>>(httpRequest).toPromise().then(response => {
      return response.objResult
    })
  }
}
