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
        public IActionResult GetRecords()
        {
            var UserId = _jwtService.GetUserIdFromToken();
            var Records = _recordService.GetRecordsWithHistory(UserId, _context);
            if (Records.Count() > 0)
            {
                return Ok(Records);
            }
            return NoContent();
        }

        // Post: RecordController/record/save
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

            int UserId = _jwtService.GetUserIdFromToken();
            var DbRecord = _recordService.GetRecordByRecordIdAndUserId(UserId, UpdateRecord.Id, _context);
            bool IsRecordExisting = DbRecord != null;
            if (IsRecordExisting)
            {
                var HasChanges = _recordService.HasChanges(UpdateRecord, DbRecord);
                if (HasChanges)
                {
                    _recordService.SavePreviousRecordToHistory(DbRecord, _context);
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
        [Route("record/delete/{recordid}")]
        public IActionResult DeleteRecord(int? RecordId)
        {
            int UserId = _jwtService.GetUserIdFromToken();
            var DbRecord = _recordService.GetRecordByRecordIdAndUserId(UserId, RecordId, _context);
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
