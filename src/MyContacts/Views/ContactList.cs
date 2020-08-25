using MyContacts.Shared.Models;
using Laconic;
using System.Collections.Generic;
using System.Linq;

namespace MyContacts.Laconic
{
    public static class ContactList
    {
        static Frame ContactCard(Contact contact, Visuals visuals) => new StyledFrame(visuals.Colors)
        {
            Margin = (12, 6),
            Visual = VisualMarker.Material,
            HasShadow = true,
            BackgroundColor = visuals.Colors.FrameBackgroundColor,
            BorderColor = visuals.Colors.FrameBorderColor,
            GestureRecognizers = {["tap"] = new TapGestureRecognizer
            {
                Tapped = () => new Signal("showDetails", contact)
            }},
            Content = new Grid
            {
                ColumnSpacing = 12,
                ColumnDefinitions = "100, *",
                ["photo"] =
                    new PancakeView
                    {
                        CornerRadius = (40, 15, 15, 40),
                        HeightRequest = 100,
                        HorizontalOptions = LayoutOptions.Center,
                        IsClippedToBounds = true,
                        BackgroundColor = visuals.Colors.FrameBackgroundColor,
                        BorderColor = visuals.Colors.SystemGray2,
                        BorderThickness = 3,
                        VerticalOptions = LayoutOptions.Center,
                        WidthRequest = 100,
                        Content = new Image {Aspect = Aspect.AspectFill, Source = contact.SmallPhotoUrl}
                    },
                ["text", column: 1] = new StackLayout
                {
                    VerticalOptions = LayoutOptions.Center,
                    ["firstName"] = new LargeLabel(visuals) {Text = contact.DisplayLastNameFirst},
                    ["company"] = new SmallLabel(visuals) {Text = contact.Company},
                    ["jobTitle"] =
                        new MicroLabel(visuals) {Text = contact.JobTitle, TextColor = visuals.Colors.AccentColor},
                },
            }
        };

        static CollectionView List(IEnumerable<Contact> contacts, Visuals visuals)
        {
            System.Diagnostics.Debug.WriteLine($"LIST: {contacts.Count()}");
            
            return new CollectionView
            {
                ItemSizingStrategy = ItemSizingStrategy.MeasureFirstItem,
                Items = contacts.ToItemsList(_ => "contactCard", c => c.Id, c => ContactCard(c, visuals))
            };
        }

        static Frame SearchBar(Colors colors) => new StyledFrame(colors)
        {
            Margin = (12, 12, 12, 0),
            Padding = 0,
            Content = new SearchBar
            {
                BackgroundColor = colors.SystemGray4,
                CancelButtonColor = colors.SystemGray,
                Placeholder = "Search Contacts",
                PlaceholderColor = colors.SystemGray2,
                TextColor = colors.SystemGray
            }
        };

        static Frame FloatingButton(Colors colors) => new StyledFrame(colors)
        {
            Margin = 8,
            Padding = 0,
            CornerRadius = 28,
            HasShadow = true,
            HeightRequest = 56,
            HorizontalOptions = LayoutOptions.End,
            BackgroundColor = colors.PrimaryColor,
            VerticalOptions = LayoutOptions.End,
            WidthRequest = 56,
            Content = new ImageButton
            {
                Padding = 12,
                Clicked = () => new Signal("showAddContact"),
                HorizontalOptions = LayoutOptions.FillAndExpand,
                BackgroundColor = colors.PrimaryColor,
                VerticalOptions = LayoutOptions.FillAndExpand,
                Source = new FontImageSource { FontFamily = IconFont.Name, Glyph = "\uf415", Color = Color.White}
            }
        };

        public static ContentPage Page(State state) => new ContentPage
        {
            Title = "My Contacts",
            BackgroundColor = state.Visuals.Colors.WindowBackgroundColor,
            ToolbarItems =
            {
                ["settings"] = new ToolbarItem
                {
                    Clicked = () => new Signal("showSettings"),
                    IconImageSource = new FontImageSource { FontFamily = IconFont.Name, Glyph = "\uF493", Color = Color.White}
                }
            },
            Content = new Grid
            {
                RowDefinitions = "Auto,*",
                ["search"] = SearchBar(state.Visuals.Colors),
                ["list", row: 1] = new RefreshView
                {
                    IsRefreshing = state.IsFetchingData,
                    Refreshing = e => e.IsRefreshing ? new DataRequested() : null,
                    Content = List(state.Contacts, state.Visuals),
                    RefreshColor = state.Visuals.Colors.SystemGray
                },
                ["fab", rowSpan: 2] = FloatingButton(state.Visuals.Colors)
            }
        };
    }
}