using efcoreApp.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace efcoreApp.Controllers
{
    public class OgretmenController : Controller
    {
        private readonly DataContext _context;
        public OgretmenController(DataContext context)
        {
            _context = context;             // injection yöntemi 
        }

         public async Task<IActionResult> Index()
        {
            var ogretmenler = await _context.Ogretmenler.ToListAsync();
            return View(ogretmenler);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Ogretmen model)
        {
            _context.Ogretmenler.Add(model);
             await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }


        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var entity = await _context
            .Ogretmenler
            .FirstOrDefaultAsync(x => x.OgretmenId == id);    // Include ile findasync kullanılmıyor first or default kullanmak lazım 
            // var ogr = await _context.Ogrenciler.FirstOrDefaultAsync(o => o.OgrenciId == id);

            if (entity == null)
            {
                return NotFound();
            }

            return View(entity);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]  // güvenlik önlemi 
        public async Task<IActionResult> Edit(int id, Ogretmen model)
        {
           if(id != model.OgretmenId)
           {
              return NotFound();
           }

           if(ModelState.IsValid)
           {
               try
               {
                   _context.Update(model);
                   await _context.SaveChangesAsync();
               }
               catch (DbUpdateConcurrencyException)
               {
                  if(!_context.Ogretmenler.Any(o => o.OgretmenId == model.OgretmenId))   // Any metodu ile ilgili öğrenciid nin veritabanında olup olmadığını kontrol ediyoruz
                  {
                    return NotFound();
                  }
                  else
                  {
                    throw;
                  }
                
               }
               return RedirectToAction("Index");
           }

           return View(model);

        }

    }
}