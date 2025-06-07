import {IRequest} from "../mediator/interfaces";
import {SearchComparisonTypes, SearchQuery} from "../../../shared/services/search/models/search-query";
import {PaginatedList} from "../../models/paginated-list";
import {Mediator} from "../mediator/mediator.service";
import {Injectable} from "@angular/core";

@Injectable({
  providedIn: 'root',

})
export class DataCacheService<T> {
  constructor(private mediator: Mediator) {
  }

  public list: PaginatedList<T> = new PaginatedList<T>();
  public isLoaded: boolean = false;

  public async get(request: IRequest<any, PaginatedList<T> | T[]>): Promise<PaginatedList<T>> {
    if (!this.isLoaded) {
      let originalPageIndex = request.pageIndex;
      let originalPageSize = request.pageSize;
      let originalConditions = request.conditions;
      let originalOrderByProperty = request.orderByProperty;
      request.pageIndex = 0;
      request.pageSize = 0;
      request.conditions = [];
      request.orderByProperty = '';

      await this.mediator.send(request).then(res => {
        // @ts-ignore
        this.list = res && res?.data ? res : <PaginatedList<T>>{data: res};
      })

      request.pageIndex = originalPageIndex;
      request.pageSize = originalPageSize;
      request.conditions = originalConditions;
      request.orderByProperty = originalOrderByProperty;

      this.isLoaded = true;
    }

    let list = {...this.list};
    if (request.conditions) list.data = this.filter(list.data, request.conditions);
    if (request.pageIndex >= 0 && request.pageSize > 0) list.data = this.paginate(list.data, request.pageIndex, request.pageSize);
    if (request.orderByProperty) list.data = this.sort(list.data, request.orderByProperty)
    list.totalCount = list.data.length;


    return list;
  }

  public filter(entities: T[], conditions: SearchQuery[]): T[] {
    let filteredEntities: T[] = entities;
    let filterExpression = '';
    conditions = conditions.map(x => {
      x.nextOperand = x.nextOperand === 'or' ? '||' : '&&'
      return x;
    })
    conditions.forEach((condition) => {
      if (condition.comparison === SearchComparisonTypes.equals) {
        filterExpression += `x.${condition.propertyName} === \"${condition.values[0]}\" ${condition.nextOperand}`;
      }
      if (condition.comparison === SearchComparisonTypes.notEquals) {
        filterExpression += `x.${condition.propertyName} !== \"${condition.values[0]}\" ${condition.nextOperand}`;
      }
      if (condition.comparison === SearchComparisonTypes.contains) {
        filterExpression += `x.${condition.propertyName}.includes(\"${condition.values[0]}\") ${condition.nextOperand}`;
      }
      if (condition.comparison === SearchComparisonTypes.notContains) {
        filterExpression += `!x.${condition.propertyName}.includes(\"${condition.values[0]}\") ${condition.nextOperand}`;
      }
      if (condition.comparison === SearchComparisonTypes.between) {
        filterExpression += `(x.${condition.propertyName} >= \"${condition.values[0]}\" && x.${condition.propertyName} <= \"${condition.values[1]}\") ${condition.nextOperand}`;
      }
      if (condition.comparison === SearchComparisonTypes.greaterThan) {
        filterExpression += `x.${condition.propertyName} >= \"${condition.values[0]}\" ${condition.nextOperand}`;
      }
      if (condition.comparison === SearchComparisonTypes.lessThan) {
        filterExpression += `x.${condition.propertyName} <= \"${condition.values[0]}\" ${condition.nextOperand}`;
      }
      if (condition.comparison === SearchComparisonTypes.startsWith) {
        filterExpression += `x.${condition.propertyName}.startsWith(\"${condition.values[0]}\") ${condition.nextOperand}`;
      }
      if (condition.comparison === SearchComparisonTypes.endsWith) {
        filterExpression += `x.${condition.propertyName}.endsWith(\"${condition.values[0]}\") ${condition.nextOperand}`;
      }


      if (condition.comparison === SearchComparisonTypes.in) {
      }
      if (condition.comparison === SearchComparisonTypes.inList) {
      }
      if (condition.comparison === SearchComparisonTypes.ofList) {
      }

    })

    if (filterExpression.endsWith('||') || filterExpression.endsWith('&&')) {
      filterExpression = filterExpression.slice(0, filterExpression.length - 2)
    }
    if (filterExpression) {
      filteredEntities = entities.filter((x: T) => eval(filterExpression));
    }

    return filteredEntities;
  }

  public paginate(entities: T[], pageIndex: number, pageSize: number): T[] {
    let startIndex = pageIndex * pageSize;
    let endIndex = startIndex + pageSize;
    let paginatedList = entities.slice(startIndex, endIndex);
    return paginatedList;
  }

  public sort(entities: T[], sortString: string): T[] {
    let sortKeys = sortString.split(',')
    let sortedEntities: T[] = [];
    sortKeys.forEach(sort => {
      let sortKey = sort.split(' ')[0]
      let sortDirection = sort.split(' ')[1]
      sortedEntities = entities.sort(function (a: any, b: any) {
        let x = a[sortKey];
        let y = b[sortKey];
        return sortDirection === 'ASC' ? ((x < y) ? -1 : ((x > y) ? 1 : 0)) : ((y < x) ? -1 : ((y > x) ? 1 : 0));
      })
    })
    return sortedEntities;
  }
}
