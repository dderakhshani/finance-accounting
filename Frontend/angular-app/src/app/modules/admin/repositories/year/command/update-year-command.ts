import {HttpRequest} from "../../../../../core/services/http/http-request";
import {ServiceResult} from "../../../../../core/models/service-result";
import {Year} from "../../../entities/year";
import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {HttpService} from "../../../../../core/services/http/http.service";
import {ApplicationError} from "../../../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {NotificationService} from "../../../../../shared/services/notification/notification.service";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import { Inject } from "@angular/core";

export class UpdateYearCommand extends IRequest<UpdateYearCommand, Year> {
  public id:number | undefined = undefined;
  public companyId:number | undefined = undefined;
  public yearName:number | undefined = undefined;
  public firstDate:Date | undefined = undefined;
  public lastDate:Date | undefined = undefined;
  public isEditable:boolean | undefined = undefined;
  public isCalculable:boolean | undefined = undefined;
  public isCurrentYear:boolean | undefined = undefined;
  public lastEditableDate:Date | undefined = undefined;
  public accountRelationCode:string | undefined = undefined;
  constructor() {
    super();
  }

  mapFrom(entity: Year): UpdateYearCommand {
    this.mapBasics(entity,this);
    return this;
  }

  mapTo(): Year {
    throw new ApplicationError(UpdateYearCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/admin/year/update";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(UpdateYearCommandHandler.name)
export class UpdateYearCommandHandler implements IRequestHandler<UpdateYearCommand, Year> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: UpdateYearCommand): Promise<Year> {
    let httpRequest: HttpRequest<UpdateYearCommand> = new HttpRequest<UpdateYearCommand>(request.url, request);




    return await this._httpService.Put<UpdateYearCommand, ServiceResult<Year>>(httpRequest).toPromise().then(response => {
      this._notificationService.showSuccessMessage()
      return response.objResult
    })

  }
}
