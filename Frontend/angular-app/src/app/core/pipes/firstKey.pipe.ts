import { Pipe, PipeTransform } from '@angular/core';
@Pipe({
  name: 'firstKey'
})
export class FirstKeyPipe {
  transform(obj:any) {
    let keys = Object?.keys(obj);
    if (keys && keys?.length>0) {
      return keys[0];
    }
    return null;
  }
}
