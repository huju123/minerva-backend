using System.Net.Http.Json;
using Minerva_Backend.IServices;

namespace Minerva_Backend.Services
{
    public class Journey1BridgeService(HttpClient _httpClient) : IJourney1BridgeService
    {
        public async Task<object?> GetQuestionsAsync()
        {
            // Python doesn't currently expose a separate "questions only" endpoint —
            // it returns questions embedded in the assessment JSON it already loaded.
            // See note below on how we handle this.
            var response = await _httpClient.GetAsync("/journey1/questions");
            if (!response.IsSuccessStatusCode) return null;
            return await response.Content.ReadFromJsonAsync<object>();
        }

        public async Task<object?> CompleteAssessmentAsync(
            string assessmentId,
            List<(string QuestionId, string SelectedOption)> answers)
        {
            var payload = new
            {
                assessment_id = assessmentId,
                answers = answers.Select(a => new
                {
                    question_id = a.QuestionId,
                    selected_option = a.SelectedOption
                })
            };

            var response = await _httpClient.PostAsJsonAsync("/journey1/exploring/complete", payload);
            if (!response.IsSuccessStatusCode) return null;
            return await response.Content.ReadFromJsonAsync<object>();
        }
    }
}