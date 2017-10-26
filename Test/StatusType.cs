using System.Drawing;
using MicroKnights.Collections;

namespace MicroKnights.Collections.Test
{
    public abstract class StatusType : Enumeration<StatusType>
    {
        protected StatusType(int value, string displayName) 
            : base(value, displayName)
        {}

        public static readonly StatusType Draft = new DraftStatus();
        public static readonly StatusType Created = new CreatedStatus();
        public static readonly StatusType Open = new StartedStatus();
        public static readonly StatusType Waiting = new WaitingStatus();
        public static readonly StatusType Complete = new CompletedStatus();
        public static readonly StatusType Failed = new FailedStatus();

        public virtual bool InProgress => false;
        public virtual bool CanCreate => false;
        public virtual bool CanWork => false;
        public virtual bool HasFailed => false;

        public abstract Color Color { get; }
        public virtual string ColorHtmlRgb => $"rgb({Color.R},{Color.G},{Color.B})";

        public abstract string FontawesomeGlyphName { get; }
        public abstract string FontawesomeGlyphHex { get; }
    }


    public sealed class DraftStatus : StatusType
    {
        public DraftStatus()
            : base(0, "Draft")
        { }

        public override bool CanCreate => true;
        public override Color Color => Color.Gray;
        public override string FontawesomeGlyphName => "fa-circle-thin";
        public override string FontawesomeGlyphHex => "f1db";
    }

    public sealed class CreatedStatus : StatusType
    {
        public CreatedStatus()
            : base(10, "Created")
        { }

        public override bool CanWork => true;
        public override Color Color => Color.Green;
        public override string FontawesomeGlyphName => "fa-plus-circle";
        public override string FontawesomeGlyphHex => "f055";
    }

    public sealed class StartedStatus : StatusType
    {
        public StartedStatus()
            : base(20, "Started")
        { }

        public override bool InProgress => true;
        public override bool CanWork => true;
        public override Color Color => Color.Green;
        public override string FontawesomeGlyphName => "fa-pencil";
        public override string FontawesomeGlyphHex => "f040";
    }

    public sealed class WaitingStatus : StatusType
    {
        public WaitingStatus()
            : base(30, "Waiting")
        { }

        public override bool InProgress => true;
        public override bool CanWork => true;
        public override Color Color => Color.DarkGreen;
        public override string FontawesomeGlyphName => "fa-pause";
        public override string FontawesomeGlyphHex => "f04c";
    }

    public sealed class CompletedStatus : StatusType
    {
        public CompletedStatus()
            : base(1000, "Completed")
        { }

        public override Color Color => Color.Blue;
        public override string FontawesomeGlyphName => "fa-flag-checkered";
        public override string FontawesomeGlyphHex => "f11e";
    }

    public sealed class FailedStatus : StatusType
    {
        public FailedStatus()
            : base(-1, "Failed")
        { }

        public override Color Color => Color.Red;
        public override bool HasFailed => true;
        public override string FontawesomeGlyphName => "fa-exclamation-triangle";
        public override string FontawesomeGlyphHex => "f071";
    }

}