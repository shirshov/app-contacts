using System.Collections.Generic;
using Laconic;
using MyContacts.Models;
using xf = Xamarin.Forms;

namespace MyContacts.Laconic
{
    
    class Colors
    {
        public readonly xf.Color WindowBackgroundColor;
        public readonly xf.Color EntryBackgroundColor;
        public readonly xf.Color FrameBackgroundColor;
        public readonly xf.Color FrameBorderColor;
        public readonly xf.Color SystemBlue;
        public readonly xf.Color SystemGray;
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
            WindowBackgroundColor = (xf.Color) resourceColors[nameof(WindowBackgroundColor)];
            EntryBackgroundColor = (xf.Color) resourceColors[nameof(EntryBackgroundColor)];
            FrameBackgroundColor = (xf.Color) resourceColors[nameof(FrameBackgroundColor)];
            FrameBorderColor = (xf.Color) resourceColors[nameof(FrameBorderColor)];
            SystemBlue = (xf.Color) resourceColors[nameof(SystemBlue)];
            SystemGray = (xf.Color) resourceColors[nameof(SystemGray)];
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

    class State
    {
        public readonly Colors Colors;
        public readonly (double Large, double Medium) Sizes;
        public readonly string[] Themes;
        public readonly Theme SelectedTheme;

        public State(Colors colors, (double, double) sizes, string[] themes, Theme selectedTheme)
        {
            Colors = colors;
            Sizes = sizes;
            Themes = themes;
            SelectedTheme = selectedTheme;
        }
        
        public static State MainReducer(State state, Signal signal) => signal switch
        {
            SetThemeSignal t => new State (state.Colors, state.Sizes, state.Themes, t.Payload),
            _ => state
        };
    }
    
    class SetThemeSignal : Signal<Theme>
    {
        public SetThemeSignal(Theme theme) : base(theme)
        {
        }
    }
}