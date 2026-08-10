namespace Minerva_Backend.Models;

public class UserProfile
{
    public string? Id { get; set; }
    public string? UserId { get; set; }
    public User? User { get; set; }

    public string? University { get; set; }
    public string? Degree { get; set; }
    public string? Semester { get; set; }
    public string? Interests { get; set; }   // comma-separated or JSON string for now
    public string? Skills { get; set; }
    public string? GitHubUrl { get; set; }
    public string? LinkedInUrl { get; set; }
    public string? JourneyType { get; set; } // exploring / career_mind / job_hunting
}