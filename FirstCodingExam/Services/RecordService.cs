using AutoMapper;
using FirstCodingExam.Data;
using FirstCodingExam.Dto;
using FirstCodingExam.Models;
using FirstCodingExam.Services.Interface;
using Microsoft.EntityFrameworkCore;

namespace FirstCodingExam.Services
{
    public class RecordService: IRecordService
    {
        private readonly IMapper _mapper;
        public RecordService(IMapper mapper)
        {
            _mapper = mapper;
        }
        public Record? GetRecordByRecordIdAndUserId(NewRecord UpdatedRecord, FirstCodingExamDbContext _context)
            => _context.Records.Where(Record => Record.UserId == UpdatedRecord.UserId && Record.Id == UpdatedRecord.Id)
            .FirstOrDefault();

        public List<Record> GetRecords(int Id, FirstCodingExamDbContext _context)
            => _context.Records.Where(Record => Record.UserId == Id && !Record.IsDeleted)
            .Include(Record => Record.CalculatedRecords
                .Where(x => x.DateCreated == x.Record.DateCreated))
            .Include(x => x.HistoryRecords)
            .ToList();

        public bool IsValidRecordInput(NewRecord NewRecord)
            => NewRecord != null
            && NewRecord.Amount != null
            && NewRecord.LowerBoundInterestRate != null
            && NewRecord.UpperBoundInterestRate != null
            && NewRecord.IncrementalRate != null
            && NewRecord.MaturityYears != null;

        public bool HasChanges(NewRecord UpdatedRecord, Record DbRecord)
            => DbRecord.Amount != UpdatedRecord.Amount
                    || DbRecord.LowerBoundInterestRate != UpdatedRecord.LowerBoundInterestRate
                    || DbRecord.UpperBoundInterestRate != UpdatedRecord.UpperBoundInterestRate
                    || DbRecord.IncrementalRate != UpdatedRecord.IncrementalRate
                    || DbRecord.MaturityYears != UpdatedRecord.MaturityYears;

        public void SavePreviousRecordToHistoryRecordTable(Record CurrentRecord, FirstCodingExamDbContext _context)
        {
            HistoryRecord NewHistoryRecord = _mapper.Map<HistoryRecord>(CurrentRecord);
            // Manually unset Id for historical record as Ignore or DoNotValidate not working on mapping
            NewHistoryRecord.Id = 0;
            _context.HistoryRecords.Add(NewHistoryRecord);
            _context.SaveChanges();
            _context.ChangeTracker.DetectChanges();
        }

        public void SaveAndCalculateNewRecord(NewRecord NewRecord, FirstCodingExamDbContext _context)
        {
            var DateTimeStamp = DateTime.Now;
            int RecordId;
            SaveNewRecord(NewRecord, _context, DateTimeStamp, out RecordId);
            GenerateCalculationList(RecordId, NewRecord, _context, DateTimeStamp);
        }

        private void SaveNewRecord(NewRecord NewRecord, FirstCodingExamDbContext _context, DateTime DateTimeStamp, out int RecordId)
        {
            Record Record = _mapper.Map<Record>(NewRecord);
            Record.UserId = NewRecord.UserId;
            Record.DateCreated = DateTimeStamp;
            _context.Records.Add(Record);
            _context.SaveChanges();
            _context.ChangeTracker.DetectChanges();
            RecordId = Record.Id;
        }

