namespace Movies.Dejmenek.Services;

public interface IBlobService
{
    Task<string> UploadAsync(IFormFile file);
    Task DeleteAsync(string fileUri);
}
