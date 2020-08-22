using MyContacts.Models;
using Laconic;
using xf = Xamarin.Forms;
using Picker = Laconic.Picker;
using ScrollView = Laconic.ScrollView;

namespace MyContacts.Laconic
{
    public static class SettingsEditor
    {
        static BoxView ColorRow(xf.Color color) => new BoxView
        {
            BackgroundColor = color, HeightRequest = 20, HorizontalOptions = xf.LayoutOptions.FillAndExpand
        };

        static Grid MainContent(Visuals visuals) => new Grid
        {
            RowDefinitions = "Auto, *",
            ColumnDefinitions = "*, Auto",
            ["title", columnSpan: 2] = new LargeLabel(visuals)
            {
                Text = "Settings",
                Margin = 10,
                VerticalOptions = xf.LayoutOptions.Center,
                HorizontalOptions = xf.LayoutOptions.Center,
            },
            ["closeButton", column: 1] = new Button
            {
                Text = "Close",
                Margin = 10,
                Clicked = () => new Signal("closeSettings"),
                BackgroundColor = visuals.Colors.FrameBackgroundColor,
                FontSize = 12,
                BorderColor = visuals.Colors.FrameBorderColor,
                BorderWidth = visuals.SelectedTheme == Theme.Dark ? 2 : 0,
                CornerRadius = 20,
                HeightRequest = 40,
                Visual = xf.VisualMarker.Material,
                TextColor = visuals.Colors.SystemBlue
            },
            ["scrollContainer", row: 1, columnSpan: 2] = new ScrollView
            {
                Content = new StackLayout
                {
                    Padding = 10,
                    Spacing = 10,
                    ["lbl"] = new LargeLabel(visuals)
                    {
                        Text = "Appearance", VerticalOptions = xf.LayoutOptions.Center,
                        BackgroundColor = visuals.Colors.FrameBackgroundColor
                    },
                    ["themeSelector"] = new StyledFrame(visuals.Colors)
                    {
                        Content = new StackLayout
                        {
                            ["lbl"] = new MediumLabel(visuals) {Text = "Theme"},
                            ["picker"] = new Picker
                            {
                                Items = visuals.Themes,
                                SelectedIndex = (int) visuals.SelectedTheme,
                                SelectedIndexChanged = e => new SetTheme((Theme) e.SelectedIndex),
                                Visual = xf.VisualMarker.Material,
                                BackgroundColor = visuals.Colors.EntryBackgroundColor,
                                TextColor = visuals.Colors.SystemGray
                            }
                        }
                    },
                    ["c1"] = ColorRow(visuals.Colors.SystemBlue),
                    ["c2"] = ColorRow(visuals.Colors.SystemGreen),
                    ["c3"] = ColorRow(visuals.Colors.SystemIndigo),
                    ["c4"] = ColorRow(visuals.Colors.SystemOrange),
                    ["c5"] = ColorRow(visuals.Colors.SystemPink),
                    ["c6"] = ColorRow(visuals.Colors.SystemPurple),
                    ["c7"] = ColorRow(visuals.Colors.SystemRed),
                    ["c8"] = ColorRow(visuals.Colors.SystemTeal),
                    ["c9"] = ColorRow(visuals.Colors.SystemYellow),
                }
            }
        };

        public static ContentPage Page(Visuals visuals) => new ContentPage {Content = MainContent(visuals)};
    }
}