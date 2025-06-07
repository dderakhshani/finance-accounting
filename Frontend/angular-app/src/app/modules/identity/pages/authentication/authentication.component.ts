import { Component, OnInit } from '@angular/core';
import {AbstractControl, FormControl, FormGroup, ValidationErrors, ValidatorFn, Validators} from "@angular/forms";
import {Router} from "@angular/router";
import {IdentityService} from "../../repositories/identity.service";
import {LoginDTO} from "../../repositories/dto/login-dto";

@Component({
  selector: 'app-authentication',
  templateUrl: './authentication.component.html',
  styleUrls: ['./authentication.component.scss']
})
export class AuthenticationComponent implements OnInit {

  isLoading = false;
  loginForm = new FormGroup({
    username: new FormControl('', [ forbiddenNameValidator(/bob/i)]),
    password: new FormControl(),
  });
  constructor(
    private identityService: IdentityService,
    private router: Router
  ) {
  }

  ngOnInit(): void {
  }

  async login() {
    this.isLoading = true;
    let dto : LoginDTO = new LoginDTO();
    dto.username = this.loginForm.get('username')?.value;
    dto.password = this.loginForm.get('password')?.value;

    await this.identityService.login(dto).toPromise().then(async(res) => {
        await this.identityService.setApplicationUserFromToken(res.objResult).then(() => {
          this.router.navigateByUrl("/")
        });
    }).catch(() => {
      this.isLoading = false;
    });
  }

}


export function forbiddenNameValidator(nameRe: RegExp): ValidatorFn {
  return (control: AbstractControl): ValidationErrors | null => {
    const forbidden = nameRe.test(control.value);
    return forbidden ? {forbiddenName: {value: control.value}} : null;
  };
}
