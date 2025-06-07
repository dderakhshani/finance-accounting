import { Component, Inject } from '@angular/core';

import { MAT_DIALOG_DATA, MatDialogRef } from "@angular/material/dialog";

import { BaseComponent } from "../../../../../core/abstraction/base.component";
import { PageModes } from "../../../../../core/enums/page-modes";
import { Mediator } from "../../../../../core/services/mediator/mediator.service";


import { Commodity } from '../../../../commodity/entities/commodity';

import { Prehension } from '../../../entities/prehension';
import { GetPrehensionGroupByCodeQuery } from '../../../repositories/prehension/queries/get-all-prehension-group-query';
import { CreateMapSamatozinToDanaCommand } from '../../../repositories/map-samatozin-to-dana/commands/create-map-samatozin-to-dana-command';
import { UpdateMapSamatozinToDanaCommand } from '../../../repositories/map-samatozin-to-dana/commands/update-map-samatozin-to-dana-command';
import { DeleteMapSamatozinToDanaCommand } from '../../../repositories/map-samatozin-to-dana/commands/delete-map-samatozin-to-dana-command';
import { Account, MapSamatozinToDana } from '../../../entities/map-samatozin-to-dana';


@Component({
  selector: 'app-samatozin-to-dana-dialog',
  templateUrl: './samatozin-to-dana-dialog.component.html',
  styleUrls: ['./samatozin-to-dana-dialog.component.scss'],

})

export class MapSamatozinToDanaDialogComponent extends BaseComponent {

  pageModes = PageModes;
  MapSamatozinToDana!: MapSamatozinToDana;
  prehensions: Account[] = [];


  constructor(
    private _mediator: Mediator,
    private dialogRef: MatDialogRef<MapSamatozinToDanaDialogComponent>,
    @Inject(MAT_DIALOG_DATA) data: any
  ) {
    super();

    this.MapSamatozinToDana = data.MapSamatozinToDana;
    this.pageMode = data.pageMode;
    this.request = new CreateMapSamatozinToDanaCommand();

  }


  async ngOnInit() {
    await this.resolve();
  }

  async resolve() {

    await this.PrehensionsFilter();
    await this.initialize()

  }

  async initialize() {


    if (this.pageMode === PageModes.Add) {
      let newRequest = new CreateMapSamatozinToDanaCommand()

      this.request = newRequest;
    }
    if (this.pageMode === PageModes.Update) {
      this.request = new UpdateMapSamatozinToDanaCommand().mapFrom(this.MapSamatozinToDana)
    }


  }
  async PrehensionsFilter() {

    await this._mediator.send(new GetPrehensionGroupByCodeQuery(0, 0, undefined)).then(res => {

      res.data.forEach(a => {
        this.prehensions.push({title:a})
      });
      
    })

  }

  async add(){

    await this._mediator.send(<CreateMapSamatozinToDanaCommand>this.request).then(res => {

      this.dialogRef.close({

        response: res,
        pageMode: this.pageMode
      })

    });
  }

  async update(entity?: any) {
    await this._mediator.send(<UpdateMapSamatozinToDanaCommand>this.request).then(res => {
      this.dialogRef.close({
        response: res,
        pageMode: this.pageMode
      })
    });
  }

  async delete() {
    await this._mediator.send(new DeleteMapSamatozinToDanaCommand((<UpdateMapSamatozinToDanaCommand>this.request).id ?? 0)).then(res => {

      this.dialogRef.close({
        response: res,
        pageMode: PageModes.Delete
      })

    });
  }

  getPrehensionById(item: any) {


    this.form.controls.samaTozinCode.setValue(item?.title);
    this.form.controls.samaTozinTitle.setValue(item?.title);
    
  }
  referenceSelect(item: any) {


    this.form.controls.accountReferenceId.setValue(item?.id);
    this.form.controls.accountReferenceCode.setValue(item?.code);
  }

  get(id?: number) {
    throw new Error('Method not implemented.');
  }

  close() {
    throw new Error('Method not implemented.');
  }

}

