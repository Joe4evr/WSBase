﻿namespace WSBase.Models
{
    internal sealed class WSMemory : Pile<IWSCard>
    {
        public override bool CanBrowse  { get; } = true;
        public override bool CanClear   { get; } = false;
        public override bool CanDraw    { get; } = false;
        public override bool CanInsert  { get; } = false;
        public override bool CanPeek    { get; } = false;
        public override bool CanPut     { get; } = true;
        public override bool CanShuffle { get; } = false;
        public override bool CanTake    { get; } = true;
    }
}
