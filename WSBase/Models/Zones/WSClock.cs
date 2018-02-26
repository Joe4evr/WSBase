﻿using System;

namespace WSBase.Models
{
    internal sealed class WSClock : Pile<IWSCard>
    {
        private readonly WSStage _stage;

        public WSClock(WSStage stage)
        {
            _stage = stage;
        }

        public override bool CanBrowse  { get; } = true;
        public override bool CanClear   { get; } = true;
        public override bool CanDraw    { get; } = false;
        public override bool CanInsert  { get; } = false;
        public override bool CanPeek    { get; } = false;
        public override bool CanPut     { get; } = true;
        public override bool CanShuffle { get; } = false;
        public override bool CanTake    { get; } = true;

        protected override void OnPut(IWSCard _)
        {
            if (Count == 7)
            {
                _stage.MustLevel = true;
            }
        }
    }
}
