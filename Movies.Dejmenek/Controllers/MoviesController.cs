using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Movies.Dejmenek.Data;
using Movies.Dejmenek.Enums;
using Movies.Dejmenek.Helpers;
using Movies.Dejmenek.Models;
using Movies.Dejmenek.Services;

namespace Movies.Dejmenek.Controllers
{
    public class MoviesController : Controller
    {
        private readonly MovieContext _context;
        private readonly IBlobService _blobService;
        private const int _pageSize = 4;

        public MoviesController(MovieContext context, IBlobService blobService)
        {
            _context = context;
            _blobService = blobService;
        }

        // GET: Movies
        public async Task<IActionResult> Index(
            string movieGenre,
            string movieRating,
            int sortOption,
            string searchString,
            int? pageNumber
        )
        {
            if (_context.Movie == null) return Problem("Entity set 'MvcMovieContext.Movie' is null.");

            IQueryable<string> genreQuery = from m in _context.Movie
                                            orderby m.Genre
                                            select m.Genre;

            IQueryable<string> ratingsQuery = from m in _context.Movie
                                              orderby m.Rating
                                              select m.Rating;

            var movies = from m in _context.Movie
                         select m;

            if (!string.IsNullOrEmpty(searchString))
            {
                pageNumber = 1;
                movies = movies.Where(m => m.Title!.ToUpper().Contains(searchString.ToUpper()));
            }

            if (!string.IsNullOrEmpty(movieGenre))
            {
                movies = movies.Where(m => m.Genre == movieGenre);
            }

            if (!string.IsNullOrEmpty(movieRating))
            {
                movies = movies.Where(m => m.Rating == movieRating);
            }

            movies = (SortOptions)sortOption switch
            {
                SortOptions.TitleDesc => movies.OrderByDescending(m => m.Title),
                SortOptions.DateDesc => movies.OrderByDescending(m => m.ReleaseDate),
                SortOptions.DateAsc => movies.OrderBy(m => m.ReleaseDate),
                SortOptions.PriceDesc => movies.OrderByDescending(m => m.Price),
                SortOptions.PriceAsc => movies.OrderBy(m => m.Price),
                _ => movies.OrderBy(m => m.Title),
            };

            var movieGenreVM = new MovieGenreViewModel
            {
                MovieGenre = movieGenre,
                MovieRating = movieRating,
                SortOption = (SortOptions)sortOption,
                Genres = new SelectList(await genreQuery.Distinct().ToListAsync()),
                Ratings = new SelectList(await ratingsQuery.Distinct().ToListAsync()),
                Movies = await PaginatedList<Movie>.CreateAsync(movies.AsNoTracking(), pageNumber ?? 1, _pageSize)
            };

            return View(movieGenreVM);
        }

        // GET: Movies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movie
                .FirstOrDefaultAsync(m => m.Id == id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        // GET: Movies/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Movies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,ReleaseDate,Genre,Price,Rating,ImageFile")] MovieDTO createMovie)
        {
            if (ModelState.IsValid)
            {
                string imageUri = null;
                if (createMovie.ImageFile != null)
                {
                    imageUri = await _blobService.UploadAsync(createMovie.ImageFile);
                }
                var movie = new Movie
                {
                    Title = createMovie.Title,
                    ReleaseDate = createMovie.ReleaseDate,
                    Genre = createMovie.Genre,
                    Price = createMovie.Price,
                    Rating = createMovie.Rating,
                    ImageUri = imageUri
                };
                _context.Add(movie);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(createMovie);
        }

        // GET: Movies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movie.FindAsync(id);
            if (movie == null)
            {
                return NotFound();
            }
            var editMovie = new MovieDTO
            {
                Id = movie.Id,
                Title = movie.Title,
                ReleaseDate = movie.ReleaseDate,
                Genre = movie.Genre,
                Price = movie.Price,
                Rating = movie.Rating,
                ImageUri = movie.ImageUri
            };

            return View(editMovie);
        }

        // POST: Movies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,ReleaseDate,Genre,Price,Rating,ImageFile,ImageUri")] MovieDTO editMovie)
        {
            if (id != editMovie.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (editMovie.ImageFile != null)
                    {
                        string imageUri = await _blobService.UploadAsync(editMovie.ImageFile);
                        if (editMovie.ImageUri != null) await _blobService.DeleteAsync(editMovie.ImageUri);

                        editMovie.ImageUri = imageUri;
                    }

                    var movie = new Movie
                    {
                        Id = editMovie.Id,
                        Title = editMovie.Title,
                        ReleaseDate = editMovie.ReleaseDate,
                        Genre = editMovie.Genre,
                        Price = editMovie.Price,
                        Rating = editMovie.Rating,
                        ImageUri = editMovie.ImageUri
                    };

                    _context.Update(movie);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MovieExists(editMovie.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(editMovie);
        }

        // GET: Movies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movie
                .FirstOrDefaultAsync(m => m.Id == id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        // POST: Movies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var movie = await _context.Movie.FindAsync(id);
            if (movie != null)
            {
                if (movie.ImageUri != null) await _blobService.DeleteAsync(movie.ImageUri);
                _context.Movie.Remove(movie);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MovieExists(int id)
        {
            return _context.Movie.Any(e => e.Id == id);
        }
    }
}
