namespace WSBase.Models
{
    internal sealed class WSCharacterPosition
    {
        private readonly WSStage _stage;
        private readonly WSStagePosition _position;

        public WSCharacterStatus? Status { get; private set; }
        public IWSCharacterCard Character { get; private set; }
        public WSMarkers Markers { get; } = new WSMarkers();

        public WSCharacterPosition(WSStagePosition position, WSStage stage)
        {
            _position = position;
            _stage = stage;
        }

        public void Stand()
        {
            if (Character != null)
                Status = WSCharacterStatus.Stand;
        }
        public void PlaceCharacter(IWSCharacterCard characterCard, WSCharacterStatus status = WSCharacterStatus.Stand)
        {
            if (Character != null)
            {
                RemoveCharacter();
            }

            Character = characterCard;
            Status = status;
            Character.SetPosition(_position);
        }

        void RemoveCharacter()
        {
            var tmp = Character;
            Character = null;
            foreach (var card in Markers.Clear())
            {
                _stage.WaitingRoom.Put(card);
            }
            _stage.WaitingRoom.Put(tmp);
        }

        public void AddMarker(IWSCard card)
            => Markers.Put(card);
    }
}
