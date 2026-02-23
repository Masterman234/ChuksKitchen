using Microsoft.AspNetCore.Http;

namespace ChuksKitchen.Application.Interfaces.IServices;

public interface ICloudinaryService
{
    Task<string> UploadAsync(IFormFile file);
}