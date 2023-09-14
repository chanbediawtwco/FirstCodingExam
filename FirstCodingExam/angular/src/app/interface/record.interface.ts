import { IHistoryRecord } from "./historyRecord.interface";
import { ICalculatedRecord } from "./calculatedRecord.interface";

export interface IRecord {
    id?: number,
    amount?: string,
    lowerBoundInterestRate?: string,
    upperBoundInterestRate?: string,
    incrementalRate?: string,
    maturityYears?: string,
    dateCreated?: Date,
    calculatedRecords?: ICalculatedRecord,
    historyRecords?: IHistoryRecord[]
}