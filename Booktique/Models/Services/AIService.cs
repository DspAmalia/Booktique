using Booktique.Models.MainModels;
using Microsoft.EntityFrameworkCore;

namespace Booktique.Models.Services
{
    public class AIService
    {
        private readonly BooktiqueContext dbContext;        
        private Book lastRecommendedBook;

        public AIService(BooktiqueContext context)
        {
            dbContext = context;
        }

        private enum FollowUpIntent
        {
            None,
            AnotherSameGenre,
            DifferentDescriptionSameGenre,
            SimilarDescription,
            AnotherCategory
        }

        private FollowUpIntent DetectIntent(string question)
        {
            var q = question.ToLower();

            if (q.Contains("give me another") || q.Contains("another book"))
                return FollowUpIntent.AnotherSameGenre;

            if (q.Contains("something else"))
                return FollowUpIntent.DifferentDescriptionSameGenre;

            if (q.Contains("more like that") || q.Contains("similar"))
                return FollowUpIntent.SimilarDescription;

            if (q.Contains("another category") || q.Contains("different genre"))
                return FollowUpIntent.AnotherCategory;

            return FollowUpIntent.None;
        }

        public async Task<string> GetRecommendation(string question)
        {
            var intent = DetectIntent(question);
            List<Book> matches;

            if (intent != FollowUpIntent.None && lastRecommendedBook != null)
            {
                switch (intent)
                {
                    case FollowUpIntent.AnotherSameGenre:
                        matches = await dbContext.Book
                            .Where(b => b.BookCategory == lastRecommendedBook.BookCategory &&
                                        b.BookId != lastRecommendedBook.BookId)
                            .ToListAsync();
                        break;

                    case FollowUpIntent.DifferentDescriptionSameGenre:
                        matches = await dbContext.Book
                            .Where(b => b.BookCategory == lastRecommendedBook.BookCategory &&
                                        b.BookId != lastRecommendedBook.BookId &&
                                        !b.BookDescription.Equals(lastRecommendedBook.BookDescription))
                            .ToListAsync();
                        break;

                    case FollowUpIntent.SimilarDescription:
                        var keywords = ExtractKeywords(lastRecommendedBook.BookDescription);
                        matches = await dbContext.Book
                            .Where(b => b.BookId != lastRecommendedBook.BookId &&
                                        keywords.Any(k => b.BookDescription.ToLower().Contains(k)))
                            .ToListAsync();
                        break;

                    case FollowUpIntent.AnotherCategory:
                        matches = await dbContext.Book
                            .Where(b => b.BookCategory != lastRecommendedBook.BookCategory)
                            .ToListAsync();
                        break;

                    default:
                        matches = new();
                        break;
                }
            }
            else
            {
                var keywords = ExtractKeywords(question);
                matches = await dbContext.Book
                    .Where(b => keywords.Any(k =>
                        b.BookTitle.ToLower().Contains(k) ||
                        b.BookCategory.ToLower().Contains(k) ||
                        b.BookDescription.ToLower().Contains(k)))
                    .ToListAsync();
            }

            if (!matches.Any())
                return "Sorry, I couldn't find any matching books.";

            var top = matches.First();
            lastRecommendedBook = top;

            return $"You might enjoy {top.BookTitle} by {top.BookAuthor}.\n📖 Description: {top.BookDescription}";
        }

        private List<string> ExtractKeywords(string question)
        {
            var stopWords = new[] { "what", "should", "i", "read", "next", "recommend", "book", "for", "me", "can", "you", "give", "a", "please", "suggest", "about", "describe", "another", "one", "description", "something", "else" };
            var words = question.Split(' ', StringSplitOptions.RemoveEmptyEntries)
                                .Select(w => w.Trim().ToLower())
                                .Where(w => !stopWords.Contains(w) && w.Length > 2)
                                .Distinct()
                                .ToList();

            return words;
        }
    }
}
