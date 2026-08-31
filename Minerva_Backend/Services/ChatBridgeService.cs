using System.Net.Http.Json;
using Minerva_Backend.IServices;

namespace Minerva_Backend.Services
{
    public class ChatBridgeService(HttpClient _httpClient) : IChatBridgeService
    {
        public async Task<object?> SendMessageAsync(string message, object skillProfile, object history, string? career)
        {
            var payload = new
            {
                message,
                skill_profile = skillProfile,
                conversation_history = history,
                career
            };

            var response = await _httpClient.PostAsJsonAsync("/chat/message", payload);
            if (!response.IsSuccessStatusCode) return null;
            return await response.Content.ReadFromJsonAsync<object>();
        }
    }
}