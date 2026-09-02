namespace Minerva_Backend.DTO.Journey1
{
    public class SubmitJourney1DTO
    {
        public string AssessmentId { get; set; } = Guid.NewGuid().ToString();
        public List<Journey1AnswerDTO> Answers { get; set; } = new();
    }
}