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
import {CreateBomItemCommand} from "../../bom-item/commands/create-bom-item-command";

export class CreateBomCommand extends IRequest<CreateBomCommand, Bom> {

  public rootId: number | undefined = undefined;
  public title: string | undefined = undefined;

  public description: string | undefined = undefined;
 

  public commodityCategoryId: number | undefined = undefined;
  public isActive: boolean | undefined = undefined;
  public bomItems: CreateBomItemCommand[] = []
    

  constructor() {
    super();
  }

  mapFrom(entity: Bom): CreateBomCommand {
    this.mapBasics(entity, this);
    this.bomItems = entity.items.map(x => new CreateBomItemCommand().mapFrom(x))
    return this;
  }

  mapTo(): Bom {
    throw new ApplicationError(CreateBomCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/commodity/bom/add";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(CreateBomCommandHandler.name)
export class CreateBomCommandHandler implements IRequestHandler<CreateBomCommand, Bom> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: CreateBomCommand): Promise<Bom> {
    let httpRequest: HttpRequest<CreateBomCommand> = new HttpRequest<CreateBomCommand>(request.url, request);
    // httpRequest.Query  += ;
    // httpRequest.Headers = httpRequest.Headers.append()

    return await this._httpService.Post<CreateBomCommand, ServiceResult<Bom>>(httpRequest).toPromise().then(response => {
      this._notificationService.showSuccessMessage()
      return response.objResult
    })

  }
}
