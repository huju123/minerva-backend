using Minerva_Backend.DTO.Interview;

namespace Minerva_Backend.IServices
{
    public interface IInterviewBridgeService
    {
        public Task<List<QuestionDto>?> StartAsync(string targetRole, List<object> skillProfile, int numQuestions);
        public Task<List<InterviewEvaluationDto>?> EvaluateAsync(List<QuestionDto> questions, List<InterviewAnswerDto> answers, string targetRole);
    }
}