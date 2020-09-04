using System;
using System.Linq;
using Laconic;

namespace MyContacts.Laconic
{
    static class SettingsEditor
    {
        static BoxView ColorRow(Color color) => new BoxView
        {
            BackgroundColor = color, HeightRequest = 20, HorizontalOptions = LayoutOptions.FillAndExpand
        };

        static Grid MainContent(Visuals visuals, SelectedTheme[] availableThemes, SelectedTheme selectedTheme, bool isDarkTheme) => new Grid
        {
            RowDefinitions = "Auto, *",
            ColumnDefinitions = "*, Auto",
            ["title", columnSpan: 2] = new LargeLabel(visuals)
            {
                Text = "Settings",
                Margin = 10,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center,
            },
            ["closeButton", column: 1] = new Button
            {
                Text = "Close",
                Margin = 10,
                Clicked = () => new CloseSettings(),
                BackgroundColor = visuals.Colors.FrameBackgroundColor,
                FontSize = 12,
                BorderColor = visuals.Colors.FrameBorderColor,
                BorderWidth = isDarkTheme ? 2 : 0,
                CornerRadius = 20,
                HeightRequest = 40,
                Visual = VisualMarker.Material,
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
                        Text = "Appearance", VerticalOptions = LayoutOptions.Center,
                        BackgroundColor = visuals.Colors.FrameBackgroundColor
                    },
                    ["themeSelector"] = new StyledFrame(visuals.Colors)
                    {
                        Content = new StackLayout
                        {
                            ["lbl"] = new MediumLabel(visuals) {Text = "Theme"},
                            ["picker"] = new Picker
                            {
                                Items = availableThemes.Select(x => x.ToString()).ToArray(),
                                SelectedIndex = Array.IndexOf(availableThemes, selectedTheme),
                                SelectedIndexChanged = e => new SetTheme(availableThemes[e.SelectedIndex]),
                                Visual = VisualMarker.Material,
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

        public static ContentPage Page(State state) => new ContentPage 
        {
            BackgroundColor = state.Visuals.Colors.WindowBackgroundColor, 
            Content = MainContent(state.Visuals, state.AvailableThemes, state.SelectedTheme, state.IsDarkTheme)
        };
    }
}