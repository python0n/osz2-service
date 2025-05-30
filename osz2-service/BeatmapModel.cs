using osu.Game.Beatmaps;
using osu.Game.Rulesets;
using osu.Game.Beatmaps.Formats;
using osu.Game.IO;

namespace osz2_service;

[Serializable]
public class BeatmapModel
{
    public BeatmapModel(Beatmap info)
    {
        ApplyBeatmap(info);
    }

    public BeatmapModel(byte[] data)
    {
        using var reader = new LineBufferedReader(new MemoryStream(data));
        var beatmap = Decoder.GetDecoder<Beatmap>(reader).Decode(reader);
        ApplyBeatmap(beatmap);
    }

    private void ApplyBeatmap(Beatmap beatmap)
    {
        Metadata = beatmap.Metadata;
        DifficultyName = beatmap.BeatmapInfo.DifficultyName;
        Ruleset = beatmap.BeatmapInfo.Ruleset;
        Difficulty = beatmap.BeatmapInfo.Difficulty;
        MaxCombo = beatmap.BeatmapInfo.MaxCombo;
        TotalLength = beatmap.CalculatePlayableLength();
        DrainLength = beatmap.CalculateDrainLength();
        BPM = beatmap.ControlPointInfo.BPMMinimum;
    }

    public BeatmapMetadata Metadata { get; set; }
    public string DifficultyName { get; set; }
    public RulesetInfo Ruleset { get; set; }
    public BeatmapDifficulty Difficulty { get; set; }
    public int? MaxCombo { get; set; }
    public double TotalLength { get; set; }
    public double DrainLength { get; set; }
    public double BPM { get; set; }
}
