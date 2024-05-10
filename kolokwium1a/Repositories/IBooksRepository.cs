using kolokwium1a.Models;

namespace kolokwium1a.Repositories;

public interface IBooksRepository
{
    Task<bool> bookExists(int id);
    Task<bool> genreExists(int id);
    Task<BookDTO> getBook(int id);
    Task<BookDTO> addBookGenre(NewBook newBook);
}