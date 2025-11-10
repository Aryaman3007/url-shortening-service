namespace URLShorteningApi.Models;

public class URLShorteningItem
{
    public long id { get; set; }
    public string url { get; set; } = string.Empty;
    public string shortCode { get; set; } = string.Empty;
    public string createdAt { get; set; } = DateTime.UtcNow.ToString("o");
    public string updatedAt { get; set; } = DateTime.UtcNow.ToString("o");
    public int accessCount { get; set; } = 0;

}
