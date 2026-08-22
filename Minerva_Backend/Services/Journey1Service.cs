using Microsoft.EntityFrameworkCore;
using Minerva_Backend.Data;
using Minerva_Backend.DTO.Journey1;
using Minerva_Backend.DTO.Journey2;
using Minerva_Backend.GenericResponse;
using Minerva_Backend.IServices;
using Minerva_Backend.Models;
using System.Text.Json;

namespace Minerva_Backend.Services
{
    public class Journey1Service(AppDbContext _context, IJourney1BridgeService _bridge) : IJourney1Service
    {
        public async Task<ResponseResult<List<Journey1QuestionDTO>>> GetQuestions()
        {
            var questions = await _context.Journey1Questions.ToListAsync();

            var dtos = questions.Select(q => new Journey1QuestionDTO
            {
                QuestionId = q.QuestionId,
                Career = q.Career,
                CareerName = q.CareerName,
                Title = q.Title,
                QuestionType = q.QuestionType,
                Interaction = q.Interaction,
                Instruction = q.Instruction,
                Options = JsonSerializer.Deserialize<object>(q.OptionsJson)!
            }).ToList();

            return new ResponseResult<List<Journey1QuestionDTO>>
            {
                Data = dtos,
                Message = "Journey 1 questions fetched successfully.",
                Status = true,
            };
        }

        public async Task<ResponseResult<object>> SubmitAssessment(string userId, SubmitJourney1DTO dto)
        {
            if (dto.Answers == null || dto.Answers.Count == 0)
            {
                return new ResponseResult<object>
                {
                    Data = null,
                    Message = "Answers are required.",
                    Status = false,
                };
            }

            var answerTuples = dto.Answers
                .Select(a => (a.QuestionId, a.SelectedOption))
                .ToList();

            var result = await _bridge.CompleteAssessmentAsync(dto.AssessmentId, answerTuples);

            if (result == null)
            {
                return new ResponseResult<object>
                {
                    Data = null,
                    Message = "Journey 1 scoring service is unavailable. Please try again later.",
                    Status = false,
                };
            }

            var journey1Result = new Journey1Result
            {
                UserId = userId,
                AssessmentId = dto.AssessmentId,
                ResultJson = JsonSerializer.Serialize(result)
            };

            _context.Journey1Results.Add(journey1Result);
            await _context.SaveChangesAsync();

            return new ResponseResult<object>
            {
                Data = result,
                Message = "Journey 1 assessment completed successfully.",
                Status = true,
            };
        }

        public async Task<ResponseResult<object>> GetResult(string userId, string assessmentId)
        {
            var result = await _context.Journey1Results
                .Where(r => r.UserId == userId && r.AssessmentId == assessmentId)
                .OrderByDescending(r => r.CreatedAt)
                .FirstOrDefaultAsync();

            if (result == null)
            {
                return new ResponseResult<object>
                {
                    Data = null,
                    Message = "No Journey 1 result found for this assessment.",
                    Status = false,
                };
            }

            return new ResponseResult<object>
            {
                Data = JsonSerializer.Deserialize<object>(result.ResultJson),
                Message = "Journey 1 result fetched successfully.",
                Status = true,
            };
        }
    }
}