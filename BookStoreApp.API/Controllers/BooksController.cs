using AutoMapper;
using AutoMapper.QueryableExtensions;
using BookStoreApp.API.Data;
using BookStoreApp.API.Models.Book;
using BookStoreApp.API.Static;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookStoreApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BooksController : ControllerBase
    {
        private readonly BookStoreDbContext _context;
        private readonly IMapper mapper;
        private readonly ILogger<BooksController> logger;
        private readonly IWebHostEnvironment webHostEnvironment;

        public BooksController(BookStoreDbContext context, IMapper mapper, ILogger<BooksController> logger,
            IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            this.mapper = mapper;
            this.logger = logger;
            this.webHostEnvironment = webHostEnvironment;
        }

        // GET: api/Books
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookReadOnlyDto>>> GetBooks()
        {
            if (_context.Books == null)
            {
                logger.LogWarning(Messages.RecordNotFound);
                return NotFound();
            }

            //var books = await _context.books.include(q => q.author).tolistasync();
            //var booksdtos = mapper.map<ienumerable<bookreadonlydto>>(books);
            //return ok(booksdtos);

            try
            {
                var bookDtos = await _context.Books
                .Include(q => q.Author)
                .ProjectTo<BookReadOnlyDto>(mapper.ConfigurationProvider)
                .ToListAsync();

                return Ok(bookDtos);

            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Error Performing GET in {nameof(GetBooks)}");
                return StatusCode(500, Messages.Error500Message);
            }


        }

        // GET: api/Books/5
        [HttpGet("{id}")]

        public async Task<ActionResult<BookDetailsDto>> GetBook(int id)
        {
            if (_context.Books == null)
            {
                logger.LogWarning(Messages.RecordNotFound);
                return NotFound();
            }

            try
            {
                var book = await _context.Books
                .Include(q => q.Author)
                .ProjectTo<BookDetailsDto>(mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(q => q.Id == id);

                if (book == null)
                {
                    logger.LogWarning(Messages.RecordNotFound);
                    return NotFound();
                }

                return Ok(book);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Error Performing GET in {nameof(GetBook)}");
                return StatusCode(500, Messages.Error500Message);

            }


        }

        // PUT: api/Books/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> PutBook(int id, BookUpdateDto bookUpdateDto)
        {
            if (id != bookUpdateDto.Id)
            {
                logger.LogWarning($"Update ID Invalid in {nameof(PutBook)} : {id}");
                return BadRequest();
            }

            var book = await _context.Books.FindAsync(id);

            if (book == null)
            {
                logger.LogWarning(Messages.RecordNotFound);
                return NotFound();
            }

            if (string.IsNullOrEmpty(bookUpdateDto.ImageData) == false)
            {
                bookUpdateDto.Image = CreateFile(bookUpdateDto.ImageData, bookUpdateDto.OriginaImageName);

                var imageName = Path.GetFileName(book.Image);
                var path = $"{webHostEnvironment.WebRootPath}\\bookcoverimages\\{imageName}";
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
            }

            mapper.Map(bookUpdateDto, book);

            _context.Entry(book).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await BookExistsAsync(id))
                {
                    logger.LogWarning(Messages.RecordNotFound);
                    return NotFound();
                }
                else
                {
                    return StatusCode(500, Messages.Error500Message);
                }
            }

            return NoContent();
        }

        // POST: api/Books
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult<Book>> PostBook(BookCreateDto bookDto)
        {
            if (_context.Books == null)
            {
                return Problem("Entity set 'BookStoreDbContext.Books'  is null.");
            }

            try
            {
                var book = mapper.Map<Book>(bookDto);
                book.Image = CreateFile(bookDto.ImageData, bookDto.OriginaImageName);
                _context.Books.Add(book);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetBook", new { id = book.Id }, book);

            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Error performing POST in {nameof(PostBook)}");
                return StatusCode(500, Messages.Error500Message);
            }



        }

        // DELETE: api/Books/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            if (_context.Books == null)
            {
                logger.LogWarning($"{Messages.RecordNotFound} - {nameof(DeleteBook)}");
                return NotFound();
            }
            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                logger.LogWarning($"{Messages.RecordNotFound} - {nameof(DeleteBook)}");
                return NotFound();
            }

            try
            {
                _context.Books.Remove(book);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                logger.LogError($"Error processing DELETE in {nameof(DeleteBook)}");
                return StatusCode(500, Messages.Error500Message);
            }


        }

        private string CreateFile(string imageBase64, string imageName)
        {
            var url = HttpContext.Request.Host.Value;
            var ext = Path.GetExtension(imageName);

            var fileName = $"{Guid.NewGuid().ToString()}{ext}";

            var path = $"{webHostEnvironment.WebRootPath}\\bookcoverimages\\{fileName}";

            byte[] image = Convert.FromBase64String(imageBase64);
            var fileStream = System.IO.File.Create(path);
            fileStream.Write(image, 0, image.Length);
            fileStream.Close();

            return $"https://{url}/bookcoverimages/{fileName}";
        }

        private async Task<bool> BookExistsAsync(int id)
        {
            return await _context.Books.AnyAsync(book => book.Id == id);
        }
    }
}
