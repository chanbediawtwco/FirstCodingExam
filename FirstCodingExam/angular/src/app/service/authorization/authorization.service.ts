import { Injectable } from '@angular/core';
import { Observable, catchError, throwError } from 'rxjs';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Router } from '@angular/router';
import { IUser } from 'src/app/interface/user.interface';
import { environment } from '../../../../environments/environment';
import { ILogin } from 'src/app/interface/login.interface';
import { IToken } from 'src/app/interface/token.interface';
import { SharedService } from '../shared/shared.service';
import { RegistrationComponent } from 'src/app/component/registration/registration.component';
import { Constants } from 'src/app/constant';

@Injectable({
  providedIn: 'root'
})
export class AuthorizationService {

  constructor(private _httpClient: HttpClient, 
    public router: Router,
    private _sharedService: SharedService) { }

  registerUser() {
    const modalRef = this._sharedService._modalService.open(RegistrationComponent, { backdrop: true, size: 'lg' });
    modalRef.result.then((newUser: any) => {
      if (newUser != null) {
        let result = this.register(newUser).subscribe(response => { return response });
        this._sharedService.modalMessage(Constants.Message.RegistrationSuccessful);
      }
    });
  }

  register(user: IUser): Observable<any> {  
    return this._httpClient.post(`${environment.uri}/account/register`, user).pipe(
        catchError(this._sharedService.handleError)
    )
  }

  login(user: ILogin) {
    return this._httpClient.post<any>(`${environment.uri}/account/login`, user)
      .subscribe((res: IToken) => {
        localStorage.setItem('access_token', res.token)
        this.router.navigate(['/records']);
        if (res != null) {
          this._sharedService._modalService.dismissAll();
        }
      })
  }

  getAccessToken() {
    return localStorage.getItem('access_token');
  }

  isLoggedIn(): boolean {
    let authToken = localStorage.getItem('access_token');
    return authToken !== null;
  }

  logout() {
    if (localStorage.removeItem('access_token') == null) {
      this.router.navigate(['/home']);
    }
  }
}
