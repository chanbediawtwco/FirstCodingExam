import { Component } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { LoginComponent } from '../login/login.component';
import { AuthorizationService } from 'src/app/service/authorization/authorization.service';
import { Constants } from 'src/app/constant';

@Component({
  selector: 'app-navigation',
  templateUrl: './navigation.component.html',
  styleUrls: ['./navigation.component.scss']
})
export class NavigationComponent {
  constructor(private _modalService: NgbModal,
    private _authorizationService: AuthorizationService) {}
  
  isLoggedIn: Boolean = false;

  ngOnInit() {
    this.isLoggedIn = this._authorizationService.isLoggedIn();
  }
  
  register() {
    this._authorizationService.registerUser();
  }

  login() {
		this._modalService.open(LoginComponent, { backdrop: true, size: Constants.Modal.Medium });
	}
  
  logout() {
    this._authorizationService.logout();
    this.isLoggedIn = this._authorizationService.isLoggedIn();
  }
}
