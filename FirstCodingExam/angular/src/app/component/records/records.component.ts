import { Component } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Constants } from 'src/app/constant';
import { IRecord } from 'src/app/interface/record.interface';
import { MessageConfirmationModal } from 'src/app/modal/message.confirmation.modal';
import { RecordService } from 'src/app/service/record/record.service';
import { SharedService } from 'src/app/service/shared/shared.service';

@Component({
  selector: 'app-records',
  templateUrl: './records.component.html',
  styleUrls: ['./records.component.scss']
})
export class RecordsComponent {
  constructor(private _recordService: RecordService,
    private _sharedService: SharedService,
    public _modalService: NgbModal) {}
  
  isLoading: boolean = false;
  hasError: boolean = false;

  newRecord: IRecord = {};

  records: any;
  edittedRecord?: IRecord;

  ngOnInit(){
    this.initializeInputValues(this.newRecord);
    this._recordService.getRecords()
    .subscribe(response => { this.records = response; });
  }

  async saveNewRecord() {
    let amount: number = Number(this.newRecord.amount);
    let lowerBoundInterestRate: number = Number(this.newRecord.lowerBoundInterestRate);
    let upperBoundInterestRate: number = Number(this.newRecord.upperBoundInterestRate);
    let incrementalRate: number = Number(this.newRecord.incrementalRate);
    let maturityYears: number = Number(this.newRecord.maturityYears);

    this.hasError = isNaN(amount)
        || isNaN(lowerBoundInterestRate)
        || isNaN(upperBoundInterestRate)
        || isNaN(incrementalRate)
        || isNaN(maturityYears);
        
    // Proceed with saving if there are no errors found
    if (!this.hasError) {
      // Save the changes if the record has no error and is not from records
      if (this.edittedRecord == undefined) {
        await this._recordService.addRecord(this.newRecord);
        window.location.reload();
      }

      // Save the changes if the record has no error and is from records
      if (this.edittedRecord != undefined) {
        this.edittedRecord.amount = this.newRecord.amount;
        this.edittedRecord.lowerBoundInterestRate = this.newRecord.lowerBoundInterestRate;
        this.edittedRecord.upperBoundInterestRate = this.newRecord.upperBoundInterestRate;
        this.edittedRecord.maturityYears = this.newRecord.maturityYears;
        await this._recordService.updateRecord(this.edittedRecord);
        window.location.reload();
      }
    }
  }

  edit(id: number){
    let record = this.findRecordById(id);
    // Show data on input fields for editing
    this.newRecord.amount = record.amount;
    this.newRecord.lowerBoundInterestRate = record.lowerBoundInterestRate;
    this.newRecord.upperBoundInterestRate = record.upperBoundInterestRate;
    this.newRecord.incrementalRate = record.incrementalRate;
    this.newRecord.maturityYears = record.maturityYears;
    this.edittedRecord = record;
  }

  clearInputs(){
    this.edittedRecord = undefined;
    this.initializeInputValues(this.newRecord);
  }

  deleteRecord(id: number) {
    const modalRef = this._modalService.open(MessageConfirmationModal, { backdrop: true, size: Constants.Modal.Medium });
    modalRef.componentInstance.message = Constants.Message.Delete;
    modalRef.result.then((result) => {
      if (result == Constants.UserConfirmed) {
        this._recordService.deleteRecord(id);
        window.location.reload();
      }
    });
  }

  findRecordById(id: number) {
    return this.records.find((record: IRecord) => record.id == id);
  }

  initializeInputValues(newRecord: IRecord){
    newRecord.amount = Constants.string.Empty;
    newRecord.lowerBoundInterestRate = Constants.string.Empty;
    newRecord.upperBoundInterestRate = Constants.string.Empty;
    newRecord.incrementalRate = Constants.string.Empty;
    newRecord.maturityYears = Constants.string.Empty;
  }
}
