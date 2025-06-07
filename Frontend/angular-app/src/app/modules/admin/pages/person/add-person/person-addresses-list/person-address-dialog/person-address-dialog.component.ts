import {Component, Inject} from '@angular/core';
import {MAT_DIALOG_DATA, MatDialogRef} from "@angular/material/dialog";
import {CountryDivision} from "../../../../../entities/countryDivision";
import {BaseValue} from "../../../../../entities/base-value";
import {BaseComponent} from "../../../../../../../core/abstraction/base.component";
import {forkJoin} from "rxjs";
import {
  GetBaseValuesByUniqueNameQuery
} from "../../../../../repositories/base-value/queries/get-base-values-by-unique-name-query";
import {
  GetCountryDivisionsQuery
} from "../../../../../repositories/country-division/queries/get-country-divisions-query";
import {Mediator} from "../../../../../../../core/services/mediator/mediator.service";
import {PageModes} from "../../../../../../../core/enums/page-modes";
import {
  CreatePersonAddressCommand
} from "../../../../../repositories/person-address/commands/create-person-address-command";
import {
  UpdatePersonAddressCommand
} from "../../../../../repositories/person-address/commands/update-person-address-command";

@Component({
  selector: 'app-person-address-dialog',
  templateUrl: './person-address-dialog.component.html',
  styleUrls: ['./person-address-dialog.component.scss']
})
export class PersonAddressDialogComponent extends BaseComponent {
  addressTypes: BaseValue[] = []
  countryDivisions: CountryDivision[] = []

  pageModes = PageModes;
  data: any;

  constructor(
    private _mediator: Mediator,
    public dialogRef: MatDialogRef<PersonAddressDialogComponent>,
    @Inject(MAT_DIALOG_DATA) data: any
  ) {
    super();
    this.pageMode = data.pageMode;
    this.data = data;
    this.request = new CreatePersonAddressCommand();
  }

  async ngOnInit() {
    await this.resolve()
  }

  async resolve() {
    forkJoin([
      this._mediator.send(new GetBaseValuesByUniqueNameQuery('AddressTitle')),
      this._mediator.send(new GetCountryDivisionsQuery())
    ]).subscribe(([
                    addressTypes,
                    countryDivisions
                  ]) => {
      this.addressTypes = addressTypes;
      this.countryDivisions = countryDivisions;
      this.initialize()
    })
  }

  initialize() {
    if (this.pageMode === PageModes.Add) {
      let newRequest = new CreatePersonAddressCommand();
      newRequest.personId = this.data.personId;
      this.request = newRequest;
    } else if (this.pageMode === PageModes.Update) {
      this.request = new UpdatePersonAddressCommand().mapFrom(this.data.personAddress)
    }
  }

  async add(param?: any) {
    let response = await this._mediator.send(<CreatePersonAddressCommand>this.request);
    this.dialogRef.close(response)
  }


  async update(param?: any) {
    let response = await this._mediator.send(<UpdatePersonAddressCommand>this.request);
    this.dialogRef.close(response)
  }

  getCountryDivisionTitle(countryDivisionId: number) {
    let countryDivision = this.countryDivisions.find(x => x.id === countryDivisionId)
    return countryDivision ? [countryDivision.ostanTitle, countryDivision.shahrestanTitle].join(' ، ') : '';
  }

  get(param?: any) {
  }

  delete(param?: any) {
  }

  close() {
  }
}
