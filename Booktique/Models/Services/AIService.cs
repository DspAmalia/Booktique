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

            if (q.Contains("another category") || q.Contains("different genre") || q.Contains("different category"))
                return FollowUpIntent.AnotherCategory;

            return FollowUpIntent.None;
        }

        private string DetectGenre(string question)
        {
            var genres = new[] { "romance", "thriller", "fantasy", "poetry", "history", "sci-fi", "memoir", "drama", "comedy", "classics" };
            var lower = question.ToLower();

            return genres.FirstOrDefault(g => lower.Contains(g)) ?? string.Empty;
        }

        public async Task<string> GetRecommendation(string question, int userId)
        {
            var intent = DetectIntent(question);
            var libraryBooks = dbContext.Book.Where(b => b.SellerId == null);
            List<Book> matches = new();

            // Verificăm INTENȚIA de follow-up ÎNAINTE de a valida cuvintele cheie
            if (intent != FollowUpIntent.None && lastRecommendedBook != null)
            {
                switch (intent)
                {
                    case FollowUpIntent.AnotherSameGenre:
                        matches = await libraryBooks
                            .Where(b => b.BookCategory == lastRecommendedBook.BookCategory &&
                                        b.BookId != lastRecommendedBook.BookId &&
                                        !recommendedBookIds.Contains(b.BookId))
                            .ToListAsync();
                        break;

                    case FollowUpIntent.DifferentDescriptionSameGenre:
                        matches = await libraryBooks
                            .Where(b => b.BookCategory == lastRecommendedBook.BookCategory &&
                                        b.BookId != lastRecommendedBook.BookId &&
                                        b.BookDescription != lastRecommendedBook.BookDescription &&
                                        !recommendedBookIds.Contains(b.BookId))
                            .ToListAsync();
                        break;

                    case FollowUpIntent.SimilarDescription:
                        var lastKeywords = ExtractKeywords(lastRecommendedBook.BookDescription);
                        matches = await libraryBooks
                            .Where(b => b.BookId != lastRecommendedBook.BookId &&
                                        !recommendedBookIds.Contains(b.BookId) &&
                                        lastKeywords.Any(k => b.BookDescription.ToLower().Contains(k)))
                            .ToListAsync();
                        break;

                    case FollowUpIntent.AnotherCategory:
                        matches = await libraryBooks
                            .Where(b => b.BookCategory != lastRecommendedBook.BookCategory &&
                                        !recommendedBookIds.Contains(b.BookId))
                            .ToListAsync();
                        break;
                }
            }
            else // Intră aici doar dacă este o căutare complet nouă (nu este legată de ultima carte recomandată)
            {
                var genre = DetectGenre(question);
                var keywords = ExtractKeywords(question);

                // Acum această barieră blochează doar textul random de la prima interacțiune
                if (string.IsNullOrEmpty(genre) && !keywords.Any())
                {
                    return "I'm sorry, I couldn't understand your request. Could you please specify a genre, author, or book topic you are looking for?";
                }

                if (!string.IsNullOrEmpty(genre))
                {
                    matches = await libraryBooks
                        .Where(b => b.BookCategory.ToLower() == genre &&
                                    !recommendedBookIds.Contains(b.BookId))
                        .ToListAsync();
                }
                else
                {
                    var availableBooks = await libraryBooks
                        .Where(b => !recommendedBookIds.Contains(b.BookId))
                        .ToListAsync();

                    var scoredBooks = new List<(Book Book, int Score)>();

                    foreach (var b in availableBooks)
                    {
                        int score = 0;
                        string title = b.BookTitle?.ToLower() ?? "";
                        string cat = b.BookCategory?.ToLower() ?? "";
                        string desc = b.BookDescription?.ToLower() ?? "";
                        string author = b.BookAuthor?.ToLower() ?? "";

                        foreach (var k in keywords)
                        {
                            if (title.Contains(k)) score += 5;
                            if (author.Contains(k)) score += 4;
                            if (cat.Contains(k)) score += 3;
                            if (desc.Contains(k)) score += 1;
                        }

                        if (score > 0)
                        {
                            scoredBooks.Add((b, score));
                        }
                    }

                    if (scoredBooks.Any())
                    {
                        int maxScore = scoredBooks.Max(sb => sb.Score);
                        matches = scoredBooks.Where(sb => sb.Score == maxScore).Select(sb => sb.Book).ToList();
                    }
                }
            }

            // Dacă nu s-au găsit cărți pentru intenția selectată sau baza de date nu mai are recomandări noi
            if (!matches.Any())
            {
                return "I couldn't find any other books matching that criteria in our collection. Try exploring a completely different topic!";
            }

            var top = matches[new Random().Next(matches.Count)];
            lastRecommendedBook = top;
            recommendedBookIds.Add(top.BookId);

            return $@" 
        <p>You might enjoy this selection from our library:</p> 
        <div class='book-card-wrapper'> 
            <a class='book-card' href='/books/details?bookid={top.BookId}'> 
                <img src='{top.BookCoverPath}' alt='{top.BookTitle}' class='book-img' /> 
                <div class='book-info'> 
                    <h5 class='book-title'>{top.BookTitle}</h5> 
                    <p class='book-author-year'> {top.BookAuthor} <br /> {top.BookYear} </p> 
                    <p class='book-price'><strong>{top.BookPrice} lei</strong></p>
                </div>
            </a>
        </div>";
        }

        private List<string> ExtractKeywords(string question)
        {
            var stopWords = new[] {
                "what", "should", "i", "read", "next", "recommend", "book", "for", "me", "can", "you", "give",
                "a", "please", "suggest", "about", "describe", "another", "one", "description", "something", "else",
                "the", "and", "with", "this", "that", "from", "recomanda", "o", "carte", "imi", "poti", "ceva", "cu", "despre",
                "random", "hello", "hi", "hey", "different", "diferit", "category", "genre"
            };

            var cleanQuestion = new string(question.Select(c => char.IsPunctuation(c) ? ' ' : c).ToArray());

            var words = cleanQuestion.Split(' ', StringSplitOptions.RemoveEmptyEntries)
                                     .Select(w => w.Trim().ToLower())
                                     .Where(w => !stopWords.Contains(w) && w.Length > 2)
                                     .Distinct()
                                     .ToList();

            return words;
        }
    }
}