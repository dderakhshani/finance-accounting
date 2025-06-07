import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {Bank} from "../../../entities/bank";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {AddBankCommand} from "./add-bank-command";
import {Inject} from "@angular/core";
import {HttpService} from "../../../../../core/services/http/http.service";
import {HttpRequest} from "../../../../../core/services/http/http-request";
import {ServiceResult} from "../../../../../core/models/service-result";
import {NotificationService} from "../../../../../shared/services/notification/notification.service";

export class UpdateBankCommand extends IRequest<UpdateBankCommand,Bank> {
  public id:number | undefined = undefined;
  public code:string | undefined = undefined;
  public title:string | undefined = undefined
  public globalCode:string | undefined = undefined
  public typeBaseId:number | undefined = undefined
  public swiftCode:string | undefined = undefined
  public managerFullName:string | undefined = undefined
  public descriptions:string | undefined = undefined
  public phones:any[] = []

  mapFrom(entity: Bank): UpdateBankCommand {
    this.mapBasics(entity,this);
    return this;
  }

  mapTo(): Bank {
    return new Bank();
  }

  get url(): string {
    return "/logistics/bank/update";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(UpdateBankCommandHandler.name)
export class UpdateBankCommandHandler implements IRequestHandler<UpdateBankCommand, Bank> {
  constructor(
    @Inject(HttpService) private httpService:HttpService,
    @Inject(NotificationService) private notification:NotificationService,
  ) {
  }


  Handle(request: UpdateBankCommand) : Promise<Bank> {
    let httpRequest = new HttpRequest(request.url,request)

    return this.httpService.Put<UpdateBankCommand,ServiceResult<Bank>>(httpRequest).toPromise().then(res => {
      this.notification.showSuccessMessage()
      return res.objResult
    })
  }

}
