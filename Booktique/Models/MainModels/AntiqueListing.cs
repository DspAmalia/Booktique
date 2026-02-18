//namespace Booktique.Models.MainModels
//{
//    public class AntiqueListing
//    {
//        public int Id { get; set; }

//        // Legătura cu utilizatorul (Vânzătorul)
//        public int SellerId { get; set; }
//        public User Seller { get; set; }

//        // Legătura cu cartea originală din baza de date (pentru Titlu, Autor, etc.)
//        public int BookId { get; set; }
//        public Book OriginalBook { get; set; }

//        // Detalii specifice exemplarului utilizatorului
//        public decimal Price { get; set; }
//        public string Condition { get; set; }
//        public string Description { get; set; } // Note personale
//        public string ImagePath { get; set; } // Poza reală făcută de user

//        public DateTime DatePosted { get; set; } = DateTime.Now;
//        public bool IsSold { get; set; } = false;
//    }
//}
