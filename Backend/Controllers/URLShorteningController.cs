using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using URLShortening.Data;
using URLShortening.Interfaces;
using URLShortening.Models;
using URLShorteningApi.Models;

namespace URLShortening.Controllers
{
    [Route("shorten")]
    [ApiController]
    public class URLShorteningController : ControllerBase
    {
        private readonly URLShorteningContext _context;
        private readonly IUrlShorteningService _shorteningService;

        public URLShorteningController(URLShorteningContext context, IUrlShorteningService shorteningService)
        {
            _context = context;
            _shorteningService = shorteningService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<URLShorteningItem>>> GetURLShorteningItems()
        {
            if (_context.URLShorteningItems == null)
            {
                return NotFound();
            }
            return await _context.URLShorteningItems.ToListAsync();
        }

        [HttpGet("{shortCode}")]
        public async Task<ActionResult<URLShorteningItem>> GetURLShorteningItem(string shortCode)
        {
            if (_context.URLShorteningItems == null)
            {
                return NotFound();
            }

            var uRLShorteningItem = await _context.URLShorteningItems
                .FirstOrDefaultAsync(x => x.shortCode == shortCode);

            if (uRLShorteningItem == null)
            {
                return NotFound();
            }

            uRLShorteningItem.accessCount++;
            await _context.SaveChangesAsync();

            return Redirect(uRLShorteningItem.url);
        }

        [HttpPut("{shortCode}")]
        public async Task<IActionResult> PutURLShorteningItem(string shortCode, [FromBody] URLShorteningDTO dto)
        {
            if (string.IsNullOrWhiteSpace(shortCode) || dto == null || string.IsNullOrWhiteSpace(dto.url))
            {
                return BadRequest();
            }

            var existingItem = await _context.URLShorteningItems
                .FirstOrDefaultAsync(x => x.shortCode == shortCode);

            if (existingItem == null)
                return NotFound();
           
            existingItem.url = dto.url;
            existingItem.updatedAt = DateTime.UtcNow.ToString("o");

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!URLShorteningItemExists(shortCode))
                    return NotFound();
                else
                    throw;
            }

            return Ok(existingItem);
        }

        [HttpPost]
        public async Task<ActionResult<URLShorteningItem>> PostURLShorteningItem(URLShorteningItem Item)
        {
            if (string.IsNullOrWhiteSpace(Item.url))
            {
                return BadRequest("URL cannot be empty.");
            }

            var shortCode = _shorteningService.GenerateShortCode(Item.url);
            var entity = new URLShorteningItem
            {
                url = Item.url,
                shortCode = shortCode,
                createdAt = DateTime.UtcNow.ToString("o")
            };

            _context.URLShorteningItems.Add(entity);
            await _context.SaveChangesAsync();

            var scheme = Request?.Scheme ?? "http";
            var host = Request?.Host.Value ?? "localhost";
            var shortUrl = $"{scheme}://{host}/shorten/{shortCode}";

            return Created(shortUrl, new { shortUrl });
        }

        [HttpDelete("{shortCode}")]
        public async Task<IActionResult> DeleteURLShorteningItem(string shortCode)
        {
            if (_context.URLShorteningItems == null)
            {
                return NotFound();
            }

            var uRLShorteningItem = await _context.URLShorteningItems
                .FirstOrDefaultAsync(x => x.shortCode == shortCode);
            if (uRLShorteningItem == null)
            {
                return NotFound();
            }

            _context.URLShorteningItems.Remove(uRLShorteningItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("{shortCode}/stats")]
        public async Task<ActionResult<URLShorteningItem>> GetShortUrlStats(string shortCode)
        {
            if (_context.URLShorteningItems == null)
            {
                return NotFound();
            }
            var item = await _context.URLShorteningItems
                .FirstOrDefaultAsync(x => x.shortCode == shortCode);

            if (item == null)
                return NotFound();

            return Ok(item);
        }

        private bool URLShorteningItemExists(string shortCode)
        {
            return _context.URLShorteningItems.Any(e => e.shortCode == shortCode);
        }
    }
}