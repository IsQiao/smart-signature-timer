using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using smart_signature_timer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace smart_signature_timer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticleController : Controller
    {
        private readonly SSPContext _context;
        public ArticleController(SSPContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<Ariticle>> Get()
        {
            var list =await _context.Ariticle.Where(x => x.State == 1).OrderByDescending(x => x.Time).ToListAsync();

            return Json(list);
        }


        [HttpPost]
        public async Task<ActionResult<Ariticle>> Post([FromBody] ArticleRequest req)
        {
            var item = new Ariticle()
            {
                ArticleUrl = req.ArticleUrl,
                Author = req.Author,
                EosAccount = req.Account,
                State = 0,
                Time = DateTime.Now,
                Title = req.Title,
                TransactionId = req.TransactionId
            };


            _context.Ariticle.Add(item);
            await _context.SaveChangesAsync();

            return CreatedAtRoute("", new { id = item.Id }, item);
        }
    }
}
