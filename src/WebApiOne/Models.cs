namespace WebApiOne;

public static class Models
{
    public record Member
    {
        public string Email { get; init; } = string.Empty;
        public string FirstName { get; init; } = string.Empty;
        public string LastName { get; init; } = string.Empty;
        public Guid? DepartmentId { get; init; }
        public bool IsDeveloper { get; init; }
    }

    public record Project
    {
        public string? Id { get; set; }
        public string Name { get; init; } = string.Empty;
        public DateTime StartsOn { get; init; }
        public IEnumerable<Member> Members { get; init; } = [];

        public bool IsStarted => StartsOn <= DateTime.UtcNow;
    }
}