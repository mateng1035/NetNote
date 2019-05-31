using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NetNode.Models;
using NetNode.Tools;

namespace NetNode.Controllers
{
    public class NoteController : Controller
    {
        private INoteRepository _noteRepository;
        private INoteTypeRepository _noteTypeRepository;
        public NoteController(INoteRepository noteRepository, INoteTypeRepository noteTypeRepository)
        {
            _noteRepository = noteRepository;
            _noteTypeRepository = noteTypeRepository;
        }

        //public async Task<IActionResult> List()
        //{
        //    var notes = await _noteRepository.ListAsync();
        //    return View(notes);
        //}
        //[HttpGet("/Note/List/{pageindex}")]
        public IActionResult List(int pageindex = 1)
        {
            //var notes = await _noteRepository.ListAsync();
            var pagesize = 3;
            var notes = _noteRepository.PageList(pageindex, pagesize);
            ViewBag.PageCount = notes.Item2;
            ViewBag.PageIndex = pageindex;
            return View(notes.Item1);
        }

        [HttpGet("/Note/Update/{id}")]
        public async Task<IActionResult> Add(int id)
        {
            var note = _noteRepository.GetByIdAsync(id);
            NoteModel model = new NoteModel();
            model.Id = note.Result.Id;
            model.Title = note.Result.Title;
            model.Content = note.Result.Content;
            model.Type = note.Result.TypeId;

            var types = await _noteTypeRepository.ListAsync();
            ViewBag.Types = types.Select(r => new SelectListItem { Text = r.Name, Value = r.Id.ToString() });

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            NoteModel model = new NoteModel();
            model.Id = -1;

            var types = await _noteTypeRepository.ListAsync();
            ViewBag.Types = types.Select(r => new SelectListItem { Text = r.Name, Value = r.Id.ToString() });

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Add(NoteModel model)
        {
            string filename = string.Empty;
            if (model.Attachment != null)
            {
                filename = Path.Combine("file", Guid.NewGuid().ToString() + Path.GetExtension(model.Attachment.FileName));
                var path = Directory.GetCurrentDirectory() + "\\wwwroot";
                using (var stream = new FileStream(Path.Combine(path, filename), FileMode.CreateNew))
                {
                    model.Attachment.CopyTo(stream);
                }
            }
            if (model.Id == -1)
            {
                await _noteRepository.AddAsync(new Note
                {
                    Title = model.Title,
                    Content = model.Content,
                    Create = DateTime.Now,
                    Update = DateTime.Now,
                    TypeId = model.Type,
                    Password = model.Password,
                    Attachment = filename
                }); ; ;
            }
            else
            {
                var dbNote = _noteRepository.GetByIdAsync(model.Id);
                if (dbNote != null)
                {
                    if (!string.IsNullOrEmpty(model.Password))
                    {
                        dbNote.Result.Password = model.Password;
                    }
                    if (!string.IsNullOrEmpty(filename))
                    {
                        dbNote.Result.Attachment = filename;
                    }
                    dbNote.Result.Title = model.Title;
                    dbNote.Result.Content = model.Content;
                    dbNote.Result.Update = DateTime.Now;
                    dbNote.Result.TypeId = model.Type;
                    await _noteRepository.UpdateAsync(dbNote.Result);
                }
            }
            return RedirectToAction("List");
        }
        [HttpPost]
        public async Task<IActionResult> Update(NoteModel model)
        {
            if (model.Id != -1)
            {
                await _noteRepository.UpdateAsync(new Note
                {
                    Id = model.Id,
                    Title = model.Title,
                    Content = model.Content,
                    Create = DateTime.Now
                });
            }
            return RedirectToAction("List");
        }
    }
}
