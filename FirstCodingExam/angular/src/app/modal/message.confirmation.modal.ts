import { Component, EventEmitter, Input, Output } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
	selector: 'ngbd-modal-content',
	standalone: true,
	template: `
		<div class="modal-dialog modal-dialog-centered">
			<div class="modal-content">
				<div class="modal-header">
					<h5 class="modal-title" id="confirmationModalLabel">Confirmation</h5>
					<button type="button" class="btn-close" aria-label="Close" (click)="activeModal.dismiss('Cross click')"></button>
				</div>
				<div class="modal-body">
					<p>{{ message }}</p>
				</div>
				<div class="modal-footer">
					<button type="button" class="btn btn-secondary" (click)="activeModal.close('CLOSED')">Close</button>
					<button type="button" class="btn btn-primary" (click)="activeModal.close('CONFIRMED')">Confirm</button>
				</div>
			<div>
		</div>
	`,
})

export class MessageConfirmationModal {
	@Input() message: any;

	constructor(public activeModal: NgbActiveModal) {}
}
