import {ApplicationError} from "../../../../../../core/exceptions/application-error";
import {ValidationRule} from "../../../../../../core/validation/validation-rule";
import {MediatorHandler} from "../../../../../../core/services/mediator/decorator";
import {IRequest, IRequestHandler} from "../../../../../../core/services/mediator/interfaces";
import {Inject} from "@angular/core";
import {ServiceResult} from "../../../../../../core/models/service-result";
import {HttpService} from "../../../../../../core/services/http/http.service";
import {NotificationService} from "../../../../../../shared/services/notification/notification.service";
import {HttpRequest} from "../../../../../../core/services/http/http-request";
import { ReceiptsInsertedByCustomers } from "src/app/modules/bursary/entities/receipts-inserted-by-customers";
import { ReceiptInsertedByCustomerStatus } from "src/app/modules/bursary/entities/enums";
import { environment } from "src/environments/environment";


export class UpdateStatusReceiptInsertedByCustomersCommand extends IRequest<UpdateStatusReceiptInsertedByCustomersCommand, ReceiptsInsertedByCustomers> {

  public id : number | undefined = undefined;
  public description : string | undefined = undefined;
  public status : number | undefined = ReceiptInsertedByCustomerStatus.Remove;
  constructor(id:number) {
    super();
    this.id = id;

  }

  mapFrom(entity: ReceiptsInsertedByCustomers): UpdateStatusReceiptInsertedByCustomersCommand {
    throw new ApplicationError(UpdateStatusReceiptInsertedByCustomersCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): ReceiptsInsertedByCustomers {
    throw new ApplicationError(UpdateStatusReceiptInsertedByCustomersCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return environment.crmServerAddress+"/api/Deposits/UpdateStatus/"+this.id;
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(UpdateStatusReceiptInsertedByCustomersCommandHandler.name)
export class UpdateStatusReceiptInsertedByCustomersCommandHandler implements IRequestHandler<UpdateStatusReceiptInsertedByCustomersCommand, boolean> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: UpdateStatusReceiptInsertedByCustomersCommand): Promise<boolean> {
    let httpRequest: HttpRequest<UpdateStatusReceiptInsertedByCustomersCommand> = new HttpRequest<UpdateStatusReceiptInsertedByCustomersCommand>(request.url, request,false);
    httpRequest.Headers = httpRequest.Headers.append("Authorization","Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiI5Iiwic2lkIjoiOSIsInN1YiI6ImFkbWluIiwiZW1haWwiOiJzdHJpbmciLCJqdGkiOiI2NjcxOGNlOS0yODM5LTRiOTYtODdkNC1jZTk3M2Q4NjhjNTQiLCJleHAiOjE3MDYxNDUwODMsImlzcyI6IkVlZmFDZXJhbS5jb20iLCJhdWQiOiJFRUZBLUNSTSJ9.wQ8mnBw4KyDGM4BBE3X7XRbhN5ppLDC4EigXARdJzNE");

    return await this._httpService.Put<UpdateStatusReceiptInsertedByCustomersCommand, boolean>(httpRequest).toPromise().then(response => {
      this._notificationService.showSuccessMessage()
      if (this._notificationService)
      return true;
      else
      return false;
    })

  }
}
