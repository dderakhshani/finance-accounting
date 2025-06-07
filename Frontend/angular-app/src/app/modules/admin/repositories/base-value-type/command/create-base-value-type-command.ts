import {Inject} from "@angular/core";
import {BaseValueType} from "../../../entities/base-value-type";
import {ServiceResult} from "../../../../../core/models/service-result";
import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {HttpService} from "../../../../../core/services/http/http.service";
import {ApplicationError} from "../../../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {NotificationService} from "../../../../../shared/services/notification/notification.service";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {HttpRequest} from "../../../../../core/services/http/http-request";

export class CreateBaseValueTypeCommand extends IRequest<CreateBaseValueTypeCommand, BaseValueType> {
  public id!: number;
  public parentId: number| undefined = undefined;
  public levelCode: string| undefined = undefined;
  public title: string| undefined = undefined;
  public uniqueName: string| undefined = undefined;
  public groupName: string| undefined = undefined;
  public subSystem: string| undefined = undefined;
  public isReadOnly: boolean | undefined = undefined;


  constructor() {
    super();
  }

  mapFrom(entity: BaseValueType): CreateBaseValueTypeCommand {
    throw new ApplicationError(CreateBaseValueTypeCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): BaseValueType {
    throw new ApplicationError(CreateBaseValueTypeCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/admin/BaseValueType/add";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(CreateBaseValueTypeCommandHandler.name)
export class CreateBaseValueTypeCommandHandler implements IRequestHandler<CreateBaseValueTypeCommand, BaseValueType> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: CreateBaseValueTypeCommand): Promise<BaseValueType> {
    let httpRequest: HttpRequest<CreateBaseValueTypeCommand> = new HttpRequest<CreateBaseValueTypeCommand>(request.url, request);


    return await this._httpService.Post<CreateBaseValueTypeCommand, ServiceResult<BaseValueType>>(httpRequest).toPromise().then(response => {
       this._notificationService.showSuccessMessage()
      return response.objResult
    })


  }
}
