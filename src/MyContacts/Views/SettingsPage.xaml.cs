using System.Collections.Generic;
using MyContacts.Laconic;
using MyContacts.Models;
using MyContacts.Styles;
using MyContacts.Util;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;

using Laconic;
using xf = Xamarin.Forms;
using Picker = Laconic.Picker;
using ScrollView = Laconic.ScrollView;

namespace MyContacts.Views
{
    public class SettingsPage : xf.ContentPage
    {
        static BoxView ColorRow(xf.Color color) => new BoxView
        {
            BackgroundColor = color, HeightRequest = 20, HorizontalOptions = xf.LayoutOptions.FillAndExpand
        };

        static Grid MainContent(State state) => new Grid
        {
            RowDefinitions = "Auto, *",
            ColumnDefinitions = "*, Auto",
            ["title", columnSpan: 2] = new LargeLabel(state)
            {
                Text = "Settings",
                Margin = 10,
                VerticalOptions = xf.LayoutOptions.Center,
                HorizontalOptions = xf.LayoutOptions.Center,
            },
            ["closeButton", column: 1] =
                new Button
                {
                    Text = "Close",
                    Margin = 10,
                    Clicked = () => new Signal("closeSettings"),
                    BackgroundColor = state.Colors.FrameBackgroundColor,
                    FontSize = 12,
                    BorderColor = state.Colors.FrameBorderColor,
                    BorderWidth = state.SelectedTheme == Theme.Dark ? 2 : 0,
                    CornerRadius = 20,
                    HeightRequest = 40,
                    Visual = xf.VisualMarker.Material,
                    TextColor = state.Colors.SystemBlue
                },
            ["scrollContainer", row: 1, columnSpan: 2] = new ScrollView
            {
                Content = new StackLayout
                {
                    Padding = 10,
                    Spacing = 10,
                    ["lbl"] =
                        new LargeLabel(state) {Text = "Appearance", VerticalOptions = xf.LayoutOptions.Center},
                    ["themeSelector"] = new StyledFrame(state.Colors)
                    {
                        Content = new StackLayout
                        {
                            ["lbl"] = new MediumLabel(state) {Text = "Theme"},
                            ["picker"] = new Picker
                            {
                                Items = state.Themes,
                                SelectedIndex = (int) state.SelectedTheme,
                                SelectedIndexChanged = newIndex => new SetThemeSignal((Theme) newIndex),
                                Visual = xf.VisualMarker.Material,
                                BackgroundColor = state.Colors.EntryBackgroundColor,
                                TextColor = state.Colors.SystemGray
                            }
                        }
                    },
                    ["c1"] = ColorRow(state.Colors.SystemBlue),
                    ["c2"] = ColorRow(state.Colors.SystemGreen),
                    ["c3"] = ColorRow(state.Colors.SystemIndigo),
                    ["c4"] = ColorRow(state.Colors.SystemOrange),
                    ["c5"] = ColorRow(state.Colors.SystemPink),
                    ["c6"] = ColorRow(state.Colors.SystemPurple),
                    ["c7"] = ColorRow(state.Colors.SystemRed),
                    ["c8"] = ColorRow(state.Colors.SystemTeal),
                    ["c9"] = ColorRow(state.Colors.SystemYellow),
                }
            }
        };

        readonly Binder<State> _binder;

        public SettingsPage()
        {
            On<iOS>().SetModalPresentationStyle(UIModalPresentationStyle.FormSheet);
            SetDynamicResource(BackgroundColorProperty, nameof(Colors.WindowBackgroundColor));

            var (themes, selectedTheme) = GetThemeList();
            _binder = Binder.Create(
                new State(
                    new Colors(xf.Application.Current.Resources),
                    (
                        xf.Device.GetNamedSize(Xamarin.Forms.NamedSize.Large, typeof(Label)),
                        xf.Device.GetNamedSize(Xamarin.Forms.NamedSize.Large, typeof(Label))
                    ),
                    themes,
                    selectedTheme),
                State.MainReducer);

            // Handle pop request:
            _binder.UseMiddleware((context, next) =>
            {
                if (context.Signal.Payload == "closeSettings")
                    Navigation.PopModalAsync();

                return next(context);
            });

            // Handle change theme request:
            _binder.UseMiddleware((context, next) =>
            {
                context = next(context);

                if (context.Signal is SetThemeSignal t)
                {
                    Settings.ThemeOption = context.State.Themes.Length == 3 ? t.Payload : t.Payload + 1;

                    ThemeHelper.ChangeTheme(t.Payload);

                    return context.WithState(new State(
                        new Colors(xf.Application.Current.Resources),
                        (
                            xf.Device.GetNamedSize(Xamarin.Forms.NamedSize.Large, typeof(Label)),
                            xf.Device.GetNamedSize(Xamarin.Forms.NamedSize.Large, typeof(Label))
                        ),
                        context.State.Themes,
                        context.State.SelectedTheme)
                    );
                }

                return context;
            });

            Content = _binder.CreateView(MainContent);
        }

        static (string[], Theme) GetThemeList()
        {
            var list = new List<string> {"Light", "Dark"};

            if (Settings.HasDefaultThemeOption)
                list.Insert(0, "Device Default");

            var savedThemeIndex = (int) Settings.ThemeOption;
            return (list.ToArray(), (Theme) (list.Count == 3 ? savedThemeIndex : savedThemeIndex - 1));
        }
    }
}

