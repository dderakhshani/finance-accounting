import {SearchQuery} from "../../../shared/services/search/models/search-query";
import {ServiceLocator} from "../service-locator/service-locator";
import {IdentityService} from "../../../modules/identity/repositories/identity.service";
import {Router} from "@angular/router";
import {EntityStates} from "../../enums/entity-states";

export interface IRequestHandler<T, K> {
  Validate?: (request: T) => void;
  Handle: (request: T) => Promise<K>;
  Log?: () => void;
}

export abstract class IRequest<HttpRequest, TResponse> {

  constructor() {
    let identityService = ServiceLocator.injector.get(IdentityService)
    let router = ServiceLocator.injector.get(Router)
    this.menueId = identityService.applicationUser.flatNavigations.find(x => x.route === router.routerState.snapshot.url.split('?')[0])?.id ?? 0
  }

  public menueId!: number;
  public state: EntityStates = EntityStates.Unchanged;

  public requestId: string = newGuid();

  public pageIndex!: number;
  public pageSize!: number;
  public orderByProperty!: string;
  public conditions?: SearchQuery[];

  private request!: HttpRequest;
  private response!: TResponse;

  abstract get url(): string;

  abstract get validationRules(): any

  abstract mapTo(): TResponse;

  abstract mapFrom(entity: TResponse): HttpRequest;

  public mapBasics(from: any, to: any) {
    let toKeys = Object.keys(to);
    Object.keys(from).forEach(key => {
      // @ts-ignore
      if (toKeys.includes(key) && key.toLowerCase() !== 'requestid') {
        if ((from[key] !== null && from[key] !== undefined) && (typeof from[key] === "string" || typeof from[key] === "number" || typeof from[key] === "boolean" || typeof from[key] === "object" || typeof from[key]?.getMonth === "function")) {
          // @ts-ignore
          to[key] = from[key]
        } else {
          to[key] = undefined
        }
      }
    });
  }
}

export interface IMediatorMiddleware {
  PreProcess: (request: any) => void;
  PostProcess: (request: any, response: any) => void;
}

type IHandler = Function;
type IRegisteredHandler = Function;

export interface IContainer {
  readonly handlers: { [id: string]: IHandler },
  readonly registeredHandlers: { [id: string]: IRegisteredHandler }
}

function newGuid() {
  return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
    var r = Math.random() * 16 | 0, v = c == 'x' ? r : (r & 0x3 | 0x8);
    return v.toString(16);
  });
}
