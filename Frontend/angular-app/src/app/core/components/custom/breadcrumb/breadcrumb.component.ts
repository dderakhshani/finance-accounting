import {Component, EventEmitter, Input, OnChanges, OnInit, Output, SimpleChanges} from '@angular/core';

@Component({
  selector: 'app-breadcrumb',
  templateUrl: './breadcrumb.component.html',
  styleUrls: ['./breadcrumb.component.scss']
})
export class BreadcrumbComponent implements OnInit,OnChanges {

  @Input() nodes: any[] = [];
  @Input() disabled: boolean = true;
  @Output() nodeSelected = new EventEmitter<any>();
  @Output() pathEvent = new EventEmitter<any>();
  breadcrumb: any[] = [];

  constructor() {
  }

  ngOnChanges(changes: SimpleChanges) {
    if (changes['nodes']) {
      if (this.nodes && this.nodes.length > 0) {
        // پیدا کردن نود ریشه توسعهیافته یا استفاده از اولین نود
        const rootNode = this.nodes.find(node => node._expanded) || this.nodes[0];
        this.breadcrumb = this.buildBreadcrumb(rootNode);
        this.pathEvent.emit( this.breadcrumb);
      }
    }
  }

  ngOnInit(): void {

  }

  buildBreadcrumb(node: any): any[] {
    let path = [{ title: node.title ,typeBaseId : node.id ,groupName: node.groupName, node }];

    if (node.children && node.children.length > 0) {
      const expandedChild = node.children.find((child: any) => child._expanded);
      if (expandedChild) {
        path = path.concat(this.buildBreadcrumb(expandedChild));
      }
    }

    return path;
  }


  onCrumbClick(crumb: any): void {
    this.nodeSelected.emit(crumb.node);
  }


}
