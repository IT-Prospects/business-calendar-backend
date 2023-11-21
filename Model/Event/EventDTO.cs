namespace Model.DTO
{
    public class EventDTO
    {
        public long? Id { get; set; }

        public string? Title { get; set; }

        public string? Description { get; set; }

        public string? Address { get; set; }

        public DateTime? EventDate { get; set; }

        public TimeSpan? EventDuration { get; set; }

        public string? ArchivePassword { get; set; }

        public long? Image_Id { get; set; }

        public string? ImageURL { get; set; }
    }
}
