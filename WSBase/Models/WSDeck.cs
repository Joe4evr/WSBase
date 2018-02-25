using System;
using System.Collections.Generic;

namespace WSBase.Models
{
    internal sealed class WSDeck : Pile<IWSCard>
    {
        private readonly WSStage _stage;

        public WSDeck(IEnumerable<IWSCard> cards, WSStage stage)
            : base(cards)
        {
            _stage = stage;
        }

        public override bool CanBrowse  { get; } = false;
        public override bool CanClear   { get; } = false;
        public override bool CanDraw    { get; } = true;
        public override bool CanInsert  { get; } = false;
        public override bool CanPeek    { get; } = true;
        public override bool CanPut     { get; } = false;
        public override bool CanShuffle { get; } = true;
        public override bool CanTake    { get; } = false;

        protected override void OnLastDraw()
            => _stage.MustRefresh = true;
    }
}
