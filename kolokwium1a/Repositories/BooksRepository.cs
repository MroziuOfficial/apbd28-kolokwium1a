using kolokwium1a.Models;
using Microsoft.Data.SqlClient;

namespace kolokwium1a.Repositories;

public class BooksRepository : IBooksRepository
{
    private readonly IConfiguration _config;

    public BooksRepository(IConfiguration config)
    {
        _config = config;
    }

    public async Task<bool> bookExists(int id)
    {
        var query = "SELECT 1 FROM books WHERE PK = @id";

        await using SqlConnection connection = new SqlConnection(_config.GetConnectionString("Default"));
        await using SqlCommand command = new SqlCommand();

        command.Connection = connection;
        command.CommandText = query;
        command.Parameters.AddWithValue("@id", id);

        await connection.OpenAsync();

        var res = await command.ExecuteScalarAsync();

        return res is not null;
    }

    public async Task<bool> genreExists(int id)
    {
        var query = "SELECT 1 FROM genres WHERE PK = @id";

        await using SqlConnection connection = new SqlConnection(_config.GetConnectionString("Default"));
        await using SqlCommand command = new SqlCommand();

        command.Connection = connection;
        command.CommandText = query;
        command.Parameters.AddWithValue("@id", id);

        await connection.OpenAsync();

        var res = await command.ExecuteScalarAsync();

        return res is not null;
    }

    public async Task<BookDTO> getBook(int id)
    {
        var query =
            @"SELECT bk.PK, bk.title, gn.name FROM books bk INNER JOIN books_genres bg ON bk.PK = bg.FK_book INNER JOIN genres gn ON bg.FK_genre = gn.PK where bk.PK = @id";

        await using SqlConnection connection = new SqlConnection(_config.GetConnectionString("Default"));
        await using SqlCommand command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@id", id);

        await connection.OpenAsync();

        var reader = await command.ExecuteReaderAsync();
        var bookIdOrdinal = reader.GetOrdinal("PK");
        var bookTitleOrdinal = reader.GetOrdinal("title");
        var genreName = reader.GetOrdinal("name");
        
        BookDTO book = null;
        while (await reader.ReadAsync())
        {
            if (book == null)
            {
                book = new BookDTO()
                {
                    PK = reader.GetInt32(bookIdOrdinal),
                    title = reader.GetString(bookTitleOrdinal),
                    genres = new List<GenreDTO>()
                    {
                        new GenreDTO()
                        {
                            nam = reader.GetString(genreName)
                        }
                    }
                };
            }
            else
            {
                book.genres.Add(new GenreDTO()
                {
                    nam = reader.GetString(genreName)
                });
            }
        }

        if (book is null)
        {
            throw new Exception();
        }
        return book;
    }
}