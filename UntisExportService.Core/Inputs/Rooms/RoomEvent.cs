using Redbus.Events;
using SchulIT.UntisExport.Rooms;
using System.Collections.Generic;

namespace UntisExportService.Core.Inputs.Rooms
{
    public class RoomEvent : EventBase
    {
        public IEnumerable<Room> Rooms { get; private set; }

        public RoomEvent(IEnumerable<Room> rooms)
        {
            Rooms = rooms;
        }
    }
}