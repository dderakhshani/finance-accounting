import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {ApplicationError} from "../../../../../core/exceptions/application-error";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {Inject} from "@angular/core";
import {HttpService} from "../../../../../core/services/http/http.service";
import {HttpRequest} from "../../../../../core/services/http/http-request";
import {ServiceResult} from "../../../../../core/models/service-result";
import {PayableDocument, PayableDocumentAccount, PayableDocumentPayOrder} from "../../../entities/payableDocument";

export class CreatePayableDocumentsCommand  extends IRequest<CreatePayableDocumentsCommand , PayableDocument> {

  // payTypeId: number =28497;
  payTypeId: number =28777;
  monetarySystemId: number =29340;
  showCredit:boolean =false;
  showDebit:boolean =false;
  chequeTypeId: number | undefined = undefined;
  chequeBookSheetId: number | undefined = undefined;
  bankAccountId: number | undefined = undefined;
  creditAccountHeadId: number | undefined = undefined;
  creditReferenceGroupId: number | undefined = undefined;
  creditReferenceId: number | undefined = undefined;
  currencyTypeBaseId: number | undefined = undefined;
  dueDate: string | undefined = undefined;
  draftDate: string | undefined = undefined;
  subjectId: number | undefined = undefined;
  currencyRate: number | undefined = undefined;
  currencyAmount: number | undefined = undefined;
  amount: number | undefined = undefined;
  descp: string | undefined = undefined;
  documentNo: string | undefined = undefined;
  accounts: PayableDocumentAccount[] = [];
  payOrders: PayableDocumentPayOrder[] = [];
  referenceNumber : string | undefined = undefined;
  trackingNumber : string | undefined = undefined;

  constructor() {
    super();
  }

  mapFrom(entity: PayableDocument): CreatePayableDocumentsCommand  {
    throw new ApplicationError(CreatePayableDocumentsCommand .name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): PayableDocument {
    throw new ApplicationError(CreatePayableDocumentsCommand .name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/bursary/Payable_Documents/Add";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(CreatePayableDocumentsCommandHandler.name)
export class CreatePayableDocumentsCommandHandler implements IRequestHandler<CreatePayableDocumentsCommand , PayableDocument> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
  ) {
  }

  async Handle(request: CreatePayableDocumentsCommand ): Promise<PayableDocument> {
    let httpRequest: HttpRequest<CreatePayableDocumentsCommand > = new HttpRequest<CreatePayableDocumentsCommand >(request.url, request);

    return await this._httpService.Post<CreatePayableDocumentsCommand , ServiceResult<PayableDocument>>(httpRequest).toPromise().then(response => {
      return response.objResult
    })
  }
}
