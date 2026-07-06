using System;

namespace Booktique.Models.Services
{
    public class ChatNotificationService
    {
        private int _totalUnreadCount;
        private int _backInStockCount; // Pentru produsele din favorite intrate în stoc

        public int TotalUnreadCount
        {
            get => _totalUnreadCount;
            private set
            {
                if (_totalUnreadCount != value)
                {
                    _totalUnreadCount = value;
                    NotifyStateChanged();
                }
            }
        }

        // Proprietate accesibilă din MainLayout / Meniu
        public int BackInStockCount
        {
            get => _backInStockCount;
            private set
            {
                if (_backInStockCount != value)
                {
                    _backInStockCount = value;
                    NotifyStateChanged();
                }
            }
        }

        // Totalul combinat care se va afișa pe bulina roșie de pe clopoțel
        public int TotalNotifications => TotalUnreadCount + BackInStockCount;

        public event Action? OnChange;

        public void UpdateUnreadCount(int count)
        {
            TotalUnreadCount = count;
        }

        // Metodă apelată când se încarcă pagina sau când se modifică un stoc în fundal
        public void UpdateBackInStockCount(int count)
        {
            BackInStockCount = count;
        }

        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}