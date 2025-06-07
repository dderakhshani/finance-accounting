import { Inject } from "@angular/core";
import { MenuItem } from "../../../entities/menu-item";
import { ServiceResult } from "../../../../../core/models/service-result";
import { IRequest, IRequestHandler } from "../../../../../core/services/mediator/interfaces";
import { HttpService } from "../../../../../core/services/http/http.service";
import { ApplicationError } from "../../../../../core/exceptions/application-error";
import { MediatorHandler } from "../../../../../core/services/mediator/decorator";
import { NotificationService } from "../../../../../shared/services/notification/notification.service";
import { ValidationRule } from "../../../../../core/validation/validation-rule";
import { HttpRequest } from "../../../../../core/services/http/http-request";
import { environment } from "../../../../../../environments/environment";
export class UpdateMenuItemCommand extends IRequest<UpdateMenuItemCommand, MenuItem> {
  public DefaultImage: string = '/assets/icons/add-tree.svg'
  public id: number | undefined = undefined;
  public parentId: number | undefined = undefined;
  public title: string | undefined = undefined;
  public orderIndex: number | undefined = undefined;
  public permissionId: number | undefined = undefined;
  public helpUrl: string | undefined = undefined;
  public imageUrl: string | undefined = this.DefaultImage;
  public imageUrlReletiveAddress: string | undefined = undefined;
  public formUrl: string | undefined = undefined;
  public pageCaption: string | undefined = undefined;
  public isActive: boolean | undefined = undefined;

  constructor() {
    super();
  }

  mapFrom(entity: MenuItem): UpdateMenuItemCommand {
    this.mapBasics(entity, this);

    return this;
  }

  mapTo(): MenuItem {

    throw new ApplicationError(UpdateMenuItemCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/admin/menuItem/update";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(UpdateMenuItemCommandHandler.name)
export class UpdateMenuItemCommandHandler implements IRequestHandler<UpdateMenuItemCommand, MenuItem> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: UpdateMenuItemCommand): Promise<MenuItem> {
    if (request.imageUrl == request.DefaultImage) {
      request.imageUrl = undefined;
    }

    let httpRequest: HttpRequest<UpdateMenuItemCommand> = new HttpRequest<UpdateMenuItemCommand>(request.url, request);




    return await this._httpService.Put<UpdateMenuItemCommand, ServiceResult<MenuItem>>(httpRequest).toPromise().then(response => {
      this._notificationService.showSuccessMessage()
      return response.objResult
    })
  }
}
