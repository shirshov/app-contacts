using Laconic;
using pv = Xamarin.Forms.PancakeView;

namespace MyContacts.Laconic
{
    class LargeLabel : Label
    {
        public LargeLabel(Visuals visuals)
        {
            TextColor = visuals.Colors.SystemGray;
            FontSize = visuals.Sizes.Large;
        }
    }

    class MediumLabel : Label
    {
        public MediumLabel(Visuals visuals)
        {
            TextColor = visuals.Colors.SystemGray;
            FontSize = visuals.Sizes.Medium;
        }
    }

    class SmallLabel : Label
    {
        public SmallLabel(Visuals visuals)
        {
            TextColor = visuals.Colors.SystemGray;
            FontSize = visuals.Sizes.Small;
        }
    }

    class MicroLabel : Label
    {
        public MicroLabel(Visuals visuals)
        {
            TextColor = visuals.Colors.SystemGray;
            FontSize = visuals.Sizes.Micro;
        }
    }

    class StyledFrame : Frame
    {
        public StyledFrame(Colors colors)
        {
            BackgroundColor = colors.FrameBackgroundColor;
            BorderColor = colors.FrameBorderColor;
            Visual = VisualMarker.Material;
            HasShadow = true;
        }
    }

    class StyledEntry : Entry
    {
        public StyledEntry(Colors colors)
        {
            Visual = VisualMarker.Material;
            BackgroundColor = colors.EntryBackgroundColor;
            TextColor = colors.SystemGray;
            PlaceholderColor = colors.AccentColor;
        }
    }
    
    class PancakeView : Layout<Xamarin.Forms.PancakeView.PancakeView>, IContentHost
    {
        public View Content { get; set; }

        public CornerRadius CornerRadius
        {
            set => SetValue(pv.PancakeView.CornerRadiusProperty, value);
        }

        public Color BorderColor
        {
            set => SetValue(pv.PancakeView.BorderColorProperty, value);
        }

        public double BorderThickness
        {
            set => SetValue(pv.PancakeView.BorderThicknessProperty, value);
        }
    }

    static class IconFont
    {
        public static string Name;
    }
 }