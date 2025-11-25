using EnrollmentSystem.Shared.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EnrollmentSystem.Frontend.Services;

public class GradeServiceClient : IGradeServiceClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<GradeServiceClient> _logger;

    public GradeServiceClient(HttpClient httpClient, ILogger<GradeServiceClient> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<IEnumerable<GradeDto>?> GetStudentGradesAsync(int studentId, string token)
    {
        try
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.GetAsync($"/api/grades/student/{studentId}");
            
            if (!response.IsSuccessStatusCode) return null;

            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<IEnumerable<GradeDto>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Grade service is unavailable");
            return null;
        }
    }

    public async Task<bool> UploadGradeAsync(UploadGradeRequest request, string token)
    {
        try
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var json = JsonSerializer.Serialize(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("/api/grades/upload", content);
            
            return response.IsSuccessStatusCode;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Grade service is unavailable during upload");
            return false;
        }
    }
}