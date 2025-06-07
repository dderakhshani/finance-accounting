import {Pipe, PipeTransform} from '@angular/core';

@Pipe({
  name: 'arrayFilter'
})
export class ArrayFilterPipe implements PipeTransform {
  private shownItemsCount: number = 50;
  transform(items: any[], searchText: string, searchableProperties: string[] = []): any[] {
    if (!items) return [];
    if (!searchText) return items.slice(0,this.shownItemsCount);
    if(!(searchableProperties?.length > 0)) return items;

    // TODO: need to improve this filter
    return items.filter(item => {
      let isConditionMet = false;
      searchableProperties.forEach(x => {
        if(!isConditionMet && item[x]?.toString()?.toLowerCase().includes(searchText?.toString()?.toLowerCase())) isConditionMet = true;
      })
      return isConditionMet;
    })?.slice(0,this.shownItemsCount);
  }

}
