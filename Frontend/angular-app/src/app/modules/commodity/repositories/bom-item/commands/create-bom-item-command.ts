import {BomItem} from "../../../entities/bom-Item";
import {ApplicationError} from "../../../../../core/exceptions/application-error";
import {Inject} from "@angular/core";
import {ServiceResult} from "../../../../../core/models/service-result";
import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {HttpService} from "../../../../../core/services/http/http.service";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {HttpRequest} from "../../../../../core/services/http/http-request";

export class CreateBomItemCommand extends IRequest<CreateBomItemCommand, BomItem> {
  public subCategoryId: number | undefined = undefined;
  public commodityId: number | undefined = undefined;
  public commodityCode: string | undefined = undefined;

  constructor() {
    super();
  }

  mapFrom(entity: BomItem): CreateBomItemCommand {
    this.mapBasics(entity, this);
    return this;
  }

  mapTo(): BomItem {
    throw new ApplicationError(CreateBomItemCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/commodity/bomItem/add";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(CreateBomItemCommandHandler.name)
export class CreateBomItemCommandHandler implements IRequestHandler<CreateBomItemCommand, BomItem> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
  ) {
  }

  async Handle(request: CreateBomItemCommand): Promise<BomItem> {
    let httpRequest: HttpRequest<CreateBomItemCommand> = new HttpRequest<CreateBomItemCommand>(request.url, request);
    // httpRequest.Query  += ;
    // httpRequest.Headers = httpRequest.Headers.append()

    return await this._httpService.Post<CreateBomItemCommand, ServiceResult<BomItem>>(httpRequest).toPromise().then(response => {
      return response.objResult
    })
  }
}
