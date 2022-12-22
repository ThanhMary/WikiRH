using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DevExpress.Data.ODataLinq.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using WikiRh.Data;
using System.Web;
using WikiRh.Models;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;

namespace WikiRh.Controllers
{
    public class QuestionsController : Controller
    {
        private readonly WikiRhContext _context;
       
        public QuestionsController(WikiRhContext context)
        {
            _context = context;
            
        }

     
        public IActionResult Index(string searchString, string searchStringBis, string currentFilter, int? pageNumber)
        {


            ViewData["CurrentFilter"] = searchStringBis;
            var questions = (from m in _context.Questions
                            select m)/*.Take(10)*/;
            ViewData["cat"] = new SelectList(_context.Categories, "Name", "Name");
            ViewBag.Files = _context.Files.ToList();
        

            if (searchString != null || searchStringBis != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchStringBis = currentFilter;
            }
            if (!String.IsNullOrEmpty(searchString))
            {
                questions = questions.Include(q => q.Categories).Where(s => s.Categories.Name.Contains(searchString));
            }
            if (!String.IsNullOrEmpty(searchStringBis))
            {
                questions = questions.Include(q => q.Categories).Where(s => s.QuestionContent.Contains(searchStringBis) || s.ResponsContent.Contains(searchStringBis));
            }
            int pageSize = 10;


            return View(PaginatedList<WikiRh_Question>.Create(questions.OrderByDescending(q => q.Count).AsNoTracking(), pageNumber ?? 1, pageSize));
            // return View(await questions.OrderByDescending(q=>q.Count).ToListAsync());

        }

        public IActionResult IndexAdmin(string searchString, string searchStringBis, string currentFilter)        
        {

            ViewData["CurrentFilter"] = searchStringBis;
            var questions = (from m in _context.Questions
                             select m);
            ViewData["cat"] = new SelectList(_context.Categories, "Name", "Name");
            ViewBag.Files = _context.Files.ToList();

            if (!String.IsNullOrEmpty(searchString))
            {
                questions = questions.Include(q => q.Categories).Where(s => s.Categories.Name.Contains(searchString));
            }
            if (!String.IsNullOrEmpty(searchStringBis))
            {
                questions = questions.Include(q => q.Categories).Where(s => s.QuestionContent.Contains(searchStringBis) || s.ResponsContent.Contains(searchStringBis));
            }

            return View(questions.ToList());
        }

        // GET: Questions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var question = await _context.Questions
                .Include(q => q.Categories)
                 .Include(q => q.Files)
                .FirstOrDefaultAsync(m => m.Id_quest == id);
            if (question == null)
            {
                return NotFound();
            }

            return View(question);
        }

        // GET: Questions/Create
        public IActionResult Create()
        {
            ViewData["Id_cat"] = new SelectList(_context.Categories, "Id_cat", "Name");

            return View();
        }

        // POST: Questions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id_quest,QuestionContent,ResponsContent,Url,Id_cat")] WikiRh_Question question, IFormFile file1, IFormFile file2)
        {
            if (ModelState.IsValid)
            {
              
                _context.Add(question);
                await _context.SaveChangesAsync();

                if (file1 != null && file1.Length > 0)
                {

                    var fileName1 = Path.GetFileName(file1.FileName);
                    var newFile1= new WikiRh_File()
                    {
                        Name = fileName1,
                        Id_quest =question.Id_quest
                    };

                    using (var target = new MemoryStream())
                    {
                        file1.CopyTo(target);
                        newFile1.Content = target.ToArray();
                    }
                    _context.Add(newFile1);
                    _context.SaveChanges();
                    question.Files = new List<WikiRh_File> { newFile1};
                }
                if (file2 != null && file2.Length > 0)
                {

                    var fileName2 = Path.GetFileName(file2.FileName);
                    var newFile2 = new WikiRh_File()
                    {
                        Name = fileName2,
                        Id_quest = question.Id_quest
                    };

                    using (var target = new MemoryStream())
                    {
                        file2.CopyTo(target);
                        newFile2.Content = target.ToArray();
                    }
                    _context.Add(newFile2);
                    _context.SaveChanges();
                    question.Files = new List<WikiRh_File> { newFile2 };
                }
                return RedirectToAction(nameof(IndexAdmin));
            }
            return View(question);
        }

             

        // GET: Questions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }

            var question = await _context.Questions.FindAsync(id);
            if (question == null)
            {
                return NotFound();
            }
            ViewData["Id_cat"] = new SelectList(_context.Categories, "Id_cat", "Name", question.Id_cat);
            return View(question);
        }

        // POST: Questions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id_quest,QuestionContent,ResponsContent,Url,Id_cat")] WikiRh_Question question, IFormFile file1, IFormFile file2)
        {
            if (id != question.Id_quest)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(question);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!QuestionExists(question.Id_quest))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                if (file1 != null && file1.Length > 0)
                {

                    var fileName1 = Path.GetFileName(file1.FileName);
                    var newFile1 = new WikiRh_File()
                    {
                        Name = fileName1,
                        Id_quest = question.Id_quest
                    };

                    using (var target = new MemoryStream())
                    {
                        file1.CopyTo(target);
                        newFile1.Content = target.ToArray();
                    }
                    _context.Add(newFile1);
                    _context.SaveChanges();
                    question.Files = new List<WikiRh_File> { newFile1 };
                }
                if (file2 != null && file2.Length > 0)
                {

                    var fileName2 = Path.GetFileName(file2.FileName);
                    var newFile2 = new WikiRh_File()
                    {
                        Name = fileName2,
                        Id_quest = question.Id_quest
                    };

                    using (var target = new MemoryStream())
                    {
                        file2.CopyTo(target);
                        newFile2.Content = target.ToArray();
                    }
                    _context.Add(newFile2);
                    _context.SaveChanges();
                    question.Files = new List<WikiRh_File> { newFile2 };
                }
                return RedirectToAction(nameof(IndexAdmin));
            }
            ViewData["Id_cat"] = new SelectList(_context.Categories, "Id_cat", "Name", question.Id_cat);
            return View(question);
        }

        // GET: Questions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var question = await _context.Questions
                .Include(q => q.Categories)
                .FirstOrDefaultAsync(m => m.Id_quest == id);
            if (question == null)
            {
                return NotFound();
            }

            return View(question);
        }

        // POST: Questions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var question = await _context.Questions.FindAsync(id);
            _context.Questions.Remove(question);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(IndexAdmin));
        }

        private bool QuestionExists(int id)
        {
            return _context.Questions.Any(e => e.Id_quest == id);
        }

        public JsonResult UpdateCount(WikiRh_Question question)
        {
            var quest = _context.Questions.FirstOrDefault(s => s.Id_quest.Equals(question.Id_quest));
            quest.Count = quest.Count + 1;
             _context.SaveChanges();
          
            return Json(question);
        }



    }
}
