using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using TroyLibrary.Common.Models;
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
                BookDetail = await _bookService.GetBook(bookId),
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

        [Authorize(Roles = "Librarian")]
        [HttpPost]
        public async Task<GetBookResponse> CreateBook([FromBody] BookRequest request)
        {
            return new GetBookResponse
            {
                BookDetail = await _bookService.CreateBookAsync(request.BookData),
            };
        }

        [Authorize(Roles = "Librarian")]
        [HttpPatch("Update")]
        public async Task<CrudResponse> UpdateBook([FromBody] BookRequest request)
        {
            return new CrudResponse
            {
                CompletedAt = await _bookService.UpdateBookAsync(request.BookData),
            };
        }

        [Authorize(Roles = "Librarian")]
        [HttpDelete]
        public async Task<CrudResponse> RemoveBook([FromQuery] int bookId)
        {
            return new CrudResponse
            {
                CompletedAt = await _bookService.RemoveBookAsync(bookId),
            };
        }

        [Authorize(Roles = "Customer")]
        [HttpPatch("Checkout")]
        public async Task<BooleanResponse> CheckoutBook([FromQuery] int bookId)
        {
            return new BooleanResponse
            {
                Success = await _bookService.CheckoutBookAsync(
                    bookId, 
                    User.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub)?.Value
                 ),
            };
        }

        [Authorize(Roles = "Librarian")]
        [HttpPatch("Return")]
        public async Task<BooleanResponse> ReturnBook([FromQuery] int bookId)
        {
            return new BooleanResponse
            {
                Success = await _bookService.ReturnBookAsync(bookId),
            };
        }
    }
}
