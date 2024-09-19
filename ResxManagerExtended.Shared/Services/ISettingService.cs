namespace ResxManagerExtended.Shared.Services;

public interface ISettingService
{
    Task<string?> GetOptionAsStringAsync(string key);

    Task SetOptionAsStringAsync(string key, string value);
}