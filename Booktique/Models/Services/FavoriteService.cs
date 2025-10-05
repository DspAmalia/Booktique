using Microsoft.EntityFrameworkCore;
using System;


namespace Booktique.Models.Services
{
    public class FavoriteService
    {
        private readonly BooktiqueContext _context;

        public FavoriteService(BooktiqueContext context)
        {
            _context = context;
        }

        public async Task<Dictionary<string, int>> GetGenreDistributionAsync(int userId)
        {
            var result = await _context.Favorite
                .Include(f => f.Book)
                .Where(f => f.UserId == userId && f.Book != null)
                .GroupBy(f => f.Book.BookCategory)
                .Select(g => new { Genre = g.Key, Count = g.Count() })
                .ToDictionaryAsync(x => x.Genre, x => x.Count);

            Console.WriteLine($"Genres for user {userId}: {string.Join(", ", result.Keys)}");
            return result;
        }

    }

}
