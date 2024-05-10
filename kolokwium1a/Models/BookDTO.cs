namespace kolokwium1a.Models;

public class BookDTO
{
    public int PK { get; set; }
    public string title { get; set; }
    public List<GenreDTO> genres { get; set; } = null!;
}

public class GenreDTO
{
    public string nam { get; set; } = string.Empty;
}