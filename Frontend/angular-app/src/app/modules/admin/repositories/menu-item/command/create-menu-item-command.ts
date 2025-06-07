import {Inject} from "@angular/core";
import {MenuItem} from "../../../entities/menu-item";
import {ServiceResult} from "../../../../../core/models/service-result";
import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {HttpService} from "../../../../../core/services/http/http.service";
import {ApplicationError} from "../../../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {NotificationService} from "../../../../../shared/services/notification/notification.service";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {HttpRequest} from "../../../../../core/services/http/http-request";

export class CreateMenuItemCommand extends IRequest<CreateMenuItemCommand, MenuItem> {
  public DefaultImage: string = '/assets/icons/add-tree.svg'
   public id!: number;
   public parentId: number| undefined = undefined;
   public title: string| undefined = undefined;
   public  orderIndex: number| undefined = undefined;
   public permissionId: number | undefined = undefined;
   public  helpUrl: string| undefined = undefined;
   public imageUrl: string | undefined = undefined;
   public imageUrlReletiveAddress: string | undefined = this.DefaultImage;
   public formUrl: string| undefined = undefined;
   public pageCaption: string| undefined = undefined;
   public isActive?: boolean | undefined = undefined;


  constructor() {
    super();
  }

  mapFrom(entity: MenuItem): CreateMenuItemCommand {
    throw new ApplicationError(CreateMenuItemCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): MenuItem {
    throw new ApplicationError(CreateMenuItemCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/admin/menuItem/add";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(CreateMenuItemCommandHandler.name)
export class CreateMenuItemCommandHandler implements IRequestHandler<CreateMenuItemCommand, MenuItem> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: CreateMenuItemCommand): Promise<MenuItem> {
    let httpRequest: HttpRequest<CreateMenuItemCommand> = new HttpRequest<CreateMenuItemCommand>(request.url, request);


    return await this._httpService.Post<CreateMenuItemCommand, ServiceResult<MenuItem>>(httpRequest).toPromise().then(response => {
       this._notificationService.showSuccessMessage()
      return response.objResult
    })


  }
}
