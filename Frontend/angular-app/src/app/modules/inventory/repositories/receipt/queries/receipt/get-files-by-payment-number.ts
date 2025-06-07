import { Inject } from "@angular/core";
import { ServiceResult } from "../../../../../../core/models/service-result";
import { IRequest, IRequestHandler } from "../../../../../../core/services/mediator/interfaces";
import { HttpService } from "../../../../../../core/services/http/http.service";
import { ApplicationError } from "../../../../../../core/exceptions/application-error";
import { MediatorHandler } from "../../../../../../core/services/mediator/decorator";
import { NotificationService } from "../../../../../../shared/services/notification/notification.service";
import { ValidationRule } from "../../../../../../core/validation/validation-rule";
import { HttpRequest } from "../../../../../../core/services/http/http-request";
import { FilesByPaymentNumber } from "../../../../entities/files-by-payment-number";

export class GetFilesByPaymentNumberQuery extends IRequest<GetFilesByPaymentNumberQuery, FilesByPaymentNumber[]> {
  constructor(public paymentNumber: string) {
    super();
  }


  mapFrom(entity: FilesByPaymentNumber[]): GetFilesByPaymentNumberQuery {
    throw new ApplicationError(GetFilesByPaymentNumberQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): FilesByPaymentNumber[] {
    throw new ApplicationError(GetFilesByPaymentNumberQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "https://sina.eefaceram.com/prime/accounting/api/GetFilesByPaymentNumberAsync";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(GetFilesByPaymentNumberQueryHandler.name)
export class GetFilesByPaymentNumberQueryHandler implements IRequestHandler<GetFilesByPaymentNumberQuery, FilesByPaymentNumber[]> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: GetFilesByPaymentNumberQuery): Promise<FilesByPaymentNumber[]> {
    this._notificationService.isLoader = true;
    let httpRequest: HttpRequest<GetFilesByPaymentNumberQuery> = new HttpRequest<GetFilesByPaymentNumberQuery>(request.url, request, false);
    httpRequest.Query += `paymentNumber=${request.paymentNumber}`;


    return await this._httpService.Get<FilesByPaymentNumber[]>(httpRequest).toPromise().then(response => {


      return response
    }).finally(() => {
      this._notificationService.isLoader = false;
    })
  }
}
