import {APP_INITIALIZER, NgModule} from '@angular/core';
import { CommonModule } from '@angular/common';
import {IdentityRoutingModule} from "./identity-routing.module";
import {ReactiveFormsModule} from "@angular/forms";
import {ComponentsModule} from "../../core/components/components.module";
import {AuthenticationComponent} from "./pages/authentication/authentication.component";
import {AngularMaterialModule} from "../../core/components/material-design/angular-material.module";
import {AccountManagerService} from "../accounting/services/account-manager.service";
import {IdentityService} from "./repositories/identity.service";



@NgModule({
  declarations: [
    AuthenticationComponent
  ],
  imports: [
    CommonModule,
    IdentityRoutingModule,
    ReactiveFormsModule,
    ComponentsModule,
    AngularMaterialModule
  ],
  providers: [

  ]
})
export class IdentityModule { }