        private void GenerateCalculationList(int RecordId, NewRecord NewRecord, FirstCodingExamDbContext _context, DateTime DateTimeStamp)
        {
            // Calculate the new record and save it to calculated record table
            var InterestRate = Convert.ToInt32(NewRecord.LowerBoundInterestRate);
            var UpperBoundInterestRate = Convert.ToInt32(NewRecord.UpperBoundInterestRate);
            var Amount = Convert.ToDouble(NewRecord.Amount);
            // Loop to maturity year to calculate the interest per year
            for (var Years = 1; Years <= NewRecord.MaturityYears; Years++)
            {
                CalculatedRecord CalculatedRecord = new CalculatedRecord();
                CalculatedRecord.RecordId = RecordId;
                CalculatedRecord.Years = Years;

                // Add date and time stamp
                CalculatedRecord.DateCreated = DateTimeStamp;

                // Assign the First Amount as current amount before calculating it with interest
                CalculatedRecord.CurrentAmount = Amount;
                CalculatedRecord.InterestRate = InterestRate;

                // Calculate new amount with the current Amount and current Interest Rate as Future Amount
                // Future Amount = Current Amount + (Current amount * (Interest Rate converted to percentage))
                CalculatedRecord.FutureAmount = CalculatedRecord.CurrentAmount
                    + (CalculatedRecord.CurrentAmount * (InterestRate / Convert.ToDouble(100)));

                // Assign the Future Amount as next run Current Amount
                Amount = CalculatedRecord.FutureAmount;

                //Calculate New Interest Rate for the next run
                // If the Interest Rate is Lower than the Upper Bound Interest Rate
                // Add Incremental Rate to Interest Rate
                if (InterestRate < UpperBoundInterestRate)
                {
                    InterestRate += Convert.ToInt32(NewRecord.IncrementalRate);
                }
                // If the Interest Rate with the Incremental Rate becomes greater than the Upper Bound Interest Rate
                // Change the Interest Rate to Upper Bound Interest Rate
                if (InterestRate > UpperBoundInterestRate)
                {
                    InterestRate = UpperBoundInterestRate;
                }

                // Add the record to list of calculated records for database saving
                _context.CalculatedRecords.Add(CalculatedRecord);
            }
            _context.SaveChanges();
            _context.ChangeTracker.DetectChanges();
        }

        public void UpdateAndCalculateRecord(Record CurrentRecord, NewRecord UpdatedRecord, FirstCodingExamDbContext _context)
        {
            var DateTimeStamp = DateTime.Now;
            UpdateRecord(CurrentRecord, UpdatedRecord, _context, DateTimeStamp);
            GenerateCalculationList(CurrentRecord.Id, UpdatedRecord, _context, DateTimeStamp);
        }

        private void UpdateRecord(Record CurrentRecord, NewRecord UpdatedRecord, FirstCodingExamDbContext _context, DateTime DateTimeStamp)
        {
            // Manually map the new values as there are properties that should not be included but is being included in auto mapper
            CurrentRecord.Amount = Convert.ToInt32(UpdatedRecord.Amount);
            CurrentRecord.LowerBoundInterestRate = Convert.ToInt32(UpdatedRecord.LowerBoundInterestRate);
            CurrentRecord.UpperBoundInterestRate = Convert.ToInt32(UpdatedRecord.UpperBoundInterestRate);
            CurrentRecord.IncrementalRate = Convert.ToInt32(UpdatedRecord.IncrementalRate);
            CurrentRecord.MaturityYears = Convert.ToInt32(UpdatedRecord.MaturityYears);
            CurrentRecord.DateCreated = DateTimeStamp;
            _context.SaveChanges();
            _context.ChangeTracker.DetectChanges();
        }

        public Record GetCalculatedRecordForHistory(int UserId, int RecordId, DateTime DateCreated, FirstCodingExamDbContext _context)
        {
            var CalculatedRecords = _context.CalculatedRecords
                .Where(CalculatedRecord => CalculatedRecord.RecordId == RecordId
                && CalculatedRecord.DateCreated == DateCreated)
                .ToList();

            var HistoryRecord = _context.HistoryRecords
                .Where(HistoryRecord => HistoryRecord.RecordId == RecordId
                    && HistoryRecord.DateCreated == DateCreated
                    && HistoryRecord.UserId == UserId)
                .FirstOrDefault();

            // Map to Record model to easily pass at as one object to front end
            // This includes History with Calculated Values
            Record Record = _mapper.Map<Record>(HistoryRecord);
            Record.CalculatedRecords = CalculatedRecords;
            return Record;
        }
    }

}
