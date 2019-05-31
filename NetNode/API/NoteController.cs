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

namespace NetNode.Api
{
    [Route("api/[controller]")]
    public class NoteController : Controller
    {
        private INoteRepository _noteRepository;
        private INoteTypeRepository _noteTypeRepository;
        public NoteController(INoteRepository noteRepository, INoteTypeRepository noteTypeRepository)
        {
            _noteRepository = noteRepository;
            _noteTypeRepository = noteTypeRepository;
        }
        [HttpGet]
        public IActionResult Get(int pageindex = 1)
        {
            var pagesize = 3;
            var notes = _noteRepository.PageList(pageindex, pagesize);
            ViewBag.PageCount = notes.Item2;
            ViewBag.PageIndex = pageindex;
            var result = notes.Item1.Select(r => new ApiNoteModel
            {
                Id = r.Id,
                Title = r.Title,
                Content = r.Content,
                Attachment = r.Attachment,
                Type = r.Type.Name
            });
            return Ok(result);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> Detail(int id)
        {
            var note = await _noteRepository.GetByIdAsync(id);
            if (note == null)
            {
                ErrorModel em = new ErrorModel
                {
                    EId = 404,
                    Message = "查询结果为空缺少数据"
                };
                return Json(em);
            }
            else
            {
                var result = new ApiNoteModel
                {
                    Id = note.Id,
                    Title = note.Title,
                    Content = note.Content,
                    Attachment = note.Attachment,
                    Type = note.Type.Name
                };
                return Ok(result);
            }
        }

    }
}
