namespace MovieList.Models
{
    public class Movie
    {
        public int MovieId { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Year { get; set; }
        public int Rating { get; set; }
        public string GenreId { get; set; } = string.Empty;
        public Genre Genre { get; set; } = new Genre();
        public string Slug => $"{Name.Replace(' ', '-')}-{Year}";
    }
}