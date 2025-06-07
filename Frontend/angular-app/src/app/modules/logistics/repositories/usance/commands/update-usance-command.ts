import {Usance} from "../../../entities/usance";
import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {HttpService} from "../../../../../core/services/http/http.service";
import {Inject} from "@angular/core";
import {ServiceResult} from "../../../../../core/models/service-result";
import {HttpRequest} from "../../../../../core/services/http/http-request";

export class UpdateUsanceCommand extends IRequest<UpdateUsanceCommand,Usance>  {
  public id:number | undefined=undefined
  public usanceNumber: string  | undefined=undefined
  public usanceIssueDate: Date | undefined=undefined
  public originPersonId: number | undefined=undefined
  public destinationPersonId: number | undefined=undefined
  public firstDriverId: number | undefined=undefined
  public secondDriverId: number | undefined=undefined
  public weight: number | undefined=undefined
  public vehicleId: number | undefined=undefined
  public perosnBankAccountId: number | undefined=undefined
  public wagePerUnit: number | undefined=undefined
  public totallWage: number | undefined=undefined
  public extraWage: number | undefined=undefined
  public extraDescriptions: string | undefined=undefined;
  public descriptions: string | undefined=undefined
  public commodityTypeBaseId: number | undefined=undefined;
  public commodityDescriptions: string | undefined=undefined;
  public containerNumber: string | undefined=undefined


  mapFrom(entity: Usance): UpdateUsanceCommand {
    this.mapBasics(this,entity)
    return this;
  }


  mapTo(): Usance {
    return new Usance()
  }

  get url(): string {
    return "/Logistic/usance/update";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}
@MediatorHandler(UpdateUsanceCommandHandler.name)
export class UpdateUsanceCommandHandler implements IRequestHandler<UpdateUsanceCommand, Usance>{

  constructor(
    @Inject(HttpService) private httpService:HttpService,
  ) {
  }

  Handle(request: UpdateUsanceCommand): Promise<Usance> {
    let httpRequest=new HttpRequest(request.url,request)
    return this.httpService.Put<UpdateUsanceCommand,ServiceResult<Usance>>(httpRequest).toPromise().then(res=>{
      return res.objResult;
    })
  }
}
