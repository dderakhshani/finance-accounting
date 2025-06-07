import { NgModule } from '@angular/core';
import {RouterModule, Routes} from "@angular/router";
import {BaseValuesComponent} from "./pages/base-values/base-values.component";
import {UnitsComponent} from "./pages/units/units.component";
import {RolesComponent} from "./pages/roles/roles.component";
import {PermissionsComponent} from "./pages/permissions/permissions.component";
import {BaseValueTypesComponent} from "./pages/base-value-types/base-value-types.component";
import {PositionsComponent} from "./pages/positions/positions.component";
import {AddPersonComponent} from "./pages/person/add-person/add-person.component";
import {AddUserComponent} from "./pages/user/add-user/add-user.component";
import {AddEmployeeComponent} from "./pages/employee/add-employee/add-employee.component";
import {PersonListComponent} from "./pages/person/person-list/person-list.component";
import {BranchesComponent} from "./pages/branches/branches.component";
import {UserListComponent} from "./pages/user/user-list/user-list.component";
import {EmployeeListComponent} from "./pages/employee/employee-list/employee-list.component";
import {AddUserDataResolver} from "./pages/user/add-user/add-user-data-resolver";
import {AddLanguageComponent} from "./pages/language/add-language/add-language.component";
import {LangaugesListComponent} from "./pages/language/langauges-list/langauges-list.component";
import {AddCompanyComponent} from "./pages/companies/add-company/add-company.component";
import {AddYearComponent} from "./pages/year/add-year/add-year.component";
import {YearListComponent} from "./pages/year/year-list/year-list.component";
import {CompanyListComponent} from "./pages/companies/company-list/company-list.component";
import {AuditListComponent} from "./pages/audit/audit-list/audit-list.component";
import { MenuItemComponent } from './pages/menu-item/menu-item.component';
import {
  CorrectionRequestsListComponent
} from "./pages/correction-requests/correction-requests-list/correction-requests-list.component";
import {AccountManagementComponent} from "./pages/accounts/account-management/account-management.component";
import {AccountsListComponent} from "./pages/accounts/accounts-list/accounts-list.component";
import {HelpEditorComponent} from "./pages/helps/help-editor/help-editor.component";
import {HelpsListComponent} from "./pages/helps/helps-list/helps-list.component";

