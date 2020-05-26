using Laconic;

namespace MyContacts.Laconic
{
    class LargeLabel : Label
    {
        public LargeLabel(State state)
        {
            TextColor = state.Colors.SystemGray;
            FontSize = state.Sizes.Large;
        }
    }

    class MediumLabel : Label
    {
        public MediumLabel(State state)
        {
            TextColor = state.Colors.SystemGray;
            FontSize = state.Sizes.Medium; 
        }
    }

    class StyledFrame : Frame
    {
        public StyledFrame(Colors colors)
        {
            BackgroundColor = colors.FrameBackgroundColor;
            BorderColor = colors.FrameBorderColor;
            Visual = Xamarin.Forms.VisualMarker.Material;
            HasShadow = true;
        }
    }
}