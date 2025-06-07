import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {AdminRoutingModule} from "./admin-routing.module";
import {ComponentsModule} from "../../core/components/components.module";
import {FormsModule, ReactiveFormsModule} from "@angular/forms";
import {BaseValuesComponent} from "./pages/base-values/base-values.component";
import {AngularMaterialModule} from "../../core/components/material-design/angular-material.module";
import {BaseValueDialogComponent} from './pages/base-values/base-value-dialog/base-value-dialog.component';
import {UnitsComponent} from './pages/units/units.component';
import {UnitDialogComponent} from './pages/units/unit-dialog/unit-dialog.component';
import {RolesComponent} from './pages/roles/roles.component';
import {RoleDialogComponent} from './pages/roles/role-dialog/role-dialog.component';
import {PermissionsComponent} from './pages/permissions/permissions.component';
import {PermissionDialogComponent} from './pages/permissions/permission-dialog/permission-dialog.component';
import {PositionsComponent} from './pages/positions/positions.component';
import {BaseValueTypesComponent} from './pages/base-value-types/base-value-types.component';
import {
  BaseValueTypeDialogComponent
} from './pages/base-value-types/base-value-type-dialog/base-value-type-dialog.component';
import {PositionDialogComponent} from './pages/positions/position-dialog/position-dialog.component';
import {AddPersonComponent} from './pages/person/add-person/add-person.component';
import {AddUserComponent} from './pages/user/add-user/add-user.component';
import {AddEmployeeComponent} from './pages/employee/add-employee/add-employee.component';
import {PersonListComponent} from './pages/person/person-list/person-list.component';
import {BranchDialogComponent} from './pages/branches/branch-dialog/branch-dialog.component';
import {BranchesComponent} from './pages/branches/branches.component';
import {
  PersonAddressDialogComponent
} from './pages/person/add-person/person-addresses-list/person-address-dialog/person-address-dialog.component';
import {UserListComponent} from './pages/user/user-list/user-list.component';
import {EmployeeListComponent} from './pages/employee/employee-list/employee-list.component';
import {AddLanguageComponent} from './pages/language/add-language/add-language.component';
import {LangaugesListComponent} from './pages/language/langauges-list/langauges-list.component';
import {AddUserYearDialogComponent} from './pages/user/add-user/add-user-year-dialog/add-user-year-dialog.component';
import {AddCompanyComponent} from './pages/companies/add-company/add-company.component';
import {AddYearComponent} from './pages/year/add-year/add-year.component';
import {YearListComponent} from './pages/year/year-list/year-list.component';
import {CompanyListComponent} from './pages/companies/company-list/company-list.component';
import {
  CompanyYearsDialogComponent
} from './pages/companies/add-company/company-years-dialog/company-years-dialog.component';
import {CompanyYearsListComponent} from './pages/companies/add-company/company-years-list/company-years-list.component';
import {UserYearListComponent} from './pages/user/add-user/user-year-list/user-year-list.component';
import {
  PersonAddressesListComponent
} from './pages/person/add-person/person-addresses-list/person-addresses-list.component';
import { AuditListComponent } from './pages/audit/audit-list/audit-list.component';
import { AuditDialogComponent } from './pages/audit/audit-list/audit-dialog/audit-dialog.component';
import {MatIconModule} from "@angular/material/icon";
import { MenuItemComponent } from './pages/menu-item/menu-item.component';
import { MenuItemDialogComponent } from './pages/menu-item/menu-item-dialog/menu-item-dialog.component';
import { PersonBankAccountsListComponent } from './pages/person/add-person/person-bank-accounts-list/person-bank-accounts-list.component';
import { PersonBankAccountDialogComponent } from './pages/person/add-person/person-bank-accounts-list/person-bank-account-dialog/person-bank-account-dialog.component';
import { PersonPhonesListComponent } from './pages/person/add-person/person-phones-list/person-phones-list.component';
import { PersonPhoneDialogComponent } from './pages/person/add-person/person-phones-list/person-phone-dialog/person-phone-dialog.component';
import { PersonCustomerComponent } from './pages/person/add-person/person-customer/person-customer.component';
import { PersonCustomerDialogComponent } from './pages/person/add-person/person-customer/person-customer-dialog/person-customer-dialog.component';
import { CorrectionRequestsListComponent } from './pages/correction-requests/correction-requests-list/correction-requests-list.component';
import { AccountManagementComponent } from './pages/accounts/account-management/account-management.component';
import { AccountsListComponent } from './pages/accounts/accounts-list/accounts-list.component';

