//loader.service.ts
import {
  Injectable
} from '@angular/core';
import {
  BehaviorSubject
} from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class LoaderService {

  public isLoading: { [key: string]: BehaviorSubject<boolean> } = {};
  constructor() {
    this.isLoading['0'] = new BehaviorSubject<boolean>(false)
  }

  hasLoading(keys: string[]) {
    let loading = false;

    keys.forEach(k => {
      for (let key in this.isLoading) {
        let value$ = this.isLoading[k];
        if (value$)
          if (value$.value) {
            loading = true;
            break;
          }

      }
      if (loading)
        return;
    });


    return loading;
  }
}
