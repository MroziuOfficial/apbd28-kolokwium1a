namespace kolokwium1a.Controllers;

using System.Transactions;
using kolokwium1a.Models;
using kolokwium1a.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
[Route("api/books")]
[ApiController]
public class BooksController : ControllerBase
{
    private readonly IBooksRepository _booksRepository;
    public BooksController(IBooksRepository booksRepository)
    {
        _booksRepository = booksRepository;
    }
    
    [HttpGet("{id}/genres")]
    public async Task<IActionResult> GetGenre(int id)
    {
        if (!await _booksRepository.bookExists(id))
            return NotFound($"The book with given ID - {id} does not exist");

        var animal = await _booksRepository.getBook(id);
        
        return Ok(animal);
    }
    
}