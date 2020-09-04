using System.Collections.Generic;
using System.Linq;
using Laconic;
using Laconic.CodeGeneration;
using MyContacts.Shared.Models;
using Xamarin.Essentials;

namespace MyContacts.Laconic
{
    [Signals]
    interface __AppContactsSignal
    {
        Signal DataRequested();
        Signal DataReceived(IEnumerable<Contact> contacts);
        Signal SaveContact(Contact contact);
        Signal DeleteContact(Contact contact);
        
        Signal DisplayAlert(string title, string message);
        
        Signal SetTheme(SelectedTheme theme);
        Signal DeviceSettingsChanged(AppTheme deviceTheme, NamedSizes namedSizes);
    }

    [Signals]
    interface __Navigation
    {
        Signal ShowSettings();
        Signal CloseSettings();
        Signal ShowContactEditor(Contact contact);
        Signal ShowAddContact();
        Signal CloseContactEditor();
        Signal ShowDetails(Contact contact);
    }

    [Signals]
    interface __ExternalAppRequestSignal
    {
        Signal DialNumber(string number);
        Signal SendTextMessage(string number);
        Signal SendEmail(string emailAddress);
        Signal ShowDirections(Contact contact);
    }
    
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

    [Records]
    interface Records
    {
        record NamedSizes(double large, double medium, double small, double micro);
        record Visuals(Colors colors,  NamedSizes sizes);
        record State(bool isFetchingData, 
            Contact[] contacts, 
            SelectedTheme[] availableThemes, 
            AppTheme deviceTheme, 
            SelectedTheme selectedTheme,
            Visuals visuals);
    }

    partial class State
    {
        public bool IsDarkTheme => CalculateIsDarkTheme(DeviceTheme, SelectedTheme);
        
        public static State MainReducer(State state, Signal signal) => signal switch {
            DataRequested _ => state.With(isFetchingData: true),
            DataReceived rec => state.With(isFetchingData: false, contacts: rec.Contacts.ToArray()),
            SetTheme t => state.With(
                selectedTheme: t.Theme, 
                visuals: state.Visuals.With(new Colors(CalculateIsDarkTheme(state.DeviceTheme, t.Theme)))),
            DeviceSettingsChanged s => state.With(
                deviceTheme: s.DeviceTheme, 
                visuals: state.Visuals.With(
                    colors: new Colors(CalculateIsDarkTheme(s.DeviceTheme, state.SelectedTheme)),
                    sizes: s.NamedSizes)),
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