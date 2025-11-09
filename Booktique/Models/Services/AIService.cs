using Booktique.Models.MainModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Booktique.Models.Services
{
    public class AIService
    {
        private readonly BooktiqueContext dbContext;
        private Book lastRecommendedBook;
        private readonly HashSet<int> recommendedBookIds = new();

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

        private string DetectGenre(string question)
        {
            var genres = new[] { "romance", "thriller", "fantasy", "poetry", "history", "sci-fi", "memoir", "drama", "comedy" };
            var lower = question.ToLower();

            return genres.FirstOrDefault(g => lower.Contains(g)) ?? string.Empty;
        }

        public async Task<string> GetRecommendation(string question, int userId)
        {
            var intent = DetectIntent(question);
            List<Book> matches = new();

            if (intent != FollowUpIntent.None && lastRecommendedBook != null)
            {
                switch (intent)
                {
                    case FollowUpIntent.AnotherSameGenre:
                        matches = await dbContext.Book
                            .Where(b => b.BookCategory == lastRecommendedBook.BookCategory &&
                                        b.BookId != lastRecommendedBook.BookId &&
                                        !recommendedBookIds.Contains(b.BookId))
                            .ToListAsync();
                        break;

                    case FollowUpIntent.DifferentDescriptionSameGenre:
                        matches = await dbContext.Book
                            .Where(b => b.BookCategory == lastRecommendedBook.BookCategory &&
                                        b.BookId != lastRecommendedBook.BookId &&
                                        !b.BookDescription.Equals(lastRecommendedBook.BookDescription) &&
                                        !recommendedBookIds.Contains(b.BookId))
                            .ToListAsync();
                        break;

                    case FollowUpIntent.SimilarDescription:
                        var keywords = ExtractKeywords(lastRecommendedBook.BookDescription);
                        matches = await dbContext.Book
                            .Where(b => b.BookId != lastRecommendedBook.BookId &&
                                        !recommendedBookIds.Contains(b.BookId) &&
                                        keywords.Any(k => b.BookDescription.ToLower().Contains(k)))
                            .ToListAsync();
                        break;

                    case FollowUpIntent.AnotherCategory:
                        matches = await dbContext.Book
                            .Where(b => b.BookCategory != lastRecommendedBook.BookCategory &&
                                        !recommendedBookIds.Contains(b.BookId))
                            .ToListAsync();
                        break;
                }
            }
            else
            {
                var genre = DetectGenre(question);
                var keywords = ExtractKeywords(question);

                if (!string.IsNullOrEmpty(genre))
                {
                    matches = await dbContext.Book
                        .Where(b => b.BookCategory.ToLower() == genre &&
                                    !recommendedBookIds.Contains(b.BookId))
                        .ToListAsync();
                }
                else
                {
                    matches = await dbContext.Book
                        .Where(b => !recommendedBookIds.Contains(b.BookId) &&
                                    keywords.Any(k =>
                                        b.BookTitle.ToLower().Contains(k) ||
                                        b.BookCategory.ToLower().Contains(k) ||
                                        b.BookDescription.ToLower().Contains(k)))
                        .ToListAsync();
                }

                if (!matches.Any())
                {
                    var genuriFolosite = recommendedBookIds
                        .Select(id => dbContext.Book.FirstOrDefault(b => b.BookId == id)?.BookCategory)
                        .Where(g => g != null)
                        .Distinct()
                        .ToList();

                    matches = await dbContext.Book
                        .Where(b => !recommendedBookIds.Contains(b.BookId) &&
                                    !genuriFolosite.Contains(b.BookCategory))
                        .ToListAsync();
                }
            }

            if (!matches.Any())
            {
                return "Sorry, I couldn't find any matching books.";
            }

            var top = matches[new Random().Next(matches.Count)];
            lastRecommendedBook = top;
            recommendedBookIds.Add(top.BookId);

            return $@" 
                <p>You might enjoy:</p> 
                <div class='book-card-wrapper'> 
                    <a class='book-card' href='/books/details?bookid={top.BookId}'> 
                    <img src='{top.BookCoverPath}' alt='{top.BookTitle}' class='book-img' /> 
                    <div class='book-info'> <h5 class='book-title'>{top.BookTitle}</h5> 
                    <p class='book-author-year'> {top.BookAuthor} <br /> {top.BookYear} <br /> 
                </p> 
                <p class='book-price'><strong>{top.BookPrice} lei</strong></p>";
        }

        private List<string> ExtractKeywords(string question)
        {
            var stopWords = new[] {
                "what", "should", "i", "read", "next", "recommend", "book", "for", "me", "can", "you", "give",
                "a", "please", "suggest", "about", "describe", "another", "one", "description", "something", "else"
            };

            var words = question.Split(' ', StringSplitOptions.RemoveEmptyEntries)
                                .Select(w => w.Trim().ToLower())
                                .Where(w => !stopWords.Contains(w) && w.Length > 2)
                                .Distinct()
                                .ToList();

            return words;
        }
    }
}
