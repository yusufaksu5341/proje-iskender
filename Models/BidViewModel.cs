namespace ProjeIskender.Models;

public class BidViewModel
{
    public float    BidPrice { get; set; }
    public DateTime BidDate  { get; set; }
    public IlanModel? Ilan   { get; set; }
}
