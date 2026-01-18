using Booktique.Models.MainModels;
using Microsoft.JSInterop;
using System.Text.Json;

namespace Booktique.Models.Services
{
    public class CartService
    {
        private readonly IJSRuntime _js;
        public List<CartItem> Items { get; private set; } = new();
        public event Action OnChange;

        public CartService(IJSRuntime js)
        {
            _js = js;
        }

        public async Task LoadCartAsync()
        {
            try
            {
                var json = await _js.InvokeAsync<string>("localStorage.getItem", "cart");
                if (!string.IsNullOrEmpty(json))
                {
                    Items = JsonSerializer.Deserialize<List<CartItem>>(json) ?? new();
                    NotifyDataChanged();
                }
            }
            catch { /* Prevenim erori la pre-randare pe server */ }
        }

        public async Task AddToCart(Book book)
        {
            var item = Items.FirstOrDefault(i => i.BookId == book.BookId);
            if (item == null)
            {
                Items.Add(new CartItem
                {
                    BookId = book.BookId,
                    Title = book.BookTitle,
                    ImagePath = book.BookCoverPath,
                    Price = (decimal)book.BookPrice,
                    Quantity = 1
                });
            }
            else
            {
                item.Quantity++;
            }
            await SaveCartAsync();
            NotifyDataChanged();
        }

        public async Task RemoveFromCart(int bookId)
        {
            Items.RemoveAll(i => i.BookId == bookId);
            await SaveCartAsync();
            NotifyDataChanged();
        }

        public async Task ClearCart()
        {
            Items.Clear();
            await SaveCartAsync();
            NotifyDataChanged();
        }

        private async Task SaveCartAsync()
        {
            try
            {
                var json = JsonSerializer.Serialize(Items);
                await _js.InvokeVoidAsync("localStorage.setItem", "cart", json);
            }
            catch { }
        }

        private void NotifyDataChanged() => OnChange?.Invoke();
    }

    public class CartItem
    {
        public int BookId { get; set; }
        public string Title { get; set; } = "";
        public decimal Price { get; set; }
        public int Quantity { get; set; } = 1;
        public string ImagePath { get; set; } = "";
    }
}