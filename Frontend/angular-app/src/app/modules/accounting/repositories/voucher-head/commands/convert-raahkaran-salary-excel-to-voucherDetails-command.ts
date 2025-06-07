import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {VoucherDetail} from "../../../entities/voucher-detail";
import {ApplicationError} from "../../../../../core/exceptions/application-error";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {Inject} from "@angular/core";
import {HttpService} from "../../../../../core/services/http/http.service";
import {NotificationService} from "../../../../../shared/services/notification/notification.service";
import {HttpRequest} from "../../../../../core/services/http/http-request";
import {ServiceResult} from "../../../../../core/models/service-result";

export class ConvertRaahkaranSalaryExcelToVoucherDetailsCommand extends IRequest<ConvertRaahkaranSalaryExcelToVoucherDetailsCommand, VoucherDetail[]> {

  public excelFile: File | undefined = undefined;
  public isFactory: boolean | undefined = undefined;

  constructor() {
    super();
  }

  mapFrom(entity: VoucherDetail[]): ConvertRaahkaranSalaryExcelToVoucherDetailsCommand {
    throw new ApplicationError(ConvertRaahkaranSalaryExcelToVoucherDetailsCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): VoucherDetail[] {
    throw new ApplicationError(ConvertRaahkaranSalaryExcelToVoucherDetailsCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/accounting/VouchersHead/ConvertRaahKaranSalaryExcelToVoucherDetails";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}


@MediatorHandler(ConvertRaahkaranSalaryExcelToVoucherDetailsCommandHandler.name)
export class ConvertRaahkaranSalaryExcelToVoucherDetailsCommandHandler implements IRequestHandler<ConvertRaahkaranSalaryExcelToVoucherDetailsCommand, VoucherDetail[]> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService
  ) {
  }

  async Handle(request: ConvertRaahkaranSalaryExcelToVoucherDetailsCommand): Promise<VoucherDetail[]> {
    let httpRequest: HttpRequest<ConvertRaahkaranSalaryExcelToVoucherDetailsCommand> = new HttpRequest<ConvertRaahkaranSalaryExcelToVoucherDetailsCommand>(request.url, request);
    httpRequest.BodyFormat = 'form';
    // httpRequest.Query  += ;
    // httpRequest.Headers = httpRequest.Headers.append()

    return await this._httpService.Post<ConvertRaahkaranSalaryExcelToVoucherDetailsCommand, ServiceResult<VoucherDetail[]>>(httpRequest).toPromise().then(response => {
      this._notificationService.showSuccessMessage()
      return response.objResult
    })
  }
}

