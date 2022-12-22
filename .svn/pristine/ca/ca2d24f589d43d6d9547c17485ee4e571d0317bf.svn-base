using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WikiRh.Data;
using WikiRh.Models;

namespace WikiRh.Controllers
{
    public class FileController : Controller
    {
        private readonly WikiRhContext _context;

        public FileController(WikiRhContext context)
        {
            _context = context;
        }

       // GET: FileUpload

       [HttpGet]
        public FileResult DownLoadFile(int id)
        {
            var listFiles = _context.Files.Include(w => w.Questions).ToList();
             
                var FileById = (from WikiRh_File in listFiles
                                where WikiRh_File.Id_file.Equals(id)
                                select new { WikiRh_File.Id_file, WikiRh_File.Name, WikiRh_File.Content }).ToList().FirstOrDefault();
              return  File(FileById.Content, "application/octet-stream", FileById.Name);
             
        }

        public async Task<IActionResult> Index()
        {

            var wikiRhContext = _context.Files.Include(w => w.Questions);
            return View(await wikiRhContext.ToListAsync());
        }
  

        // GET: File/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var wikiRh_File = await _context.Files
                .Include(w => w.Questions)
                .FirstOrDefaultAsync(m => m.Id_file == id);
            if (wikiRh_File == null)
            {
                return NotFound();
            }

            return View(wikiRh_File);
        }

        // GET: File/Create
        public IActionResult Create()
        {
            ViewData["questions"] = new SelectList(_context.Questions, "Id_quest", "QuestionContent");
            return View();
        }

        // POST: File/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id_file,Name,Content,Id_quest")] WikiRh_File wikiRh_File, IFormFile fileupload)
        {
            if (ModelState.IsValid)
            {

                if (fileupload != null && fileupload.Length > 0)
                {

                    var fileName = Path.GetFileName(fileupload.FileName);
                    var newfile = new WikiRh_File()
                    {
                        Name = fileName,
                        Id_quest = wikiRh_File.Id_quest
                    };

                    using (var target = new MemoryStream())
                    {
                        fileupload.CopyTo(target);
                        newfile.Content = target.ToArray();
                    }

                    _context.Add(newfile);
                    await _context.SaveChangesAsync();
                }
              
                return RedirectToAction(nameof(Index));
            }
            ViewData["Id_quest"] = new SelectList(_context.Questions, "Id_quest", "QuestionContent", wikiRh_File.Id_quest);
            return View(wikiRh_File);
        }

        //public async Task<IActionResult> OnPostUploadAsync()
        //{
        //    using (var memoryStream = new MemoryStream())
        //    {
        //        await FileUpload.FormFile.CopyToAsync(memoryStream);

        //        // Upload the file if less than 2 MB
        //        if (memoryStream.Length < 2097152)
        //        {
        //            var file = new AppFile()
        //            {
        //                Content = memoryStream.ToArray()
        //            };

        //            _dbContext.File.Add(file);

        //            await _dbContext.SaveChangesAsync();
        //        }
        //        else
        //        {
        //            ModelState.AddModelError("File", "The file is too large.");
        //        }
        //    }

        //    return Page();
        //}

        // GET: File/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var wikiRh_File = await _context.Files.FindAsync(id);
            if (wikiRh_File == null)
            {
                return NotFound();
            }
            ViewData["Id_quest"] = new SelectList(_context.Questions, "Id_quest", "QuestionContent", wikiRh_File.Id_quest);
            return View(wikiRh_File);
        }

        // POST: File/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id_file,Name,Id_quest")] WikiRh_File wikiRh_File)
        {
            if (id != wikiRh_File.Id_file)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(wikiRh_File);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WikiRh_FileExists(wikiRh_File.Id_file))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["Id_quest"] = new SelectList(_context.Questions, "Id_quest", "QuestionContent", wikiRh_File.Id_quest);
            return View(wikiRh_File);
        }

        // GET: File/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var wikiRh_File = await _context.Files
                .Include(w => w.Questions)
                .FirstOrDefaultAsync(m => m.Id_file == id);
            if (wikiRh_File == null)
            {
                return NotFound();
            }

            return View(wikiRh_File);
        }

        // POST: File/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var wikiRh_File = await _context.Files.FindAsync(id);
            _context.Files.Remove(wikiRh_File);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WikiRh_FileExists(int id)
        {
            return _context.Files.Any(e => e.Id_file == id);
        }



    }
}
