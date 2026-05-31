using System;

namespace Booktique.Models.Services
{
    public class ChatNotificationService
    {
        private int _totalUnreadCount;

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

        // Eveniment pe care meniul îl va asculta pentru a se re-randa
        public event Action? OnChange;

        // Metodă pentru a actualiza numărul total din orice componentă
        public void UpdateUnreadCount(int count)
        {
            TotalUnreadCount = count;
        }

        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}