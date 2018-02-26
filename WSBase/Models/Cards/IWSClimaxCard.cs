namespace WSBase.Models
{
    internal interface IWSClimaxCard : IWSCard
    {
        WSClimaxEffect ClimaxEffect   { get; }
        WSClimaxTrigger ClimaxTrigger { get; }
    }
}
