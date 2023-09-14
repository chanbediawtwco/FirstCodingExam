import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthorizationService } from 'src/app/service/authorization/authorization.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent {
  constructor(private _authorizationService: AuthorizationService,
    private router: Router) {}

  ngOnInit() {
    this.redirectAuthenticatedUser();
  }

  redirectAuthenticatedUser(){
    if (this._authorizationService.isLoggedIn()) {
      this.router.navigate(['/records']);
    }
  }
}
