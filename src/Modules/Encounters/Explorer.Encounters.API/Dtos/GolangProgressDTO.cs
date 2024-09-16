namespace Explorer.Encounters.API.Dtos
{
    public class GolangProgressDto
    {
        public Guid Id { get; set; }
        public long UserId { get; set; }
        public int Xp { get; set; }
        public int Level { get; set; }
    }
}
