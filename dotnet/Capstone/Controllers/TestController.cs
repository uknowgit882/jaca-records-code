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
        public ActionResult<Record> GetRecordById(int recordId)
        {
            try
            {
                Record output = null;
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
    }
}
