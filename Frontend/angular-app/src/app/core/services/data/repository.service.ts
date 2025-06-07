import { Injectable } from '@angular/core';
import {IRequest} from "../mediator/interfaces";
import {CacheService} from "./cache.service";
import {PaginatedList} from "../../models/paginated-list";

@Injectable({
  providedIn: 'root'
})
export class RepositoryService {

  constructor(
    private cacheService:CacheService
  ) { }


  public async getAll<T>(request:IRequest<any, T[]>) : Promise<T[]> {
    let a:T;
    let entities : T[] = this.cacheService.get('');

    return entities;
  }
}
