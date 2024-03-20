namespace todoAPI.Model
{
    public class Item
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime FinishedAt { get; set; } 
        public long? OwnerId { get; set; }

    }
}
