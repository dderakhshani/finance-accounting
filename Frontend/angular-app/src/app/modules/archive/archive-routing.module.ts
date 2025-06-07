import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import {MenuItemComponent} from "../admin/pages/menu-item/menu-item.component";
import {ArchivesListComponent} from "./pages/archives-list/archives-list.component";
import {SearchAttachmentsComponent} from "./pages/search-attachments/search-attachments.component";

const routes: Routes = [
  {
    path: '',
    data: {
      title: 'archive'
    },
    children: [
      {
        path: 'archiveList',
        component: ArchivesListComponent,
        data: {
          title: 'بایگانی',
          id: '#archiveList',
          isTab: true
        },
      },
      {
        path: 'searchAttachments',
        component: SearchAttachmentsComponent,
        data: {
          title: 'جستجوی فایل',
          id: '#searchAttachments',
          isTab: true
        },
      },
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ArchiveRoutingModule {
}
