public class TradeState
{
    public bool IsLong { get; set; }
    public bool IsShort { get; set; }
    public bool IsSameTime { get; set; }
    public DateTime LastCandleAdded { get; set; }

    //function set to short, that sets the state to short
    public void SetShort()
    {
        IsLong = false;
        IsShort = true;
        IsSameTime = false;
        LastCandleAdded = DateTime.Now;
    }
    //function set to long, that sets the state to long
    public void SetLong()
    {
        IsLong = true;
        IsShort = false;
        IsSameTime = false;
        LastCandleAdded = DateTime.Now;
    }

    // set to isSameTime to true
    public void SetSameTime()
    {
        IsLong = false;
        IsShort = false;
        IsSameTime = true;
        LastCandleAdded = DateTime.Now;
    }
}
