using Microsoft.AspNetCore.Mvc;
using osu.Game.Beatmaps.Formats;
using osu.Game.Beatmaps;
using osu.Game.IO;

namespace osz2_service.Controllers;

public class Parse : ControllerBase
{
    [Route("/osu/parse")]
    [HttpPost]
    [Consumes("multipart/form-data")]
    [Produces("application/json")]
    public ActionResult Post([FromForm(Name = "osu")] IFormFile osu)
    {
        try
        {
            using Stream stream = osu.OpenReadStream();
            using MemoryStream memoryStream = new MemoryStream();
            stream.CopyTo(memoryStream);
            return this.Ok(new BeatmapModel(memoryStream.ToArray()));
        }
        catch (Exception e)
        {
            return this.BadRequest(e.Message);
        }
    }
}