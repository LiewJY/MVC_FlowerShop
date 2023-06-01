using Microsoft.AspNetCore.Mvc;
using MVC_FlowerShop.Models;
using MVC_FlowerShop.Data;
using Microsoft.EntityFrameworkCore;

namespace MVC_FlowerShop.Controllers
{
    public class FlowerListController : Controller
    {
        private readonly MVC_FlowerShopContext _context;

        public FlowerListController(MVC_FlowerShopContext context)
        {
            _context = context;
        }
        //function 3: learn how to retrieve data back from flower table
        public async Task<IActionResult> Index()
        {
            List<Flower> flowerlist = await _context.FlowerTable.ToListAsync();

            return View(flowerlist);
        }

        //function 1: learn how to create the add flower form

        public IActionResult AddNewFlower()
        {
            return View();
        }

        //function 2: learn how to insert to the flower table
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddFlower(Flower flower)
        {
            if(ModelState.IsValid)
            {
                _context.FlowerTable.Add(flower);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "FlowerList");

            }
            return View("AddNewFlower", flower);
        }

    
    }
}
