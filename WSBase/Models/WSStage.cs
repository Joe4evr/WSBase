﻿using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading.Tasks;

namespace WSBase.Models
{
    internal sealed class WSStage
    {
        public event Func<WSInterruptChoice> ChooseLevelRefresh;
        public event Func<WSLevelChoice> ChooseLevelUp;

        private WSClock Clock  { get; }
        private WSDeck Library { get; }

        internal IWSCard[] LevelZone       { get; } = new IWSCard[3];
        internal WSMemory Memory           { get; } = new WSMemory();
        internal WSStock Stock             { get; } = new WSStock();
        internal WSWaitingRoom WaitingRoom { get; } = new WSWaitingRoom();

        private Queue<IWSCard> ResolutionZone { get; } = new Queue<IWSCard>();

        private WSCharacterPosition CenterLeft   { get; }
        private WSCharacterPosition CenterMiddle { get; }
        private WSCharacterPosition CenterRight  { get; }
        private WSCharacterPosition BackLeft     { get; }
        private WSCharacterPosition BackRight    { get; }

        public bool MustRefresh { get; internal set; }
        public bool MustLevel   { get; internal set; }

        public IWSClimaxCard ClimaxZone { get; private set; }

        internal IReadOnlyDictionary<WSStagePosition, WSCharacterPosition> AllPositions { get; }

        public WSStage(IEnumerable<IWSCard> library)
        {
            Library = new WSDeck(library.Shuffle(32), this);
            Clock = new WSClock(this);

            CenterLeft = new WSCharacterPosition(WSStagePosition.CenterLeft, this);
            CenterMiddle = new WSCharacterPosition(WSStagePosition.CenterMiddle, this);
            CenterRight = new WSCharacterPosition(WSStagePosition.CenterRight, this);
            BackLeft = new WSCharacterPosition(WSStagePosition.BackLeft, this);
            BackRight = new WSCharacterPosition(WSStagePosition.BackRight, this);
            AllPositions = new Dictionary<WSStagePosition, WSCharacterPosition>
            {
                [WSStagePosition.CenterLeft] = CenterLeft,
                [WSStagePosition.CenterMiddle] = CenterMiddle,
                [WSStagePosition.CenterRight] = CenterRight,
                [WSStagePosition.BackLeft] = BackLeft,
                [WSStagePosition.BackRight] = BackRight
            }.ToImmutableDictionary();
        }

        private async Task CheckInterrupts()
        {
            if (MustLevel && MustRefresh)
            {
                var promise = new TaskCompletionSource<WSInterruptChoice>();
                Task.Factory.StartNew(() => promise.SetResult(ChooseLevelRefresh()));
                var result = await promise.Task;





                return;
            }

            if (MustLevel)
            {
                var promise = new TaskCompletionSource<WSLevelChoice>();
                Task.Factory.StartNew(() => promise.SetResult(ChooseLevelUp()));
                var result = await promise.Task;


            }

            if (MustRefresh)
            {
                await Refresh();
            }
        }

        private async Task Refresh()
        {
            Library.Reshuffle(() =>
            {
                MustRefresh = false;
                return WaitingRoom.Clear().Shuffle(28);
            });
            //Clock.Put(Library.Draw());
            await CheckInterrupts();
        }

        internal async Task TakeDamage(int soul)
        {
            for (int i = 0; i < soul; i++)
            {
                var tmp = Library.Draw();
                ResolutionZone.Enqueue(tmp);
                await CheckInterrupts();

                if (tmp is IWSClimaxCard)
                {
                    while (ResolutionZone.Count > 0)
                    {
                        WaitingRoom.Put(ResolutionZone.Dequeue());
                    }
                    return;
                }
            }

            while (ResolutionZone.Count > 0)
            {
                Clock.Put(ResolutionZone.Dequeue());
                await CheckInterrupts();
            }
        }
    }
}
