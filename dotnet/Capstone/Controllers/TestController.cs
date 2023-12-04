using Capstone.Models;
using Capstone.Service;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Capstone.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        public readonly IRecordService recordService = new RecordService();
        public TestController() { }

        [HttpGet("GetRecord/{recordId}")]
        public ActionResult<RecordClient> GetRecordById(int recordId)
        {
            try
            {
                RecordClient output = null;
                output = recordService.GetRecord(recordId);
                if(output != null)
                {
                    return Ok(output);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("search")]
        public ActionResult<RecordClient> Search(string q, string artist, string title, string genre, string year, string country, string label)
        {
            try
            {
                RecordClient output = null;
                if (output != null)
                {
                    return Ok(output);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
