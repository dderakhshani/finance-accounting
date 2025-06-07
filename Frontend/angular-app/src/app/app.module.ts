import {Injector, NgModule, RendererStyleFlags2} from '@angular/core';
import {BrowserModule} from '@angular/platform-browser';

import {AppRoutingModule} from './app-routing.module';
import {AppComponent} from './app.component';

import {HTTP_INTERCEPTORS, HttpClientModule} from "@angular/common/http";
import {LayoutModule} from "./layouts/layout.module";
import {AngularMaterialModule} from "./core/components/material-design/angular-material.module";
import {BrowserAnimationsModule} from "@angular/platform-browser/animations";
import {GlobalHttpInterceptorService} from "./core/interceptors/global-http-interceptor.service";
import {CommonModule, DecimalPipe} from "@angular/common";
import {MAT_SNACK_BAR_DEFAULT_OPTIONS} from "@angular/material/snack-bar";
import {SharedModule} from "./shared/shared.module";
import {CoreModule} from "./core/core.module";
import {ServiceLocator} from "./core/services/service-locator/service-locator";
import {LoaderInterceptor} from './core/interceptors/loader.interceptor';
import {LoaderService} from './core/services/loader.service';
import {IdentityService} from "./modules/identity/repositories/identity.service";
import {Renderer} from "@angular/compiler-cli/ngcc/src/rendering/renderer";

@NgModule({
  declarations: [
    AppComponent,
  ],
  imports: [
    CommonModule,
    BrowserModule,
    BrowserAnimationsModule,
    HttpClientModule,
    AngularMaterialModule,

    AppRoutingModule,

    CoreModule,
    LayoutModule,
    SharedModule
  ],
  providers: [
    LoaderService,
    DecimalPipe,
    {provide: HTTP_INTERCEPTORS, useClass: GlobalHttpInterceptorService, multi: true},
    {provide: HTTP_INTERCEPTORS, useClass: LoaderInterceptor, multi: true},
    {provide: MAT_SNACK_BAR_DEFAULT_OPTIONS, useValue: {duration: 5000, horizontalPosition: "left", verticalPosition: "bottom"}}
  ],
  bootstrap: [AppComponent],
})


export class AppModule {
  constructor(private injector: Injector,private identityService: IdentityService) {
    ServiceLocator.injector = injector;
    this.identityService.init()

  }
}



