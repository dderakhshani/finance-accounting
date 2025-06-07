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
import { environment } from "src/environments/environment";


export class UpdateReceiptsInsertedByCustomersCommand extends IRequest<UpdateReceiptsInsertedByCustomersCommand, ReceiptsInsertedByCustomers> {

  public id : number | undefined = undefined;
  constructor(id:number) {
    super();
    this.id = id;
  }

  mapFrom(entity: ReceiptsInsertedByCustomers): UpdateReceiptsInsertedByCustomersCommand {
    throw new ApplicationError(UpdateReceiptsInsertedByCustomersCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): ReceiptsInsertedByCustomers {
    throw new ApplicationError(UpdateReceiptsInsertedByCustomersCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return environment.crmServerAddress+"/api/Deposits/Confirm/"+this.id;
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(UpdateReceiptsInsertedByCustomersCommandHandler.name)
export class UpdateReceiptsInsertedByCustomersCommandHandler implements IRequestHandler<UpdateReceiptsInsertedByCustomersCommand, boolean> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: UpdateReceiptsInsertedByCustomersCommand): Promise<boolean> {
    let httpRequest: HttpRequest<UpdateReceiptsInsertedByCustomersCommand> = new HttpRequest<UpdateReceiptsInsertedByCustomersCommand>(request.url, request,false);
    httpRequest.Headers = httpRequest.Headers.append("Authorization","Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiI5Iiwic2lkIjoiOSIsInN1YiI6ImFkbWluIiwiZW1haWwiOiJzdHJpbmciLCJqdGkiOiI2NjcxOGNlOS0yODM5LTRiOTYtODdkNC1jZTk3M2Q4NjhjNTQiLCJleHAiOjE3MDYxNDUwODMsImlzcyI6IkVlZmFDZXJhbS5jb20iLCJhdWQiOiJFRUZBLUNSTSJ9.wQ8mnBw4KyDGM4BBE3X7XRbhN5ppLDC4EigXARdJzNE");

    return await this._httpService.Put<UpdateReceiptsInsertedByCustomersCommand, boolean>(httpRequest).toPromise().then(response => {
      this._notificationService.showSuccessMessage()
      if (this._notificationService)
      return true;
      else
      return false;
    })

  }
}
