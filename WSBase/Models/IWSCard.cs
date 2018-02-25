namespace WSBase.Models
{
    internal interface IWSCard
    {
        string Id         { get; }
        string Name       { get; }
        WSColor Color     { get; }
        WSTrigger Trigger { get; }
    }
}
