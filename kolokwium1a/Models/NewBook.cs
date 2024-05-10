namespace kolokwium1a.Models;

public class NewBook
{
    public string title { get; set; }
    public IEnumerable<GenreDTO> genres { get; set; } = new List<GenreDTO>();
}