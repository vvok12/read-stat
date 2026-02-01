using CommunityToolkit.Mvvm.Messaging.Messages;

namespace ReadStat.Messages;

public class CurrentPageChanged: ValueChangedMessage<object?>
{
    public CurrentPageChanged(object? value) : base(value)
    {
    }
}