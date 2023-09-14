import { Injectable } from '@angular/core';
import { environment } from '../../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { IRecord } from 'src/app/interface/record.interface';
import { ICalculatedRecord } from 'src/app/interface/calculatedRecord.interface';

@Injectable({
  providedIn: 'root'
})
export class RecordService {

  constructor(private _httpClient: HttpClient) { }

  getRecords(){
    return this._httpClient.get<IRecord>(`${environment.uri}/records`);
  }
  
  async addRecord(record: IRecord) {
    return await this._httpClient.post<IRecord>(`${environment.uri}/record/save`, record)
    .subscribe(result => { return result });
  }

  async updateRecord(record: IRecord) {
    return await this._httpClient.put<IRecord>(`${environment.uri}/record/update`, record)
    .subscribe(result => { return result });
  }

  deleteRecord(recordId?: number) {
    return this._httpClient.delete<IRecord>(`${environment.uri}/record/delete/${recordId}`)
    .subscribe(result => { return result });
  }
}