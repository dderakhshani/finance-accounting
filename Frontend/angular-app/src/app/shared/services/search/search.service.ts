import { Injectable } from '@angular/core';
import {SearchQuery} from "./models/search-query";

@Injectable({
  providedIn: 'root'
})
export class SearchService {

  constructor() { }


  createSearchQuery(propertyName: string, values: any[], comparison: string, nextOperand:string) : SearchQuery {
    return {
      propertyName: propertyName,
      values: values,
      comparison: comparison,
      nextOperand: nextOperand
    }
  }
}
