import { ICalculatedRecord } from "./calculatedRecord.interface";

export interface IHistoryRecord {
    id?: number,
    recordId?: number,
    amount?: number,
    lowerBoundInterestRate?: number,
    upperBoundInterestRate?: number,
    incrementalRate?: number,
    maturityYears?: number,
    dateCreated?: Date,
    calculatedRecords?: ICalculatedRecord
}