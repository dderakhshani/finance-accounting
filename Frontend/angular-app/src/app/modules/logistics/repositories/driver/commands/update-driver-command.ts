import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {Driver} from "../../../entities/driver";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {Inject} from "@angular/core";
import {HttpService} from "../../../../../core/services/http/http.service";
import {NotificationService} from "../../../../../shared/services/notification/notification.service";
import {HttpRequest} from "../../../../../core/services/http/http-request";
import {ServiceResult} from "../../../../../core/models/service-result";
import {UpdateBankCommand, UpdateBankCommandHandler} from "../../bank/commands/update-bank-command";

export class UpdateDriverCommand extends IRequest<UpdateDriverCommand, Driver>{
  public id: number | undefined = undefined;
  public  personId: number | undefined = undefined;
  public  driveingLisenceNumber: string | undefined = undefined;
  public  driveingLisenceExpiryDate: Date | undefined = undefined;
  public  drivingLisenceBaseTypeId: Date | undefined = undefined;
  public  rate: number | undefined = undefined;
  public  healthSmartCardNumber: string | undefined = undefined;
  public  healthSmartCardExpiryDate: Date | undefined = undefined;
  public  isBlock: boolean | undefined = undefined;
  public  blockReasonBaseId: number | undefined = undefined;
  public  workingMonthJson: any[] = []

  mapFrom(entity: Driver): UpdateDriverCommand {
    this.mapBasics(entity,this);
    return this;
  }

  mapTo(): Driver {
    return new Driver();
  }

  get url(): string {
    return "/logistics/driver/update";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(UpdateDriverCommandHandler.name)
export class UpdateDriverCommandHandler implements IRequestHandler<UpdateDriverCommand, Driver> {
  constructor(
    @Inject(HttpService) private httpService:HttpService,
    @Inject(NotificationService) private notification:NotificationService,
  ) {
  }


  Handle(request: UpdateDriverCommand) : Promise<Driver> {
    let httpRequest = new HttpRequest(request.url,request)

    return this.httpService.Put<UpdateDriverCommand,ServiceResult<Driver>>(httpRequest).toPromise().then(res => {
      this.notification.showSuccessMessage()
      return res.objResult
    })
  }

}
