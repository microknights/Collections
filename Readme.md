# Enumeration
First of all, this is a fork of Headspring Enumeration, so all credits to them.

This Enumeration has a breaking change, the reason for this fork. No exceptions thrown, but returns null itstead. Also now running .net core v2.0, and added Parser methods with fallback of default.

But again, all credits to Headpring for introducing the way...

## Nuget package
```
Install-Package MicroKnights.Collections
```


### Usage simple
Declare your enumerations individual by doing this:

```
    public  class ColorType : Enumeration<ColorType>
    {
        public ColorType(int value, string displayName, Color color) 
            : base(value, displayName)
        {
            Color = color;
        }
        
        public Color Color { get; }

        public static readonly StatusType Unknown = new ColorType(-1,"unknown", Color.Transpart);
        public static readonly StatusType Red = new ColorType(10,"Red", Color.Red);
        public static readonly StatusType Green = new ColorType(20,"Green", Color.Green);
        public static readonly StatusType Blue = new ColorType(30,"Blue", Color.Blue);

        public bool IsUnknown => Value = Unknown.Value;
        public bool IsKnown => IsUnknown == false;
    }
```

### Usage more complex

Declare your enumerations individual by doing this:
```
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
```

Now think of many entities with this status property, they can now easily be integrated for specific status by using linq:

```
var failedStatus = StatusType.GetAll().Where( st => st.HasFailed );
```

or when showing a list of entities and their status:

```
foreach( var entity in entities )
{
    var statusType = entity.Status;
    <tr>
        <td>@entity.Number</td>
        <td style="text-color: @statusType.ColorHtmlRgb"><i class="fa-@statusType.FontawesomeGlyphName">@statusType.DisplayName</i>
        <td>@statusType.DisplayName</td>
        @if( statusType.CanCreate ) {
            <button>Create</button>
        }
        ... (you get the picture)
    </tr>
}
```

For use with Entity Framework, this can easily be done:

```
public class MyEntity
{
    private int _statusTypeValue;
    
    public int StatusValue => _statusTypeValue;
    public StatusType Status
    {
        get { return StatusType.FromValue(_statusTypeVal); }
        set { _statusTypeValue = value.value; }
    }
}
```

and in your mapping:

```
    modelBuilder.Entity<MyEntity>()
            .Property(b => b.StatusValue)
            .HasField("_statusTypeValue");
    modelBuilder.Entity<MyEntity>()
            .Property(b => b.Status)
            .Ignore();
```
(Possible to avoid StatusValue, or another way around - anyone?)
