using Laconic;
using xf = Xamarin.Forms;

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
            Visual = xf.VisualMarker.Material;
            HasShadow = true;
        }
    }

    class StyledEntry : Entry
    {
        public StyledEntry(Colors colors)
        {
            Visual = xf.VisualMarker.Material;
            BackgroundColor = colors.EntryBackgroundColor;
            TextColor = colors.SystemGray;
            PlaceholderColor = colors.AccentColor;
        }
    }
    
    class PancakeView : Layout<xf.PancakeView.PancakeView>, IContentHost
    {
        public View Content { get; set; }

        public xf.CornerRadius CornerRadius
        {
            set => SetValue(xf.PancakeView.PancakeView.CornerRadiusProperty, value);
        }

        public xf.Color BorderColor
        {
            set => SetValue(xf.PancakeView.PancakeView.BorderColorProperty, value);
        }

        public double BorderThickness
        {
            set => SetValue(xf.PancakeView.PancakeView.BorderThicknessProperty, value);
        }
    }

    static class ImageSource
    {
        static string MaterialFontName { get; } = xf.Device.RuntimePlatform == xf.Device.iOS
            ? "Material Design Icons"
            : "materialdesignicons-webfont.ttf#Material Design Icons";

        public static xf.FontImageSource FromFont(string glyph, xf.Color color) =>
            new xf.FontImageSource {FontFamily = MaterialFontName, Glyph = glyph, Color = color};
    }
}