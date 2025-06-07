import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {Usance} from "../../../entities/usance";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {Inject} from "@angular/core";
import {HttpService} from "../../../../../core/services/http/http.service";
import {ServiceResult} from "../../../../../core/models/service-result";
import {HttpRequest} from "../../../../../core/services/http/http-request";

export class AddUsanceCommand extends IRequest<AddUsanceCommand,Usance>{
  public usanceNumber: string  | undefined=undefined;
  public usanceIssueDate: Date | undefined=undefined;
  public issuerFreightId: number | undefined=undefined;
  public originPersonId: number | undefined=undefined;
  public destinationPersonId: number | undefined=undefined;
  public firstDriverId: number | undefined=undefined;
  public secondDriverId: number | undefined=undefined;
  public weight: number | undefined=undefined;
  public vehicleId: number | undefined=undefined;
  public perosnBankAccountId: number | undefined=undefined;
  public wagePerUnit: number | undefined=undefined;
  public totallWage: number | undefined=undefined;
  public extraWage: number | undefined=undefined;
  public extraDescriptions: string | undefined=undefined;
  public descriptions: string | undefined=undefined;
  public commodityTypeBaseId: number | undefined=undefined;
  public commodityDescriptions: string | undefined=undefined;
  public containerNumber: string | undefined=undefined


  mapFrom(entity: Usance): AddUsanceCommand {
    return new AddUsanceCommand();
    new Usance()
  }


  mapTo(): Usance {
    return new Usance()
  }

  get url(): string {
    return "/Logistic/usance/add";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }

}
@MediatorHandler(AddUsanceCommandHandler.name)
export class AddUsanceCommandHandler implements IRequestHandler<AddUsanceCommand, Usance>{

  constructor(
    @Inject(HttpService) private httpService:HttpService,
  ) {
  }

  Handle(request: AddUsanceCommand): Promise<Usance> {
    let httpRequest=new HttpRequest(request.url,request)
    return this.httpService.Post<AddUsanceCommand,ServiceResult<Usance>>(httpRequest).toPromise().then(res=>{
      return res.objResult;
    })
  }
}
