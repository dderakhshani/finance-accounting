import {Component,} from '@angular/core';
import {Employee} from "../../../entities/employee";
import {ActivatedRoute, Router} from "@angular/router";
import {Person} from "../../../entities/person";
import {Branch} from "../../../entities/branch";
import {Unit} from "../../../entities/unit";
import {UnitPosition} from "../../../entities/unit-position";
import {FormAction} from "../../../../../core/models/form-action";
import {FormActionTypes} from "../../../../../core/constants/form-action-types";
import {BaseComponent} from "../../../../../core/abstraction/base.component";
import {Mediator} from "../../../../../core/services/mediator/mediator.service";
import {CreateEmployeeCommand} from "../../../repositories/employee/commands/create-employee-command";
import {PageModes} from "../../../../../core/enums/page-modes";
import {UpdateEmployeeCommand} from "../../../repositories/employee/commands/update-employee-command";
import {GetEmployeeQuery} from "../../../repositories/employee/queries/get-employee-query";
import {forkJoin} from "rxjs";
import {GetUnitPositionsQuery} from "../../../repositories/unit-positions/queries/get-unit-positions-query";
import {GetUnitsQuery} from "../../../repositories/unit/queries/get-units-query";
import {GetBranchesQuery} from "../../../repositories/branch/queries/get-branches-query";
import {GetPersonsQuery} from "../../../repositories/person/queries/get-persons-query";
import {PreDefinedActions} from "../../../../../core/components/custom/action-bar/action-bar.component";
import {GetPositionsQuery} from "../../../repositories/position/queries/get-positions-command";
import {Position} from "../../../entities/position";


@Component({
  selector: 'app-add-employee',
  templateUrl: './add-employee.component.html',
  styleUrls: ['./add-employee.component.scss']
})
export class AddEmployeeComponent extends BaseComponent {

  persons: Person[] = [];

  branches: Branch[] = [];
  units: Unit[] = [];
  positions: Position[] = [];

  filteredBranches: Branch[] = [];
  filteredUnits: Unit[] = [];
  filteredUnitPositions: Position[] = [];



  constructor(
    private _mediator: Mediator,
    private route: ActivatedRoute,
    private router: Router) {
    super(route, router);
    let newRequest = new CreateEmployeeCommand();
    newRequest.employmentDate = new Date();
    this.request = newRequest;
  }
  ngAfterViewInit() {
    this.actionBar.actions=[
      PreDefinedActions.save(),
      PreDefinedActions.add(),
      PreDefinedActions.list(),
      PreDefinedActions.delete(),
    ]
  }
  async ngOnInit() {
    await this.resolve()


  }

  async resolve() {
    forkJoin([
      this._mediator.send(new GetBranchesQuery()),
      this._mediator.send(new GetUnitsQuery()),
      this._mediator.send(new GetPositionsQuery()),
      this._mediator.send(new GetPersonsQuery())
    ]).subscribe(async ([
                          branchResponse,
                          unitsResponse,
                          positions,
                          persons
                        ]) => {
      this.branches = branchResponse.data;
      this.filteredBranches = this.branches;

      this.units = unitsResponse.data;
      this.filteredUnits = this.units;

      this.positions = positions.data;
      this.filteredUnitPositions=this.positions;


      this.persons = persons.data;
      await this.initialize()
    })
  }

  async initialize(entity?: Employee) {
    if (entity || this.getQueryParam('id')) {
      this.pageMode = PageModes.Update;
      if (!entity) entity = await this.get(this.getQueryParam('id'));
      this.request = new UpdateEmployeeCommand().mapFrom(entity);
    } else {
      this.form.controls['unitId']?.disable()
      this.form.controls['unitPositionId']?.disable()
    }
    this.form.controls['accountReferenceCode']?.disable()
  }


  async get(id: number) {
    return await this._mediator.send(new GetEmployeeQuery(id))
  }

  async add() {
    let response = await this._mediator.send(<CreateEmployeeCommand>this.request)
    await this.initialize(response)
  }

  async update() {
    let response = await this._mediator.send(<UpdateEmployeeCommand>this.request)
    await this.initialize(response)
  }

  async navigateToEmployeeList() {
    await this.router.navigateByUrl('/admin/employee/list')
  }

  close(): any {
  }

  delete(): any {

  }

  async reset() {
    await this.deleteQueryParam('id');
    return super.reset();
  }


  getPersonNameById(id: number) {
    let person = this.persons?.find(x => x.id === id)
    return person && id ? [person.firstName, person.lastName].join(' ') : ''
  }


  handleEmployeeUnitPositionAndUnitAndBranchSelection() {
    let branch = this.branches.find(x => x.id === this.form.controls['branchId']?.value);
    let unit = this.units.find(x => x.id === this.form.controls['unitId']?.value);
    let unitPosition = this.positions.find(x => x.id === this.form.controls['unitPositionId']?.value);

    let isBranchConnectedToUnit = unit?.branchId === branch?.id;
    let isUnitConnectedToUnitPosition = unitPosition ? unit?.positionIds.includes(unitPosition?.id) : false;

    if (branch && unit && !isBranchConnectedToUnit) {
      this.filteredUnits = this.units.filter(x => x.branchId === branch?.id)
      this.form.controls['unitId'].reset();

      this.filteredUnitPositions = []
      this.form.controls['unitPositionId'].reset();
      this.form.controls['unitPositionId'].disable();
    }

    if (branch && !unit) {
      this.filteredUnits = this.units.filter(x => x.branchId === branch?.id)
      this.form.controls['unitId'].enable();
    }

    if (branch && unit && (!unitPosition || !isUnitConnectedToUnitPosition)) {
      this.filteredUnitPositions = this.positions.filter(x => unit?.positionIds.includes(x.id))
      this.form.controls['unitPositionId'].reset();
      this.form.controls['unitPositionId'].enable();

    }
  }

}
