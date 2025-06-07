import {IRequest, IRequestHandler} from "../../../../core/services/mediator/interfaces";
import {Archive} from "../../entities/archive";
import {MediatorHandler} from "../../../../core/services/mediator/decorator";
import {HttpRequest} from "../../../../core/services/http/http-request";
import {Inject} from "@angular/core";
import {HttpService} from "../../../../core/services/http/http.service";
import {NotificationService} from "../../../../shared/services/notification/notification.service";


export class UpdateArchiveCommand extends IRequest<UpdateArchiveCommand, Archive> {
  public id: number | undefined = undefined;
  public baseValueTypeId: number | undefined = undefined;
  public typeBaseId: number | undefined = undefined;
  public fileNumber: number | undefined = undefined;
  public title: number | undefined = undefined;
  public description: number | undefined = undefined;
  public keyWords: string | undefined = undefined;
  public attachmentIds: number[] = [];

  constructor() {
    super();
  }

  mapFrom(entity: Archive): UpdateArchiveCommand {
    this.mapBasics(entity,this)
    return this;
  }

  mapTo(): Archive {
    throw new Error("Method not implemented.");
  }

  get url(): string {
    return "/fileTransfer/Archive/Update";
  }

  get validationRules(): any[] {
    return [];
  }
}

@MediatorHandler(UpdateArchiveCommandHandler.name)
export class UpdateArchiveCommandHandler implements IRequestHandler<UpdateArchiveCommand, Archive> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: UpdateArchiveCommand): Promise<Archive> {
    let httpRequest: HttpRequest<UpdateArchiveCommand> = new HttpRequest<UpdateArchiveCommand>(request.url, request);

    return await this._httpService.Put<UpdateArchiveCommand, Archive>(httpRequest).toPromise().then(response => {
      this._notificationService.showSuccessMessage()
      return response
    })
  }
}
