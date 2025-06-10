import {Component, EventEmitter, Input, OnChanges, Output} from '@angular/core';

@Component({
  selector: 'app-content',
  templateUrl: './content.component.html',
  styleUrls: ['./content.component.scss']
})
export class ContentComponent implements OnChanges{

  @Input() tabManagerService:any

  ngOnChanges(changes: any): void {
    if (changes['test']) {
      console.log("Updated test:", this.tabManagerService);
    }
  }

}
