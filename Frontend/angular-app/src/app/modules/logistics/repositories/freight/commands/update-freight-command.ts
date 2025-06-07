import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {Freight} from "../../../entities/freight";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {Inject} from "@angular/core";
import {HttpService} from "../../../../../core/services/http/http.service";
import {NotificationService} from "../../../../../shared/services/notification/notification.service";
import {HttpRequest} from "../../../../../core/services/http/http-request";
import {ServiceResult} from "../../../../../core/models/service-result";

export class UpdateFreightCommand extends IRequest<UpdateFreightCommand, Freight>{

  public id:number |undefined= undefined;
  public personId: number|undefined= undefined
  public isActive:boolean|undefined= undefined
  public isBlock: boolean|undefined= undefined
  public rate:number|undefined= undefined
  public managerFullName: string|undefined= undefined
  public descriptions: string|undefined= undefined;

  mapFrom(entity: Freight): UpdateFreightCommand {
    this.mapBasics(entity,this);
    return this;  }

  mapTo(): Freight {
    return new Freight;
  }

  get url(): string {
    return "/logestic/freight/update";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(UpdateFreightCommandHandler.name)
export class UpdateFreightCommandHandler implements IRequestHandler<UpdateFreightCommand, Freight> {
  constructor(
    @Inject(HttpService) private httpService:HttpService,
    @Inject(NotificationService) private notification:NotificationService,
  ) {
  }


  Handle(request: UpdateFreightCommand) : Promise<Freight> {
    let httpRequest = new HttpRequest(request.url,request)

    return this.httpService.Put<UpdateFreightCommand,ServiceResult<Freight>>(httpRequest).toPromise().then(res => {
      this.notification.showSuccessMessage()
      return res.objResult
    })
  }

}
