using System.Collections.Generic;
using System.Linq;
using Laconic;
using Laconic.CodeGeneration;
using MyContacts.Models;
using MyContacts.Shared.Models;

namespace MyContacts.Laconic
{
    public class Colors
    {
        public readonly Color PrimaryColor;
        public readonly Color AccentColor;
        public readonly Color WindowBackgroundColor;
        public readonly Color EntryBackgroundColor;
        public readonly Color FrameBackgroundColor;
        public readonly Color FrameBorderColor;
        public readonly Color SystemBlue;
        public readonly Color SystemGray;
        public readonly Color SystemGray2;
        public readonly Color SystemGray3;
        public readonly Color SystemGray4;
        public readonly Color SystemGreen;
        public readonly Color SystemIndigo;
        public readonly Color SystemOrange;
        public readonly Color SystemPink;
        public readonly Color SystemPurple;
        public readonly Color SystemRed;
        public readonly Color SystemTeal;
        public readonly Color SystemYellow;

        public Colors(IDictionary<string, object> resourceColors)
        {
            PrimaryColor = FromXamarinFormsColor(resourceColors[nameof(PrimaryColor)]);
            AccentColor = FromXamarinFormsColor(resourceColors[nameof(AccentColor)]);
            WindowBackgroundColor = FromXamarinFormsColor(resourceColors[nameof(WindowBackgroundColor)]);
            EntryBackgroundColor = FromXamarinFormsColor(resourceColors[nameof(EntryBackgroundColor)]);
            FrameBackgroundColor = FromXamarinFormsColor(resourceColors[nameof(FrameBackgroundColor)]);
            FrameBorderColor = FromXamarinFormsColor(resourceColors[nameof(FrameBorderColor)]);
            SystemBlue = FromXamarinFormsColor(resourceColors[nameof(SystemBlue)]);
            SystemGray = FromXamarinFormsColor(resourceColors[nameof(SystemGray)]);
            SystemGray2 = FromXamarinFormsColor(resourceColors[nameof(SystemGray2)]);
            SystemGray3 = FromXamarinFormsColor(resourceColors[nameof(SystemGray3)]);
            SystemGray4 = FromXamarinFormsColor(resourceColors[nameof(SystemGray4)]);
            SystemGreen = FromXamarinFormsColor(resourceColors[nameof(SystemGreen)]);
            SystemIndigo = FromXamarinFormsColor(resourceColors[nameof(SystemIndigo)]);
            SystemOrange = FromXamarinFormsColor(resourceColors[nameof(SystemOrange)]);
            SystemPink = FromXamarinFormsColor(resourceColors[nameof(SystemPink)]);
            SystemPurple = FromXamarinFormsColor(resourceColors[nameof(SystemPurple)]);
            SystemRed = FromXamarinFormsColor(resourceColors[nameof(SystemRed)]);
            SystemTeal = FromXamarinFormsColor(resourceColors[nameof(SystemTeal)]);
            SystemYellow = FromXamarinFormsColor(resourceColors[nameof(SystemYellow)]);
        }

        Color FromXamarinFormsColor(object source) => ((Xamarin.Forms.Color)source).ToHex();
    }

    [Signals]
    interface __AppContactsSignal
    {
        Signal DataRequested();
        Signal DataReceived(IEnumerable<Contact> contacts);
        Signal SaveContact(Contact contact);
        Signal DisplayAlert(string title, string message);
        Signal SetTheme(Theme theme);
        Signal ThemeUpdated(Theme newTheme, Colors colors, NamedSizes namedSizes);
    } 
    
    [Records]
    public interface Records
    {
        record NamedSizes(double large, double medium, double small, double micro);
        record Visuals(Colors colors,  NamedSizes sizes, string[] themes, Theme selectedTheme);
        record State(bool isFetchingData, Contact[] contacts, Visuals visuals);
    }

    partial class State
    {
        public static State MainReducer(State state, Signal signal) => signal switch {
            DataRequested _ => state.With(isFetchingData: true),
            DataReceived rec => state.With(isFetchingData: false, contacts: rec.Contacts.ToArray()),
            ThemeUpdated tu => state.With(visuals: state.Visuals.With(colors: tu.Colors, sizes: tu.NamedSizes)),
            _ => state,
        };
    }
}