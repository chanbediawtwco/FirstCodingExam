using FirstCodingExam.Data;
using FirstCodingExam.Dto;
using FirstCodingExam.Models;
using FirstCodingExam.Services.Interface;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FirstCodingExam.Controllers
{
    public class RecordController : Controller
    {
        private readonly IJwtService _jwtService;
        private readonly FirstCodingExamDbContext _context;
        private readonly IRecordService _recordService;

        public RecordController(IJwtService jwtService,
            FirstCodingExamDbContext context,
            IRecordService recordService)
        {
            _jwtService = jwtService;
            _context = context;
            _recordService = recordService;
        }

        // Get: RecordController/record/getallrecords
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet]
        [Route("records")]
        public IActionResult GetRecordsWithHistory()
        {
            var UserId = _jwtService.GetUserIdFromToken();
            var Records = _recordService.GetRecords(UserId, _context);
            if (Records.Count() > 0)
            {
                return Ok(Records);
            }
            return NoContent();
        }

        // Get: RecordController/record/getallrecords
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet]
        [Route("record/calculated-history/{recordid}/{datecreated}")]
        public IActionResult GetHistoryCalculatedRecords(int RecordId, DateTime DateCreated)
        {
            var UserId = _jwtService.GetUserIdFromToken();

            var Record = _recordService.GetCalculatedRecordForHistory(UserId, RecordId, DateCreated, _context);
            
            if (Record != null )
            {
                return Ok(Record);
            }
            return NoContent();
        }

        // Post: RecordController/record/saverecord
        [Authorize]
        [HttpPost]
        [Route("record/save")]
        public IActionResult SaveRecord([FromBody] NewRecord NewRecord)
        {
            NewRecord.UserId = _jwtService.GetUserIdFromToken();
            if (!_recordService.IsValidRecordInput(NewRecord))
            {
                return BadRequest();
            }
            _recordService.SaveAndCalculateNewRecord(NewRecord, _context);
            // Send response 201 = Created response
            return StatusCode(201);
        }

        // Put: RecordController/record/updaterecord
        [Authorize]
        [HttpPut]
        [Route("record/update")]
        public IActionResult UpdateRecord([FromBody] NewRecord UpdateRecord)
        {
            if (!_recordService.IsValidRecordInput(UpdateRecord))
            {
                return BadRequest();
            }

            UpdateRecord.UserId = _jwtService.GetUserIdFromToken();
            var DbRecord = _recordService.GetRecordByRecordIdAndUserId(UpdateRecord, _context);
            bool IsRecordExisting = DbRecord != null;
            if (IsRecordExisting)
            {
                var HasChanges = _recordService.HasChanges(UpdateRecord, DbRecord);
                if (HasChanges)
                {
                    _recordService.SavePreviousRecordToHistoryRecordTable(DbRecord, _context);
                    _recordService.UpdateAndCalculateRecord(DbRecord, UpdateRecord, _context);
                    return NoContent();
                }
                else
                {
                    // TODO: look for better response code for no update needed
                    return Ok();
                }
            }
            return NotFound();
        }

        // Delete: RecordController/record/updaterecord
        [Authorize]
        [HttpDelete]
        [Route("record/delete")]
        public IActionResult DeleteRecord([FromBody] NewRecord DeleteRecord)
        {
            DeleteRecord.UserId = _jwtService.GetUserIdFromToken();
            var DbRecord = _recordService.GetRecordByRecordIdAndUserId(DeleteRecord, _context);
            bool IsRecordExisting = DbRecord != null;
            if (IsRecordExisting)
            {
                DbRecord.IsDeleted = true;
                _context.SaveChanges();
                _context.ChangeTracker.DetectChanges();
                return Ok();
            }
            return NotFound();
        }
    }
}
