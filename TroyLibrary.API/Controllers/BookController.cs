using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TroyLibrary.Common.Models.Book;
using TroyLibrary.Service.Interfaces;

namespace TroyLibrary.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class BookController : ControllerBase
    {
        private readonly IBookService _bookService;

        public BookController(IBookService bookService)
        {
            _bookService = bookService;
        }

        [HttpGet("Book")]
        public async Task<GetBookResponse> GetBook([FromQuery] int bookId)
        {
            return new GetBookResponse
            {
                Book = await _bookService.GetBook(bookId),
            };
        }

        [HttpGet("Featured")]
        public GetBooksResponse GetFeaturedBooks()
        {
            return new GetBooksResponse
            {
                Books = _bookService.GetFeaturedBooks(),
            };
        }

        [HttpGet("Search")]
        public GetBooksResponse SearchBooks([FromQuery] string title)
        {
            return new GetBooksResponse
            {
                Books = _bookService.SearchBooks(title),
            };
        }
    }
}
