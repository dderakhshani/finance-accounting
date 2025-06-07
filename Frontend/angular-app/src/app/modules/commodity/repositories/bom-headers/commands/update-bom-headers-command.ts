import {Inject} from "@angular/core";
import Bom from "../../../entities/bom";
import {ServiceResult} from "../../../../../core/models/service-result";
import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {HttpService} from "../../../../../core/services/http/http.service";
import {ApplicationError} from "../../../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {HttpRequest} from "../../../../../core/services/http/http-request";
import {NotificationService} from "../../../../../shared/services/notification/notification.service";
import { BomHeader } from "../../../entities/boms-header";
import { UpdateBomHeaderItemCommand } from "../../bom-item-headers/commands/update-bom-header-item-command";

export class UpdateBomHeadersCommand extends IRequest<UpdateBomHeadersCommand, BomHeader> {

  public id: number | undefined = undefined;
  public bomId: number | undefined = undefined;
  public commodityId: number | undefined = undefined;
  public name: string | undefined = undefined;
  public bomDate: Date | undefined = undefined;
  public values: UpdateBomHeaderItemCommand[] = []
  constructor() {
    super();
  }

  mapFrom(entity: BomHeader): UpdateBomHeadersCommand {
    this.mapBasics(entity, this);
    
    
    this.values = entity.values.map(x => new UpdateBomHeaderItemCommand().mapFrom(x))
    return this;
  }

  mapTo(): BomHeader {
    throw new ApplicationError(UpdateBomHeadersCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/commodity/bomValueHeader/update";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(UpdateBomHeadersCommandHandler.name)
export class UpdateBomHeadersCommandHandler implements IRequestHandler<UpdateBomHeadersCommand, BomHeader> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: UpdateBomHeadersCommand): Promise<BomHeader> {
    let httpRequest: HttpRequest<UpdateBomHeadersCommand> = new HttpRequest<UpdateBomHeadersCommand>(request.url, request);
    
    return await this._httpService.Put<UpdateBomHeadersCommand, ServiceResult<BomHeader>>(httpRequest).toPromise().then(response => {
      this._notificationService.showSuccessMessage()
      return response.objResult
    })

  }
}
