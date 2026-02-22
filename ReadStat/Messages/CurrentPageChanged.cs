using CommunityToolkit.Mvvm.Messaging.Messages;

namespace ReadStat.Messages;

public class CurrentPageChanged(object? value) : ValueChangedMessage<object?>(value);