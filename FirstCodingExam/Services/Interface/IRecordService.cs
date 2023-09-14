using FirstCodingExam.Data;
using FirstCodingExam.Dto;
using FirstCodingExam.Models;

namespace FirstCodingExam.Services.Interface
{
    public interface IRecordService
    {
        public Record? GetRecordByRecordIdAndUserId(int? UserId, int? RecordId, FirstCodingExamDbContext _context);
        public List<Record> GetRecordsWithHistory(int Id, FirstCodingExamDbContext _context);
        public bool IsValidRecordInput(NewRecord NewRecord);
        public bool HasChanges(NewRecord UpdatedRecord, Record DbRecord);
        public void SavePreviousRecordToHistory(Record CurrentRecord, FirstCodingExamDbContext _context);
        public void SaveAndCalculateNewRecord(NewRecord NewRecord, FirstCodingExamDbContext _context);
        public void UpdateAndCalculateRecord(Record CurrentRecord, NewRecord UpdatedRecord, FirstCodingExamDbContext _context);
    }
}