import { MenuItemRolesListComponent } from './pages/menu-item/menu-item-roles-list/menu-item-roles-list.component';
import { HelpComponent } from './components/help/help.component';
import { HelpEditorComponent } from './pages/helps/help-editor/help-editor.component';
import { HelpsListComponent } from './pages/helps/helps-list/helps-list.component';
import { HelpSidebarComponent } from './components/help/help-sidebar/help-sidebar.component';
import { HelpSidebarItemComponent } from './components/help/help-sidebar/help-sidebar-item/help-sidebar-item.component';
import { CKEditorModule } from '@ckeditor/ckeditor5-angular';
import {PersonEmployeeComponent} from "./pages/person/add-person/person-employee/person-employee.component";
import { VerifierRequestsListComponent } from './pages/correction-requests/correction-requests-list/verifier-requests-list/verifier-requests-list.component';
import { CentralBankReportDialogComponent } from './pages/person/person-list/central-bank-report-dialog/central-bank-report-dialog.component';

@NgModule({
    declarations: [
        BaseValuesComponent,
        BaseValueDialogComponent,
        UnitsComponent,
        UnitDialogComponent,
        RolesComponent,
        RoleDialogComponent,
        PermissionsComponent,
        PermissionDialogComponent,
        PositionsComponent,
        BaseValueTypesComponent,
        BaseValueTypeDialogComponent,
        PositionDialogComponent,
        AddPersonComponent,
        AddUserComponent,
        AddEmployeeComponent,
        PersonListComponent,
        BranchDialogComponent,
        BranchesComponent,
        PersonAddressDialogComponent,
        UserListComponent,
        EmployeeListComponent,
        AddLanguageComponent,
        LangaugesListComponent,
        AddUserYearDialogComponent,
        AddCompanyComponent,
        AddYearComponent,
        YearListComponent,
        CompanyListComponent,
        CompanyYearsDialogComponent,
        CompanyYearsListComponent,
        UserYearListComponent,
        PersonAddressesListComponent,
        AuditListComponent,
        AuditDialogComponent,
        MenuItemComponent,
        MenuItemDialogComponent,
        PersonBankAccountsListComponent,
        PersonBankAccountDialogComponent,
        PersonPhonesListComponent,
        PersonPhoneDialogComponent,
        PersonCustomerComponent,
        PersonCustomerDialogComponent,
        CorrectionRequestsListComponent,
        AccountManagementComponent,
        AccountsListComponent,
        MenuItemRolesListComponent,
        HelpComponent,
        HelpEditorComponent,
        HelpsListComponent,
        HelpSidebarComponent,
        HelpSidebarItemComponent,
        PersonEmployeeComponent,
        VerifierRequestsListComponent,
        CentralBankReportDialogComponent,

    ],
    exports: [
        PersonAddressesListComponent,
        PersonPhonesListComponent,
        PersonBankAccountsListComponent,
        PersonBankAccountsListComponent,
        PersonBankAccountsListComponent,
        BaseValueTypeDialogComponent
    ],
  imports: [
    CommonModule,
    AdminRoutingModule,
    ComponentsModule,
    ReactiveFormsModule,
    AngularMaterialModule,
    MatIconModule,
    CKEditorModule,
    FormsModule,
  ]
})
export class AdminModule {
}
