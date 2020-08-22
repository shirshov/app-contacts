using System.Collections.Generic;
using System.Linq;
using Laconic;
using Laconic.CodeGeneration;
using MyContacts.Models;
using MyContacts.Shared.Models;
using xf = Xamarin.Forms;

namespace MyContacts.Laconic
{
    public class Colors
    {
        public readonly xf.Color PrimaryColor;
        public readonly xf.Color AccentColor;
        public readonly xf.Color WindowBackgroundColor;
        public readonly xf.Color EntryBackgroundColor;
        public readonly xf.Color FrameBackgroundColor;
        public readonly xf.Color FrameBorderColor;
        public readonly xf.Color SystemBlue;
        public readonly xf.Color SystemGray;
        public readonly xf.Color SystemGray2;
        public readonly xf.Color SystemGray3;
        public readonly xf.Color SystemGray4;
        public readonly xf.Color SystemGreen;
        public readonly xf.Color SystemIndigo;
        public readonly xf.Color SystemOrange;
        public readonly xf.Color SystemPink;
        public readonly xf.Color SystemPurple;
        public readonly xf.Color SystemRed;
        public readonly xf.Color SystemTeal;
        public readonly xf.Color SystemYellow;

        public Colors(IDictionary<string, object> resourceColors)
        {
            PrimaryColor = (xf.Color) resourceColors[nameof(PrimaryColor)];
            AccentColor = (xf.Color) resourceColors[nameof(AccentColor)];
            WindowBackgroundColor = (xf.Color) resourceColors[nameof(WindowBackgroundColor)];
            EntryBackgroundColor = (xf.Color) resourceColors[nameof(EntryBackgroundColor)];
            FrameBackgroundColor = (xf.Color) resourceColors[nameof(FrameBackgroundColor)];
            FrameBorderColor = (xf.Color) resourceColors[nameof(FrameBorderColor)];
            SystemBlue = (xf.Color) resourceColors[nameof(SystemBlue)];
            SystemGray = (xf.Color) resourceColors[nameof(SystemGray)];
            SystemGray2 = (xf.Color) resourceColors[nameof(SystemGray2)];
            SystemGray3 = (xf.Color) resourceColors[nameof(SystemGray3)];
            SystemGray4 = (xf.Color) resourceColors[nameof(SystemGray4)];
            SystemGreen = (xf.Color) resourceColors[nameof(SystemGreen)];
            SystemIndigo = (xf.Color) resourceColors[nameof(SystemIndigo)];
            SystemOrange = (xf.Color) resourceColors[nameof(SystemOrange)];
            SystemPink = (xf.Color) resourceColors[nameof(SystemPink)];
            SystemPurple = (xf.Color) resourceColors[nameof(SystemPurple)];
            SystemRed = (xf.Color) resourceColors[nameof(SystemRed)];
            SystemTeal = (xf.Color) resourceColors[nameof(SystemTeal)];
            SystemYellow = (xf.Color) resourceColors[nameof(SystemYellow)];
        }
    }

    [Signals]
    interface __AppContactsSignal
    {
        Signal DataRequested();
        Signal DataReceived(IEnumerable<Contact> contacts);
        Signal SaveContact(Contact contact);
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