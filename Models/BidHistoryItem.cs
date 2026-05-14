namespace ProjeIskender.Models;

public class BidHistoryItem
{
    public ulong  UserId   { get; set; }
    public string UserName { get; set; } = "";
    public float  Price    { get; set; }
    public DateTime BidDate { get; set; }
}
