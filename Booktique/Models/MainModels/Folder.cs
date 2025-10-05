using System.Text.Json.Serialization;

namespace Booktique.Models.MainModels
{
    public class Folder
    {
        public int FolderId { get; set; }
        public string FolderName { get; set; } = string.Empty;

        public int UserId { get; set; }
        public User User { get; set; } = default!;

        [JsonIgnore]
        public ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();
    
    }
}
