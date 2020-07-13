using System;
using MyContacts.Shared.Models;
using Laconic;
using System.Collections.Generic;
using xf = Xamarin.Forms;

namespace MyContacts.Laconic
{
    public partial class ContactList : xf.ContentPage
    {
        static Frame ContactCard(Contact contact, Visuals visuals) => new StyledFrame(visuals.Colors)
        {
            Margin = new xf.Thickness(12, 6),
            Visual = xf.VisualMarker.Material,
            HasShadow = true,
            BackgroundColor = visuals.Colors.FrameBackgroundColor,
            BorderColor = visuals.Colors.FrameBorderColor,
            GestureRecognizers = { ["tap"] = new TapGestureRecognizer { Tapped = () => new Signal("showDetails", contact) } },
            Content = new Grid {
                ColumnSpacing = 12,
                ColumnDefinitions = "100, *",
                ["photo"] = new PancakeView {
				    CornerRadius = new xf.CornerRadius(40, 15, 15, 40),
				    HeightRequest = 100,
				    HorizontalOptions = xf.LayoutOptions.Center,
				    IsClippedToBounds = true,
				    BackgroundColor = visuals.Colors.FrameBackgroundColor,
				    BorderColor = visuals.Colors.SystemGray2,
				    BorderThickness = 3,
				    VerticalOptions = xf.LayoutOptions.Center,
				    WidthRequest = 100,
				    Content = new Image {Aspect = xf.Aspect.AspectFill, Source = contact.SmallPhotoUrl}
			    },
                ["text", column: 1] = new StackLayout {
                    VerticalOptions = xf.LayoutOptions.Center,
                    ["firstName"] = new LargeLabel(visuals) {Text = contact.DisplayLastNameFirst},
                    ["company"] = new SmallLabel(visuals) {Text = contact.Company},
                    ["jobTitle"] = new MicroLabel(visuals) { Text = contact.JobTitle, TextColor = visuals.Colors.AccentColor},
                },
            }
        };

        static CollectionView List(IEnumerable<Contact> contacts, Visuals visuals) => new CollectionView
        {
            ItemSizingStrategy = xf.ItemSizingStrategy.MeasureFirstItem,
            Items = contacts.ToItemsList(_ => "contactCard", c => c.Id, c => ContactCard(c, visuals))
        };

        static Frame SearchBar(Colors colors) => new StyledFrame(colors)
        {
            Margin = new xf.Thickness(12, 12, 12, 0),
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
            HorizontalOptions = xf.LayoutOptions.End,
            BackgroundColor = colors.PrimaryColor,
            VerticalOptions = xf.LayoutOptions.End,
            WidthRequest = 56,
            Content = new ImageButton
            {
                Padding = 12,
                Clicked = () => new Signal("addContact"),
                HorizontalOptions = xf.LayoutOptions.FillAndExpand,
                BackgroundColor = colors.PrimaryColor,
                VerticalOptions = xf.LayoutOptions.FillAndExpand,
                Source = new xf.FontImageSource
                {
                    FontFamily = "Material Design Icons", Glyph = "\uf415", Color = xf.Color.White
                }
            }
        };

        public static ContentPage Page(State state) => new ContentPage
        {
            Title = "My Contacts",
            BackgroundColor = state.Visuals.Colors.WindowBackgroundColor,
            Appearing = () => new DataRequested(),
            ToolbarItems =
            {
                ["settings"] = new ToolbarItem
                {
                    Clicked = () => new Signal("showSettings"),
                    IconImageSource = new xf.FontImageSource
                    {
						// TODO: Android
                        FontFamily = "Material Design Icons", Glyph = "\uF493"
                    }
                }
            },
            Content = new Grid
            {
                RowDefinitions = "Auto,*",
                ["search"] = SearchBar(state.Visuals.Colors),
                ["list", row: 1] = new RefreshView
                {
                    IsRefreshing = state.IsFetchingData,
                    Refreshing = () => new DataRequested(),
                    Content = List(state.Contacts, state.Visuals),
                    RefreshColor = state.Visuals.Colors.SystemGray
                },
                ["fab", rowSpan: 2] = FloatingButton(state.Visuals.Colors)
            }
        };
    }
}