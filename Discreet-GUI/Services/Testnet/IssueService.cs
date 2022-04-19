using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Services.Testnet
{
    /// <summary>
    /// A service class to interact with the Issue service, responsible for saving wallet / daemon issues during the testnet
    /// </summary>
    public class IssueService
    {
        private readonly HttpClient _httpClient;

        public IssueService(HttpClient httpClient)
        {
            httpClient.BaseAddress = new Uri("http://localhost:5556/");
            httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("multipart/form-data"));
            _httpClient = httpClient;
        }

        public async Task<SubmitIssueResult> SubmitIssue(string summary, IssueSeverity severity, string description, string attachmentPath)
        {
            using var formContent = new MultipartFormDataContent();
            formContent.Add(new StringContent(summary), "summary");
            formContent.Add(new StringContent(((int)severity).ToString()), "severity");
            formContent.Add(new StringContent(description), "description");

            formContent.Add(new StringContent("file"));
            FileInfo fi = new FileInfo(attachmentPath);
            var fileContent = new ByteArrayContent(System.IO.File.ReadAllBytes(fi.FullName));
            fileContent.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("form-data")
            {
                Name = "file",
                FileName = fi.Name
            };
            formContent.Add(fileContent);

            var response = await _httpClient.PostAsync("/issue", formContent);
            var responseContent = await response.Content.ReadAsStringAsync();

            if(!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.TooManyRequests) return new SubmitIssueResult("Please wait a minute, before submitting another issue");   

                SubmitIssueErrorResponse errorResponse = System.Text.Json.JsonSerializer.Deserialize<SubmitIssueErrorResponse>(responseContent);
                return new SubmitIssueResult(errorResponse.ErrorMessage);
            }

            return new SubmitIssueResult("The issue has been submitted");
        }
    }

    public enum IssueSeverity
    {
        Low = 0,
        Medium = 1,
        High = 2
    }

    public class SubmitIssueResponse
    {
        [JsonPropertyName("hasError")]
        public bool HasError { get; set; }
    }

    public class SubmitIssueErrorResponse
    {
        [JsonPropertyName("hasError")]
        public bool HasError { get; set; }

        [JsonPropertyName("errorMessage")]
        public string ErrorMessage { get; set; }
    }

    public class SubmitIssueResult
    {
        public string Message { get; set; }

        public SubmitIssueResult(string message)
        {
            Message = message;
        }
    }
}
