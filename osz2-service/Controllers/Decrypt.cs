using Microsoft.AspNetCore.Mvc;
using osu.Game.Beatmaps;
using osu.Game.Beatmaps.Formats;
using osu.Game.IO;
using Osz2Decryptor;

namespace osz2_service.Controllers;

public class Decrypt : ControllerBase
{
    [Route("/osz2/decrypt")]
    [HttpPost]
    [Consumes("multipart/form-data")]
    [Produces("application/json")]
    public ActionResult Post([FromForm(Name = "osz2")] IFormFile osz2)
    {
        try
        {
            if (osz2.Length >= int.MaxValue)
                return this.BadRequest("Invalid file size");
         
            var package = new Osz2Package(osz2.OpenReadStream());
            var files = RemoveUnusedFiles(package.Files);

            var beatmaps = package.Files
                .Where(item => item.Key.EndsWith(".osu"))
                .ToDictionary(
                    item => item.Key,
                    item => new BeatmapModel(item.Value)
                );

            return this.Ok(new Dictionary<string, object> {
                { "metadata", package.Metadata },
                { "beatmaps", beatmaps },
                { "files", files }
            });
        }
        catch (Exception e)
        {
            return this.BadRequest(e.Message);
        }
    }

    private Dictionary<string, byte[]> RemoveUnusedFiles(Dictionary<string, byte[]> files)
    {
        var validFileExtensions = new HashSet<string> {
            ".osu", ".osz", ".osb", ".osk", ".png", ".mp3", ".jpeg",
            ".wav", ".wav", ".ogg", ".jpg", ".wmv", ".flv", ".m4v",
            ".mp3", ".flac", ".mp4", ".avi", ".ini", ".jpg"
        };

        return files
            .Where(item => validFileExtensions.Contains(Path.GetExtension(item.Key.ToLower())))
            .ToDictionary(item => item.Key, item => item.Value);
    }
}