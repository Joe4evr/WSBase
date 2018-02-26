namespace WSBase.Models
{
    internal interface IWSCharacterCard : IWSCard
    {
        int Power    { get; }
        bool OnStage { get; }

        void CheckContinuous();
        void SetPosition(WSStagePosition? position);
    }
}
