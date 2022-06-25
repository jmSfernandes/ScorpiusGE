namespace SleepDataExporter.Models;

public class SleepClassify
{
    public int Confidence { get; set; }
    public int Motion { get; set; }
    public int Light { get; set; }
    public string? Timestamp { get; set; }
    public string? TimestampEvent { get; set; }
}