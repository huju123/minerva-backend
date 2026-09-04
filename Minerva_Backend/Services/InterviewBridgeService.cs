using System.Net.Http.Json;
using Minerva_Backend.DTO.Interview;
using Minerva_Backend.IServices;

namespace Minerva_Backend.Services
{
    public class InterviewBridgeService(HttpClient _httpClient) : IInterviewBridgeService
    {
        public async Task<List<QuestionDto>?> StartAsync(string targetRole, List<object> skillProfile, int numQuestions)
        {
            var payload = new
            {
                target_role = targetRole,
                skill_profile = skillProfile,
                num_questions = numQuestions
            };

            var response = await _httpClient.PostAsJsonAsync("/interview/start", payload);
            if (!response.IsSuccessStatusCode) return null;

            var wrapper = await response.Content.ReadFromJsonAsync<PythonResponseWrapper<List<QuestionDto>>>();
            return wrapper?.Success == true ? wrapper.Data : null;
        }

        public async Task<List<InterviewEvaluationDto>?> EvaluateAsync(List<QuestionDto> questions, List<InterviewAnswerDto> answers, string targetRole)
        {
            var payload = new
            {
                questions,
                answers,
                target_role = targetRole
            };

            var response = await _httpClient.PostAsJsonAsync("/interview/evaluate", payload);
            if (!response.IsSuccessStatusCode) return null;

            var wrapper = await response.Content.ReadFromJsonAsync<PythonResponseWrapper<List<InterviewEvaluationDto>>>();
            return wrapper?.Success == true ? wrapper.Data : null;
        }
    }

    public class PythonResponseWrapper<T>
    {
        public bool Success { get; set; }
        public T? Data { get; set; }
        public string? Error { get; set; }
    }
}