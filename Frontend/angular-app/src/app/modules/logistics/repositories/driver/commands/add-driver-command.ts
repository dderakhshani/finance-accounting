import {Driver} from "../../../entities/driver";
import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {Bank} from "../../../entities/bank";
import {Inject} from "@angular/core";
import {HttpService} from "../../../../../core/services/http/http.service";
import {NotificationService} from "../../../../../shared/services/notification/notification.service";
import {HttpRequest} from "../../../../../core/services/http/http-request";
import {ServiceResult} from "../../../../../core/models/service-result";
import {AddBankCommand} from "../../bank/commands/add-bank-command";

export class AddDriverCommand extends IRequest<AddDriverCommand,Driver>{

  public  personId: number | undefined = undefined;
  public  driveingLisenceNumber: string | undefined = undefined;
  public  driveingLisenceExpiryDate: Date | undefined = undefined;
  public  drivingLisenceBaseTypeId: Date | undefined = undefined;
  public  rate: number | undefined = undefined;
  public  healthSmartCardNumber: string | undefined = undefined;
  public  healthSmartCardExpiryDate: Date | undefined = undefined;
  public  isBlock: boolean | undefined = undefined;
  public  blockReasonBaseId: number | undefined = undefined;
  //public  workingMonthJson: any[] = []

  mapFrom(entity: Driver): AddDriverCommand {
    return new AddDriverCommand();
    new Driver()
  }

  mapTo(): Driver {
    return new Driver();
  }

  get url(): string {
    return "/logistics/driver/add";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(AddDriverCommandHandler.name)
export class AddDriverCommandHandler implements IRequestHandler<AddDriverCommand, Driver>{
  constructor(
    @Inject(HttpService) private httpService:HttpService,
    @Inject(NotificationService) private notification:NotificationService,
  ) {
  }

  Handle(request: AddDriverCommand): Promise<Driver> {

    let httpRequest = new HttpRequest(request.url,request)

    return this.httpService.Post<AddDriverCommand,ServiceResult<Driver>>(httpRequest).toPromise().then(res => {
      this.notification.showSuccessMessage()
      return res.objResult
    })
  }


}