const routes: Routes = [
  {
    path: '',
    data: {
      title: 'Admin'
    },
    children: [
      {
        path: 'menuItem',
        component: MenuItemComponent,
        data: {
          title: 'فهرست منوها',
          id: '#menuItem',
          isTab: true
        },
      },
      {
        path: 'baseValues',
        component: BaseValuesComponent,
        data: {
          title: 'اطلاعات پایه',
          id: '#baseValues',
          isTab: true
        },
      },
      {
        path: 'audit',
        component: AuditListComponent,
        data: {
          title: 'ممیزی',
          id: '#audit',
          isTab: true
        },
      },

      {
        path: 'baseValueTypes',
        component: BaseValueTypesComponent,
        data: {
          title: 'نوع اطلاعات پایه',
          id: '#baseValueTypes',
          isTab: true
        },
      },
      {
        path: 'companies',
        children: [
          {
            path:'add',
            component: AddCompanyComponent,
            data: {
              title:'شرکت ها',
              id: '#addcompany',
              isTab: true
            }
          },
          {
            path:'list',
            component: CompanyListComponent,
            data: {
              title:'لیست شرکتها',
              id: '#companylist',
              isTab: true
            }
          }
          ]
      },

      {
        path: 'year',
        children: [
          {
            path:'add',
            component: AddYearComponent,
            data: {
              title:'اطلاعات سال مالی',
              id: '#addyear',
              isTab: true,
            }
          },
          {
            path:'list',
            component: YearListComponent,
            data: {
              title:' لیست سالهای مالی',
              id: '#yearList',
              isTab: true,
            }
          }

        ]
      },
      {
        path: 'branches',
        component: BranchesComponent,
        data: {
          title: 'شعب',
          id: '#branches',
          isTab: true
        },
      },
      {
        path: 'units',
        component: UnitsComponent,
        data: {
          title: 'واحد های سازمانی',
          id: '#units',
          isTab: true
        },
      },
      {
        path: 'positions',
        component: PositionsComponent,
        data: {
          title: 'جایگاه شغلی',
          id: '#positions',
          isTab: true
        },
      },
      {
        path: 'roles',
        component: RolesComponent,
        data: {
          title: 'نقش های سیستم',
          id: '#roles',
          isTab: true
        },
      },
      {
        path: 'permissions',
        component: PermissionsComponent,
        data: {
          title: 'دسترسی ها',
          id: '#permissions',
          isTab: true
        },
      },
      {
        path:'accountManagement',
        component: AccountManagementComponent,
        data: {
          title:' مدیریت حساب',
          id: '#accountManagement',
          isTab: true
        }
      },
      {
        path:'accountsList',
        component: AccountsListComponent,
        data: {
          title:'فهرست حساب ها',
          id: '#accountsList',
          isTab: true
        }
      },
      {
        path: 'person',
        children: [
          {
            path:'add',
            component: AddPersonComponent,
            data: {
              title:' تعریف اشخاص',
              id: '#addPerson',
              isTab: true
            }
          },
          {
            path:'list',
            component: PersonListComponent,
            data: {
              title:'فهرست اشخاص',
              id: '#personList',
              isTab: true
            }
          }
        ]
      },
      {
        path: 'user',
        children: [
          {
            path:'add',
            component: AddUserComponent,
            resolve: { blockedReason: AddUserDataResolver},
            data: {
              title:'تعریف کاربران',
              id: '#addUser',
              isTab: true
            }
          },
          {
            path:'list',
            component: UserListComponent,
            data: {
              title:'فهرست کاربران',
              id: '#userList',
              isTab: true
            }
          }
        ]
      },
      {
        path: 'employee',
        children: [
          {
            path:'add',
            component: AddEmployeeComponent,
            data: {
              title:'تعریف کارمندان',
              id: '#addEmployee',
              isTab: true
            }
          },
          {
            path:'list',
            component: EmployeeListComponent,
            data: {
              title:'فهرست کارمندان',
              id: '#employeeList',
              isTab: true
            }
          }
        ]
      },
      {
        path: 'languages',
        children: [
          {
            path:'add',
            component: AddLanguageComponent,
            data: {
              title:'افزودن زبان ها',
              id: '#addLanguage',
              isTab: true
            }
          },
          {
            path:'list',
            component: LangaugesListComponent,
            data: {
              title:'فهرست زبان ها',
              id: '#languagesList',
              isTab: true
            }
          },
          {
            path: 'employee',
            children: [
              {
                path:'add',
                component: AddEmployeeComponent,
                data: {
                  title:'کارمندان',
                  id: '#addEmployee',
                  isTab: true
                }
              },
              {
                path:'list',
                component: EmployeeListComponent,
                data: {
                  title:'لیست',
                  id: '#employeeList',
                  isTab: true
                }
              },
            ]
          }
        ]
      },
      {
        path:'correctionRequests',
        component: CorrectionRequestsListComponent,
        data: {
          title:'فهرست درخواست تغییرات',
          id: '#correctionRequests',
          isTab: true
        }
      },
      {
        path: 'helps',
        children: [
          {
            path:'add',
            component: HelpEditorComponent,
            data: {
              title:'افزودن راهنما',
              id: '#HelpEditor',
              isTab: true
            }
          },{
            path: 'list',
            component: HelpsListComponent,
            data: {
              title: 'لیست راهنما',
              id: '#HelpsList',
              isTab: true
            }
          }
        ]
      }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AdminRoutingModule { }
