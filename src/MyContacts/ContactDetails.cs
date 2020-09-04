using System;
using System.Linq;
using System.Threading.Tasks;
using Laconic;
using Laconic.Maps;
using MyContacts.Laconic;
using MyContacts.Shared.Models;

namespace MyContacts.Views
{
    static class ContactDetails
    {
        static ImageButton ActionButton(string glyph, Func<Signal> clicked, Colors colors, double padding = 4) =>
            new ImageButton
            {
                Clicked = clicked,
                VerticalOptions = LayoutOptions.Center,
                BackgroundColor = colors.FrameBackgroundColor,
                BorderColor = colors.SystemGray2,
                BorderWidth = 2,
                CornerRadius = 20,
                WidthRequest = 40,
                HeightRequest = 40,
                Padding = padding,
                Visual = VisualMarker.Material,
                Source = new FontImageSource
                {
                    FontFamily = IconFont.Name, Glyph = glyph, Color = colors.PrimaryColor
                }
            };

        static Grid Content(Contact contact, Position? position, Visuals visuals) => new Grid
        {
            RowSpacing = 12,
            ColumnDefinitions = "124, *",
            RowDefinitions = "Auto, Auto, *",
            ["photo"] = new PancakeView
            {
                CornerRadius = (40, 15, 15, 40),
                Margin = (0, 12, 0, 0),
                HeightRequest = 100,
                HorizontalOptions = LayoutOptions.Center,
                IsClippedToBounds = true,
                BackgroundColor = visuals.Colors.FrameBackgroundColor,
                BorderColor = visuals.Colors.SystemGray2,
                BorderThickness = 3,
                VerticalOptions = LayoutOptions.Center,
                WidthRequest = 100,
                Content = new Image {Aspect = Aspect.Fill, Source = contact.PhotoUrl}
            },
            ["details", column: 1] = new StackLayout
            {
                ["job"] = new StackLayout
                {
                    Padding = (0, 12, 0, 0),
                    Spacing = 0,
                    ["company"] = new LargeLabel(visuals) {Text = contact.Company},
                    ["title"] = new SmallLabel(visuals) {Text = contact.JobTitle},
                },
                ["address"] = new MicroLabel(visuals)
                {
                    // Don't understand why FormattedString was used here in the original code. Windows Phone?
                    FormattedText = new FormattedString
                    {
                        contact.Street,
                        "\n",
                        contact.City,
                        ", ",
                        contact.StatePostal,
                        "\n",
                        contact.Phone,
                        "\n",
                        contact.Email
                    }
                }
            },
            ["buttons", row: 1, columnSpan: 2] =
                new StackLayout
                {
                    HorizontalOptions = LayoutOptions.Center,
                    Orientation = StackOrientation.Horizontal,
                    Spacing = 24,
                    ["btn-directions"] = ActionButton("\uf1d0", () => new ShowDirections(contact), visuals.Colors),
                    ["btn-message"] = ActionButton("\uf369", () => new SendTextMessage(contact.Phone), visuals.Colors, 8),
                    ["btn-dial"] = ActionButton("\uf3f2", () => new DialNumber(contact.Phone), visuals.Colors, 6),
                    ["btn-email"] = ActionButton("\uf1ee", () => new SendEmail(contact.Email), visuals.Colors, 6)
                },
            ["map-container", row: 2, columnSpan: 2] = new Grid
            {
                ["no-map-message"] = new StyledFrame(visuals.Colors)
                {
                    Margin = 16,
                    VerticalOptions = LayoutOptions.Center,
                    Content = new StackLayout
                    {
                        Spacing = 20,
                        ["info"] =
                            new SmallLabel(visuals)
                            {
                                Text = "No map is available because this person does not have an address.",
                                VerticalTextAlignment = TextAlignment.Center
                            },
                        ["suggestion"] = new SmallLabel(visuals)
                        {
                            Text = "Enter an address on the edit screen to see this person's location on a map.",
                            VerticalTextAlignment = TextAlignment.Center
                        }
                    }
                },
                ["map-container"] = new AbsoluteLayout
                {
                    IsEnabled = !string.IsNullOrWhiteSpace(contact.AddressString),
                    IsVisible = !string.IsNullOrWhiteSpace(contact.AddressString),
                    ["spinner", (0, 0, 1, 1), AbsoluteLayoutFlags.All] = new ActivityIndicator
                    {
                        IsRunning = position == null,
                        IsVisible = position == null,
                    },
                    ["map", (0, 0, 1, 1), AbsoluteLayoutFlags.All] = new Map
                    {
                        IsVisible = position != null,
                        VisibleRegion = position == null ? null : MapSpan.FromCenterAndRadius(new Position(position.Value.Latitude, position.Value.Longitude), Distance.FromMiles(10)),
                        Pins = 
                        {
                            ["user"] = position == null ? null : new Pin 
                            {
                                Type = PinType.Place,
                                Position = position.Value,
                                Label = contact.DisplayName,
                                Address = contact.AddressString
                            }
                        }
                    }
                }
            }
        };
        
        public static VisualElement<Xamarin.Forms.ContentPage> Page(Contact contact, Visuals visuals) => Element.WithContext(ctx =>
        {
            var (position, positionAvailable) = ctx.UseLocalState((Position?) null);
            
            if (position == null)
            {
                Task.Run(async () =>
                {
                    var location = (await Xamarin.Essentials.Geocoding.GetLocationsAsync(contact.AddressString)).FirstOrDefault();
                    ctx.Send(positionAvailable(new Position(location.Latitude, location.Longitude)));
                });
            }
            
            return new ContentPage
            {
                Title = contact.DisplayName,
                BackgroundColor = visuals.Colors.WindowBackgroundColor,
                ToolbarItems =
                {
                    ["delete"] = new ToolbarItem
                    {
                        IconImageSource = new FontImageSource {FontFamily = IconFont.Name, Glyph = "\uf1c0"},
                        Clicked = () => new DeleteContact(contact)
                    },
                    ["edit"] = new ToolbarItem
                    {
                        IconImageSource = new FontImageSource {FontFamily = IconFont.Name, Glyph = "\ufda5"},
                        Clicked = () => new ShowContactEditor(contact)
                    }
                },
                Content = Content(contact, position, visuals)
            };
        });
    }
}