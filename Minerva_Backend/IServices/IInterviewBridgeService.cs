namespace Minerva_Backend.IServices
{
    public interface IInterviewBridgeService
    {
        public Task<object?> StartAsync(string targetRole, List<object> skillProfile, int numQuestions);
        public Task<object?> EvaluateAsync(List<string> questions, List<string> answers, string targetRole);
    }
}