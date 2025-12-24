using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Serilog;
using System.Text;

namespace SchoolManagementSystem.Infrastructure.Middlewares
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public LoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Log request
            context.Request.EnableBuffering(); // So body can be read multiple times
            var requestBody = await ReadRequestBody(context.Request);
            Log.Information("HTTP Request {Method} {Path} Body: {Body}",
                            context.Request.Method, context.Request.Path, MaskSensitiveData(requestBody));

            // Capture response
            var originalBodyStream = context.Response.Body;
            using var responseBody = new MemoryStream();
            context.Response.Body = responseBody;

            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Unhandled exception for {Method} {Path}", context.Request.Method, context.Request.Path);
                throw;
            }

            context.Response.Body.Seek(0, SeekOrigin.Begin);
            var responseText = await new StreamReader(context.Response.Body).ReadToEndAsync();
            context.Response.Body.Seek(0, SeekOrigin.Begin);

            Log.Information("HTTP Response {StatusCode} Body: {Body}",
                            context.Response.StatusCode, MaskSensitiveData(responseText));

            await responseBody.CopyToAsync(originalBodyStream);
        }

        private async Task<string> ReadRequestBody(HttpRequest request)
        {
            request.Body.Seek(0, SeekOrigin.Begin);
            using var reader = new StreamReader(request.Body, Encoding.UTF8, leaveOpen: true);
            var body = await reader.ReadToEndAsync();
            request.Body.Seek(0, SeekOrigin.Begin);
            return body;
        }

        private string MaskSensitiveData(string body)
        {
            if (string.IsNullOrEmpty(body)) return body;

            try
            {
                var jToken = JsonConvert.DeserializeObject<JToken>(body);
                if (jToken is null) return body;

                return JsonConvert.SerializeObject(MaskValues(jToken), Formatting.None);
            }
            catch
            {
                return body; // If not JSON, just return original
            }
        }

        private JToken MaskValues(JToken token)
        {
            if (token is JObject obj)
            {
                foreach (var prop in obj.Properties())
                {
                    if (prop.Name.ToLower().Contains("password") ||
                        prop.Name.ToLower().Contains("token") ||
                        prop.Name.ToLower().Contains("secret"))
                    {
                        prop.Value = "***";
                    }
                    else
                    {
                        MaskValues(prop.Value); // Recursive
                    }
                }
            }
            else if (token is JArray array)
            {
                foreach (var item in array)
                {
                    MaskValues(item); // Recursive
                }
            }
            return token;
        }
    }
}
