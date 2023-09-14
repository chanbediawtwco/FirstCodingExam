import { Component, Input } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
	selector: 'ngbd-modal-content',
	standalone: true,
	template: `
		<div class="modal-dialog modal-dialog-centered">
			<div class="modal-content">
				<div class="modal-header">
					<button type="button" class="btn-close" aria-label="Close" (click)="activeModal.dismiss('Cross click')"></button>
				</div>
				<div class="modal-body">
					<p>{{ message }}</p>
				</div>
				<div class="modal-footer">
					<button type="button" class="btn btn-outline-primary" (click)="activeModal.close('Close click')">Close</button>
				</div>
			<div>
		</div>
	`,
})

export class MessageModal {
	@Input() message: any;

	constructor(public activeModal: NgbActiveModal) {}
}
