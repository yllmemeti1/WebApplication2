using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Common.Entities;
using WebApplication2.Common.Models;
using WebApplication2.Common.Models.Car;
using WebApplication2.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace WebApplication2.Controllers
{
    public class CarController : BaseController
    {


        private readonly DataContext _context;

        public CarController(DataContext context)
        {
            _context = context;
        }

        [Authorize]
        [HttpGet("car")]
        public async Task<IActionResult> GetAllCustomers()
        {
            return Ok(await _context.Car.ToListAsync());
        }

         [HttpPost]
        public async Task<IActionResult> InsertCar([FromBody] InsertCaryDto carDto)
        {
            var car = new Car
            {
                Name = carDto.Name,
                
                InsertedAt = DateTime.UtcNow,
            };

            _context.Car.Add(car);

            await _context.SaveChangesAsync();

            return Ok(new Response { Status = "Success", Id = car.Id.ToString() });
        }

    }
}
