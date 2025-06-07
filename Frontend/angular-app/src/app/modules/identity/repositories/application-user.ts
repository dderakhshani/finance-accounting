import {Person} from "../../admin/entities/person";
import {NavigationItem} from "../../../layouts/main-container/models/navigation-item";
import {UserRole} from "./models/user-role";
import {UserYear} from "./models/user-year";
import {ServiceLocator} from "../../../core/services/service-locator/service-locator";
import {IdentityService} from "./identity.service";
import {ExtensionsService} from "../../../shared/services/extensions/extensions.service";
import {Permission} from "../../admin/entities/permission";
import {UserPermission} from "./models/user-permission";
import {UserCompany} from "./models/user-company";
import {UserMenu} from "./models/user-menu";

export class ApplicationUser {
  public isAuthenticated: boolean = false;
  public id!: number;
  public personId!: number;
  public username!: string;
  public password!: string;
  public isBlocked!: boolean;
  public blockedReason!: string;
  public person!: Person;
  public fullName!: string;
  public firstName!: string;
  public lastName!: string;
  public nationalNumber!: string;
  public flatNavigations: NavigationItem[] = [];
  public photoUrl!: string;
  public userAvatarReletiveAddress!: string;
  public yearId!: number;
  public companyId!: number;
  public roleId!: number;
  public userRoleName!: string;
  public languageId!: number;
  public accountReferenceId!: number;

  public roles: UserRole[] = [];
  public years: UserYear[] = [];
  public companies!: UserCompany[];
  public permissions!: UserPermission[];
  public menus!: UserMenu[];

  public token!: string

  public get navigations() {
    let extensionService = ServiceLocator.injector.get(ExtensionsService);
    return extensionService.listToTree(this.flatNavigations, 'children', 'id', 'parentId')
  }

  public set navigations(navItems: NavigationItem[]) {
    this.flatNavigations = navItems;
  }
}
