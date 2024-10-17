using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoniteringBackend.Data;
using MoniteringBackend.Models;

namespace MoniteringBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceRecordController : ControllerBase
    {
        private readonly MonitoringContext _context;

        public ServiceRecordController(MonitoringContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ServiceRecord>>> GetServiceRecords()
        {
            return await _context.ServiceRecords.ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<ServiceRecord>> PostServiceRecord(ServiceRecord serviceRecord)
        {
            _context.ServiceRecords.Add(serviceRecord);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetServiceRecords), new { id = serviceRecord.ServiceRecordId }, serviceRecord);
        }
    }

}
