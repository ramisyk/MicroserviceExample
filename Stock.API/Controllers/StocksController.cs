using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stock.API.Models;

namespace Stock.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StocksController : ControllerBase
    {
        private readonly StockAPIDbContext _dbContext;

        public StocksController(StockAPIDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpPost]
        public async Task<IActionResult> CreateStockInfo()
        {
            Models.Entities.Stock stock = new()
            {
                ProductId = new Guid(),
                Count = 5,
                Id = new Guid()
            };
            return Ok();
        }
    }
}
