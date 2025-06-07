import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import {AuthenticationComponent} from "./pages/authentication/authentication.component";


const routes: Routes = [
  {
    path: '',
    component: AuthenticationComponent,
    data: {
      title: 'لاگین',
      id: '#Login',
    },
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class IdentityRoutingModule { }
