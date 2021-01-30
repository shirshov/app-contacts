using System.Collections.Generic;
using System.Linq;
using Laconic;
using MyContacts.Shared.Models;
using Xamarin.Essentials;

namespace MyContacts.Laconic
{
    //  Data
    class DataRequested : Signal { public DataRequested() : base(null) { } }
    class DataReceived : Signal<IEnumerable<Contact>> { public DataReceived(IEnumerable<Contact> contacts) : base(contacts) { } }
    class SaveContact : Signal<Contact> { public SaveContact(Contact contact) : base(contact) { } }
    class DeleteContact : Signal<Contact> { public DeleteContact(Contact contact) : base(contact) { } }

    class DisplayAlert : Signal
    {
        public DisplayAlert(string title, string message) : base(title, message)
        {
            Title = title;
            Message = message;
        }

        public string Title { get; }
        public string Message { get; }
    }

    class SetTheme : Signal<SelectedTheme> { public SetTheme(SelectedTheme theme) : base(theme) { } }

    class DeviceSettingsChanged : Signal
    {
        public DeviceSettingsChanged(AppTheme deviceTheme, NamedSizes namedSizes) : base(deviceTheme, namedSizes) { }
    }

    // Navigation
    interface INavigationSignal { }

    class ShowSettings : Signal, INavigationSignal { public ShowSettings() : base(null) { } }
    class CloseSettings : Signal, INavigationSignal { public CloseSettings() : base(null) { } }
    class   ShowContactEditor : Signal<Contact>, INavigationSignal { public ShowContactEditor(Contact contact) : base(contact) { } }
    class ShowAddContact : Signal, INavigationSignal { public ShowAddContact() : base(null) { } }
    class CloseContactEditor : Signal, INavigationSignal { public CloseContactEditor() : base(null) { } }
    class ShowDetails : Signal<Contact>, INavigationSignal { public ShowDetails(Contact contact) : base(contact) { } }

    // external apps
    interface IExternalAppRequestSignal { }

    class DialNumber : Signal<string>, IExternalAppRequestSignal { public DialNumber(string number) : base(number) { } }
    class SendTextMessage : Signal<string>, IExternalAppRequestSignal { public SendTextMessage(string number) : base(number) { } }
    class SendEmail : Signal<string>, IExternalAppRequestSignal { public SendEmail(string emailAddress) : base(emailAddress) { } }
    class ShowDirections : Signal<Contact>, IExternalAppRequestSignal {public ShowDirections(Contact contact) : base(contact) {} }

    public class Colors
    {
        public readonly Color PrimaryColor;
        public readonly Color AccentColor;
        public readonly Color WindowBackgroundColor;
        public readonly Color EntryBackgroundColor;
        public readonly Color FrameBackgroundColor;
        public readonly Color FrameBorderColor;
        public readonly Color RefreshViewBackgroundColor;

        public readonly Color SystemGray;
        public readonly Color SystemGray2;
        public readonly Color SystemGray4;

        public readonly Color SystemBlue;
        public readonly Color SystemGreen;
        public readonly Color SystemIndigo;
        public readonly Color SystemOrange;
        public readonly Color SystemPink;
        public readonly Color SystemPurple;
        public readonly Color SystemRed;
        public readonly Color SystemTeal;
        public readonly Color SystemYellow;

        public Colors(bool isDark)
        {
            PrimaryColor = "547799";
            AccentColor = isDark ? "FFD60A" : "5AC8FA";
            WindowBackgroundColor = isDark ? "202124" : "F5F5F5";
            EntryBackgroundColor = isDark ? "3B4042" : Color.White;
            FrameBackgroundColor = isDark ? "1E2222" : Color.White;
            FrameBorderColor = isDark ? "5A5C60" : Color.Default;
            RefreshViewBackgroundColor = isDark ? Color.White : Color.Black;

            SystemGray = isDark ? "8E8E93" : "8E8E93";
            SystemGray2 = isDark ? "636366" : "AEAEB2";
            SystemGray4 = isDark ? "3A3A3C" : "D1D1D6";

            SystemBlue = isDark ? "0A84FF" : "007AFF";
            SystemGreen = isDark ? "30D158" : "34C759";
            SystemIndigo = isDark ? "5E5CE6" : "5856D6";
            SystemOrange = isDark ? "FF9F0A" : "FF9500";
            SystemPink = isDark ? "FF375F" : "FF2D55";
            SystemPurple = isDark ? "BF5AF2" : "AF52DE";
            SystemRed = isDark ? "FF453A" : "FF3B30";
            SystemTeal = isDark ? "64D2FF" : "5AC8FA";
            SystemYellow = isDark ? "FFD60A" : "FFCC00";
        }
    }

    record NamedSizes(double Large, double Medium, double Small, double Micro);
    record Visuals(Colors Colors, NamedSizes Sizes);
    partial record State(bool IsFetchingData,
            Contact[] Contacts,
            SelectedTheme[] AvailableThemes,
            AppTheme DeviceTheme,
            SelectedTheme SelectedTheme,
            Visuals Visuals);

    partial record State
    {
        public bool IsDarkTheme => CalculateIsDarkTheme(DeviceTheme, SelectedTheme);

        public static State MainReducer(State state, Signal signal) => signal switch
        {
            DataRequested => state with { IsFetchingData = true },
            DataReceived dr => state with {IsFetchingData = false, Contacts = dr.Payload.ToArray() },
            SetTheme t => state with {
                SelectedTheme = t.Payload,
                Visuals = state.Visuals with { Colors = new Colors(CalculateIsDarkTheme(state.DeviceTheme, t.Payload)) }},
            //(Signals.DeviceSettingsChanged, (AppTheme DeviceTheme, NamedSizes NamedSizes) v) => state with {
            //    DeviceTheme = deviceTheme },
            //    Visuals = state.Visuals with {
            //        Colors = new Colors(CalculateIsDarkTheme(v.DeviceTheme, state.SelectedTheme)),
            //        sizes = v.NamedSizes 
            //    }},
            _ => state,
        };

        static bool CalculateIsDarkTheme(AppTheme deviceTheme, SelectedTheme selectedTheme) =>
            selectedTheme == SelectedTheme.Phone && deviceTheme == AppTheme.Dark
                              || selectedTheme == SelectedTheme.Dark;

        public static State Initial(NamedSizes namedSizes, SelectedTheme[] availableThemes,
            AppTheme deviceTheme, SelectedTheme selectedTheme) => new State(false,
                new Contact[0],
                 availableThemes,
                deviceTheme,
                selectedTheme,
                new Visuals(new Colors(CalculateIsDarkTheme(deviceTheme, selectedTheme)), namedSizes)
            );
    }
}