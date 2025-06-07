import {Component} from '@angular/core';
import {BaseComponent} from "../../../../../core/abstraction/base.component";
import {Mediator} from "../../../../../core/services/mediator/mediator.service";
import {CorrectionRequest} from "../../../entities/correction-request";
import {ActivatedRoute, Router} from "@angular/router";

import {IdentityService} from "../../../../identity/repositories/identity.service";


@Component({
  selector: 'app-correction-requests-list',
  templateUrl: './correction-requests-list.component.html',
  styleUrls: ['./correction-requests-list.component.scss']
})
export class CorrectionRequestsListComponent extends BaseComponent {
  selectedTabIndex: number = 0;
  createdRecords: CorrectionRequest[] = [];

  userId!: number;


  constructor(
    private mediator: Mediator,
    private identityService: IdentityService,
    private route: ActivatedRoute,
    private router: Router
  ) {
    super();
  }

  ngAfterViewInit() {

  }

  async ngOnInit() {

  }

  async getUser() {
    this.identityService._applicationUser.subscribe(res => {
      if (res.isAuthenticated) {
        this.userId = res.id;
      }
    });
  }


 resolve(params?: any): any {
 }

  get() {

  }

  add(param?: any): any {
  }

  close(): any {
  }

  delete(param?: any): any {
  }


  initialize(params?: any): any {
  }

  update(param?: any): any {
  }

}
