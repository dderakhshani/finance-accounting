import {UserRole} from "./user-role";
import {UserYear} from "./user-year";
import {UserMenu} from "./user-menu";
import {UserPermission} from "./user-permission";
import {UserCompany} from "./user-company";

export class UserProfile {
  public id!: number;
  public username!: string;
  public fullName!: string;
  public roles!: UserRole[];
  public years!: UserYear[];
  public menus!: UserMenu[];
  public companies!: UserCompany[];
  public permissions!: UserPermission[];
}
