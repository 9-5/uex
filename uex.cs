using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System;

public class Uex
{
    private static readonly HttpClient httpClient = new HttpClient();
    public static async Task Main(string[] args)
    {
        string? scriptContent = null;

        try
        {
            if (Console.IsInputRedirected)
            {
                scriptContent = await Console.In.ReadToEndAsync();
            }
            else if (args.Length == 1)
            {
                string url = args[0];

                // Ensure the URL is valid and uses HTTPS.
                if (!Uri.TryCreate(url, UriKind.Absolute, out var uriResult) || uriResult.Scheme != Uri.UriSchemeHttps)
                {
                    Console.Error.WriteLine("Error: Invalid URL or non-HTTPS protocol. Only https:// is allowed.");
                    return;
                }

                scriptContent = await httpClient.GetStringAsync(uriResult);
            }
            else
            {
                PrintUsage();
                return;
            }
            if (!string.IsNullOrWhiteSpace(scriptContent))
            {
                await ExecuteBatchScript(scriptContent);
            }
        }
        catch (HttpRequestException ex)
        {
            Console.Error.WriteLine($"Network Error: Failed to download script. ({ex.Message})");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"An unexpected error occurred: {ex.Message}");
        }
    }

    private static async Task ExecuteBatchScript(string scriptContent)
    {
        string tempFilePath = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName() + ".bat");

        try
        {
            await File.WriteAllTextAsync(tempFilePath, scriptContent);
            var processInfo = new ProcessStartInfo("cmd.exe", $"/c \"{tempFilePath}\"")
            {
                UseShellExecute = false
            };

            using (var process = Process.Start(processInfo))
            {
                if (process != null)
                {
                    await process.WaitForExitAsync();
                }
            }
        }
        finally
        {
            if (File.Exists(tempFilePath))
            {
                File.Delete(tempFilePath);
            }
        }
    }

    private static void PrintUsage()
    {
        Console.WriteLine("uex: Executes a remote Windows batch script from a URL.");
        Console.WriteLine("\nUsage:");
        Console.WriteLine("  1. Direct URL: uex.exe https://example.com/script.bat");
        Console.WriteLine("  2. Piped:      curl https://example.com/script.bat | uex.exe");
    }
}