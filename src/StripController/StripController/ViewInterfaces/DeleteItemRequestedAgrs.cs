using System;
using StripController.PresentationEntities;

namespace StripController.ViewInterfaces
{
    public class DeleteItemRequestedAgrs : EventArgs
    {
        public ProgramItemPe Item { get; }

        public DeleteItemRequestedAgrs(ProgramItemPe item)
        {
            Item = item;
        }
    }
}