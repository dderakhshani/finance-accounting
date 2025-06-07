import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ArchiveRoutingModule } from './archive-routing.module';
import { ArchivesListComponent } from './pages/archives-list/archives-list.component';
import {ComponentsModule} from "../../core/components/components.module";
import {MatCardModule} from "@angular/material/card";
import {MatMenuModule} from "@angular/material/menu";
import {AngularMaterialModule} from "../../core/components/material-design/angular-material.module";
import {AddArchiveDialogComponent} from "./pages/dialog/add-archive-dialog/add-archive-dialog.component";
import {FormsModule, ReactiveFormsModule} from "@angular/forms";
import {AddAttachmentDialogComponent} from "./pages/dialog/add-attachment-dialog/add-attachment-dialog.component";
import { SearchAttachmentsComponent } from './pages/search-attachments/search-attachments.component';

@NgModule({
  declarations: [
    ArchivesListComponent,
    AddArchiveDialogComponent,
    AddAttachmentDialogComponent,
    SearchAttachmentsComponent
  ],
  imports: [
    CommonModule,
    ArchiveRoutingModule,
    ComponentsModule,
    MatCardModule,
    MatMenuModule,
    AngularMaterialModule,
    FormsModule,
    ReactiveFormsModule
  ]
})
export class ArchiveModule { }
