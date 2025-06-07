import {IRequest, IRequestHandler} from "../../../../core/services/mediator/interfaces";
import {Archive} from "../../entities/archive";
import {MediatorHandler} from "../../../../core/services/mediator/decorator";
import {HttpRequest} from "../../../../core/services/http/http-request";
import {Inject} from "@angular/core";
import {HttpService} from "../../../../core/services/http/http.service";
import {NotificationService} from "../../../../shared/services/notification/notification.service";


export class CreateArchiveCommand extends IRequest<CreateArchiveCommand, Archive> {
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

  mapFrom(entity: Archive): CreateArchiveCommand {
    throw new Error("Method not implemented.");
  }

  mapTo(): Archive {
    throw new Error("Method not implemented.");
  }

  get url(): string {
    return "/fileTransfer/Archive/Create";
  }

  get validationRules(): any[] {
    return [];
  }
}

@MediatorHandler(CreateArchiveCommandHandler.name)
export class CreateArchiveCommandHandler implements IRequestHandler<CreateArchiveCommand, Archive> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: CreateArchiveCommand): Promise<Archive> {
    let httpRequest: HttpRequest<CreateArchiveCommand> = new HttpRequest<CreateArchiveCommand>(request.url, request);

    return await this._httpService.Post<CreateArchiveCommand, Archive>(httpRequest).toPromise().then(response => {
      this._notificationService.showSuccessMessage()
      return response
    })
  }
}
