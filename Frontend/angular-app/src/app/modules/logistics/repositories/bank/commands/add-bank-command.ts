import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {Bank} from "../../../entities/bank";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {HttpService} from "../../../../../core/services/http/http.service";
import {Inject} from "@angular/core";
import {HttpRequest} from "../../../../../core/services/http/http-request";
import {ServiceResult} from "../../../../../core/models/service-result";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {NotificationService} from "../../../../../shared/services/notification/notification.service";

export class AddBankCommand extends IRequest<AddBankCommand,Bank>{

  public code:string | undefined = undefined;
  public title:string | undefined = undefined
  public globalCode:string | undefined = undefined
  public typeBaseId:number | undefined = undefined
  public swiftCode:string | undefined = undefined
  public managerFullName:string | undefined = undefined
  public descriptions:string | undefined = undefined
  public phones:any[] = []

  mapFrom(entity: Bank): AddBankCommand {
    return new AddBankCommand();
    new Bank()
  }

  mapTo(): Bank {
    return new Bank();
  }

  get url(): string {
    return "/logistics/bank/add";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(AddBankCommandHandler.name)
export class AddBankCommandHandler implements IRequestHandler<AddBankCommand, Bank>{
  constructor(
    @Inject(HttpService) private httpService:HttpService,
    @Inject(NotificationService) private notification:NotificationService,
  ) {
  }

  Handle(request: AddBankCommand): Promise<Bank> {

    let httpRequest = new HttpRequest(request.url,request)

    return this.httpService.Post<AddBankCommand,ServiceResult<Bank>>(httpRequest).toPromise().then(res => {
      this.notification.showSuccessMessage()
      return res.objResult
    })
  }


}
