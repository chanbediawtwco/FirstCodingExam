import { Injectable } from '@angular/core';
import { AuthorizationService } from './authorization.service';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot } from '@angular/router';
import { Observable } from 'rxjs';
import { Constants } from 'src/app/constant';

@Injectable({
  providedIn: 'root'
})
export class AuthorizationGuardService implements CanActivate {

  constructor(
    public _authorizationService : AuthorizationService,
    public router: Router) { }

  canActivate(next: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean> | Promise<boolean> | boolean
    {
      if (this._authorizationService.isLoggedIn() !== true) {
        window.alert(Constants.Error.AccessNotAllowed);
        this.router.navigate(['/home'])
      }
      return true;
    }
}
