import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {User} from "../../../entities/user";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {Inject} from "@angular/core";
import {HttpService} from "../../../../../core/services/http/http.service";
import {HttpRequest} from "../../../../../core/services/http/http-request";
import {ServiceResult} from "../../../../../core/models/service-result";
import {NotificationService} from "../../../../../shared/services/notification/notification.service";

export class UpdateUserCommand extends IRequest<UpdateUserCommand, User>{
  public id:number | undefined = undefined;
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
  public firstName:string  | undefined = undefined;
  public lastName:string  | undefined = undefined;
  public nationalNumber:string  | undefined = undefined;

  mapFrom(entity: User): UpdateUserCommand {
    this.mapBasics(entity,this)
    this.rolesIdList = entity.rolesIdList
    this.userAllowedYears = entity.userYears
    return this;
  }

  mapTo(): User {
    return new User();
  }

  get url(): string {
    return "/admin/User/update";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(UpdateUserCommandHandler.name)
export class UpdateUserCommandHandler implements IRequestHandler<UpdateUserCommand, User> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {

  }
  async Handle(request: UpdateUserCommand) {
    let httpRequest = new HttpRequest(request.url,request);
    return await this._httpService.Put<UpdateUserCommand,ServiceResult<User>>(httpRequest).toPromise().then(res => {
      this._notificationService.showSuccessMessage()
      return res.objResult
    })
  }
}
