import { Component, EventEmitter, Output } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { Constants } from 'src/app/constant';
import { IUser } from 'src/app/interface/user.interface';
import { SharedService } from 'src/app/service/shared/shared.service';

@Component({
  selector: 'app-registration',
  templateUrl: './registration.component.html',
  styleUrls: ['./registration.component.scss']
})
export class RegistrationComponent {
  userRegistration: IUser = {
    firstname: Constants.string.Empty,
    lastname: Constants.string.Empty,
    email: Constants.string.Empty,
    password: Constants.string.Empty,
  };
  confirmPassword: string = Constants.string.Empty
  isLoading: boolean = false;
  hasError: boolean = false;
  errorMessage: string = Constants.string.Empty;
  
  constructor(public activeModal: NgbActiveModal,
    private _sharedService: SharedService) {}
  
  register(){
    if (this.userRegistration.firstname == Constants.string.Empty
    || this.userRegistration.lastname == Constants.string.Empty 
    || this.userRegistration.email == Constants.string.Empty 
    || this.userRegistration.password == Constants.string.Empty
    || this.confirmPassword == Constants.string.Empty) {
      this.setErrors(Constants.Error.MissingInformation);
    }

    if (this.userRegistration.password != this.confirmPassword) {
      this.setErrors(Constants.Error.DifferentPasswordConfirmation);
    }
    
    if (!this.hasError) {
      this.activeModal.close(this.userRegistration);
    }
  }

  setErrors(error: string) {
    this.hasError = true;
    this.errorMessage = error;
    this._sharedService.modalMessage(error);
  }
}
