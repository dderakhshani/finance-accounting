import {Inject} from "@angular/core";
import {ServiceResult} from "../../../../../core/models/service-result";
import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {HttpService} from "../../../../../core/services/http/http.service";
import {ApplicationError} from "../../../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {HttpRequest} from "../../../../../core/services/http/http-request";
import {NotificationService} from "../../../../../shared/services/notification/notification.service";
import { BomHeader } from "../../../entities/boms-header";
import { CreateBomHeaderItemCommand } from "../../bom-item-headers/commands/create-bom-hearder-item-command";

export class CreateBomHeadersCommand extends IRequest<CreateBomHeadersCommand, BomHeader> {


  public bomId: number | undefined = undefined;
  public commodityId: number | undefined = undefined;
  public bomDate: Date | undefined = undefined;
  public name: string | undefined = undefined;
  public values: CreateBomHeaderItemCommand[] = []
    

  constructor() {
    super();
  }

  mapFrom(entity: BomHeader): CreateBomHeadersCommand {
    this.mapBasics(entity, this);


    this.values = entity.values.map(x => new CreateBomHeaderItemCommand().mapFrom(x))
    return this;
  }

  mapTo(): BomHeader {
    throw new ApplicationError(CreateBomHeadersCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/commodity/bomValueHeader/add";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(CreateBomHeadersCommandHandler.name)
export class CreateBomHeadersCommandHandler implements IRequestHandler<CreateBomHeadersCommand, BomHeader> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: CreateBomHeadersCommand): Promise<BomHeader> {
    let httpRequest: HttpRequest<CreateBomHeadersCommand> = new HttpRequest<CreateBomHeadersCommand>(request.url, request);
    // httpRequest.Query  += ;
    // httpRequest.Headers = httpRequest.Headers.append()

    return await this._httpService.Post<CreateBomHeadersCommand, ServiceResult<BomHeader>>(httpRequest).toPromise().then(response => {
      this._notificationService.showSuccessMessage()
      return response.objResult
    })

  }
}
