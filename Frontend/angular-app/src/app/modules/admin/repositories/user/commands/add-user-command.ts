import {User} from "../../../entities/user";
import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {Inject} from "@angular/core";
import {HttpService} from "../../../../../core/services/http/http.service";
import {HttpRequest} from "../../../../../core/services/http/http-request";
import {ServiceResult} from "../../../../../core/models/service-result";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {BaseValue} from "../../../entities/base-value";
import {NotificationService} from "../../../../../shared/services/notification/notification.service";

export class AddUserCommand extends IRequest<AddUserCommand,User>{
  public personId:number | undefined = undefined;
  public username:string | undefined = undefined;
  public password:string | undefined = undefined;
  public confirmPassword:string | undefined = undefined;
  public isBlocked:boolean | undefined = undefined;
  public blockedReasonBaseId:boolean | undefined = undefined;
  public rolesIdList:number[] = [];
  public userAllowedYears:number[] = [];

  public lastOnlineTime:Date  | undefined = undefined;
  public failedCount:number  | undefined = undefined;
  public unitPositionTitle:string  | undefined = undefined;

  mapFrom(entity: User): AddUserCommand {
    return new AddUserCommand();
  }

  mapTo(): User {
    return new User();
  }

  get url(): string {
    return "/admin/User/add";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(AddUserCommandHandler.name)
export class AddUserCommandHandler implements IRequestHandler<AddUserCommand, User>{
  constructor(
    @Inject(HttpService) private _httpService:HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }
  async Handle(request:AddUserCommand) {
    let httpRequest = new HttpRequest(request.url,request);

    return await this._httpService.Post<AddUserCommand,ServiceResult<User>>(httpRequest).toPromise().then(res => {
      this._notificationService.showSuccessMessage()
      return res.objResult;
    })
  }
}
