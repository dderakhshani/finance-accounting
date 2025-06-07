import {Inject} from "@angular/core";
import {ServiceResult} from "../../../../../core/models/service-result";
import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {BaseValue} from "../../../entities/base-value";
import {HttpService} from "../../../../../core/services/http/http.service";
import {ApplicationError} from "../../../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {NotificationService} from "../../../../../shared/services/notification/notification.service";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {HttpRequest} from "../../../../../core/services/http/http-request";

export class UpdateBaseValueCommand extends IRequest<UpdateBaseValueCommand, BaseValue> {
  public id: number | undefined = undefined;
  public parentId: number | undefined = undefined;
  public baseValueTypeId: number | undefined = undefined;
  public title: string | undefined = undefined;
  public value: string | undefined = undefined;
  public isReadOnly: boolean | undefined = undefined;
  public orderIndex: number | undefined = undefined;
  public uniqueName: string | undefined = undefined;
  public code: string | undefined = undefined;

  constructor() {
    super();
  }

  mapFrom(entity: BaseValue): UpdateBaseValueCommand {
    this.mapBasics(entity,this)
    return this;
  }

  mapTo(): BaseValue {
    throw new ApplicationError(UpdateBaseValueCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/admin/basevalue/update";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(UpdateBaseValueCommandHandler.name)
export class UpdateBaseValueCommandHandler implements IRequestHandler<UpdateBaseValueCommand, BaseValue> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: UpdateBaseValueCommand): Promise<BaseValue> {
    let httpRequest: HttpRequest<UpdateBaseValueCommand> = new HttpRequest<UpdateBaseValueCommand>(request.url, request);

    return await this._httpService.Put<UpdateBaseValueCommand, ServiceResult<BaseValue>>(httpRequest).toPromise().then(response => {
      this._notificationService.showSuccessMessage()
      return response.objResult
    })

  }
}
