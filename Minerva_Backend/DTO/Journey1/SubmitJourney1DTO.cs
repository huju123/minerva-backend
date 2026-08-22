namespace Minerva_Backend.DTO.Journey1
{
    public class SubmitJourney1DTO
    {
        public string AssessmentId { get; set; } = "minerva_career_discovery_v4";
        public List<Journey1AnswerDTO> Answers { get; set; } = new();
    }
}