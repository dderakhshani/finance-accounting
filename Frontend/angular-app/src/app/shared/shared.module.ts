import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatIconModule } from '@angular/material/icon';
import { PermissionDirective } from './directives/permission.directive';


@NgModule({
    declarations: [
        PermissionDirective
    ],
    exports: [
    ],
    imports: [
        CommonModule,
        MatIconModule
    ]
})
export class SharedModule { }
