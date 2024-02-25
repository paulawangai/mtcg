using System;

public class UserProfile
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Bio { get; set; }
    public UserStats? Stats { get; set; }
}

