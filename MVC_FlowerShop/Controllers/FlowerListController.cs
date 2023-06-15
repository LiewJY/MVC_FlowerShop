using Microsoft.AspNetCore.Mvc;
using MVC_FlowerShop.Models;
using MVC_FlowerShop.Data;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol;

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
        public async Task<IActionResult> Index(String searchString)
        {
            List<Flower> flowerlist = await _context.FlowerTable.ToListAsync();

            //filter the flower befoew display
            if(!string.IsNullOrEmpty(searchString))
            {
                flowerlist = flowerlist.Where(s => s.FlowerName.Contains(searchString)).ToList();
            }
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


        //function 3: learn how to delwte item from the flower tablw
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> deletepage(int? fid)
        {
            if (fid == null)
                return NotFound();
            Flower flower = await _context.FlowerTable.FindAsync(fid);

            if (flower == null)
                return NotFound();

            _context.FlowerTable.Remove(flower);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "FlowerList");
        }

        //function 4: learn how to edit the item from the flower table 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> editpage (int ? fid)
        {
            if (fid == null)
                return NotFound();
            Flower flower = await _context.FlowerTable.FindAsync(fid);
            if (flower == null)
                return NotFound();

            return View(flower);
        }

        //function 5: learn how to update form
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> updatepage(Flower flower)
        {
            if(ModelState.IsValid)
            {
                _context.FlowerTable.Update(flower);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "FlowerList");
            }
            return View("editpage", flower);
        }
    }
}
