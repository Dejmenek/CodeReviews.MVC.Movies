using Microsoft.AspNetCore.Mvc.Rendering;
using Movies.Dejmenek.Enums;
using Movies.Dejmenek.Helpers;

namespace Movies.Dejmenek.Models;

public class MovieGenreViewModel
{
    public PaginatedList<Movie>? Movies { get; set; }
    public SelectList? Genres { get; set; }
    public SelectList? Ratings { get; set; }
    public string? MovieGenre { get; set; }
    public string? MovieRating { get; set; }
    public SortOptions? SortOption { get; set; }
    public string? SearchString { get; set; }
}
