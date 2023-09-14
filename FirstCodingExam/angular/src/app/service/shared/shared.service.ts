import { HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { throwError } from 'rxjs';
import { MessageConfirmationModal } from 'src/app/modal/message.confirmation.modal';
import { MessageModal } from 'src/app/modal/message.modal';

@Injectable({
  providedIn: 'root'
})
export class SharedService {

  constructor(public _modalService: NgbModal) { }

  modalMessage(message: string) {
		const modalRef = this._modalService.open(MessageModal, { backdrop: true, size: 'md' });
		modalRef.componentInstance.message = message;
	}

  handleError(error: HttpErrorResponse) {
    let message = '';
    if (error.error instanceof ErrorEvent) {
      // client-side error
      message = `Error: ${error.error.message}`;
    } else {
      // server-side error
      message = `Error Code: ${error.status}\nMessage: ${error.message}`;
    }
    return throwError(message);
  }
}
