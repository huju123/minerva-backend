namespace Minerva_Backend.DTO.Interview
{
    public class SubmitInterviewDto
    {
        public string AttemptId { get; set; } = string.Empty;
        public List<string> Answers { get; set; } = new();
    }
}