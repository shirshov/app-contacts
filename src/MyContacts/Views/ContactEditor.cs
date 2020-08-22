using System;
using Laconic;
using MyContacts.Shared.Models;
using xf = Xamarin.Forms;

namespace MyContacts.Laconic
{
    public static class ContactEditor
    {
        static Grid Section(Visuals visuals, string sectionName,
            params (string Text, Func<string, Signal> Update, string IconGlyph, string Placeholder)[] rows)
        {
            var grid = new Grid
            {
                ColumnDefinitions = "Auto, *",
                RowDefinitions = "Auto, Auto, Auto",
                ["title", columnSpan: 2] = new LargeLabel(visuals) {Text = sectionName},
            };

            for (var i = 0; i < rows.Length; i++)
            {
                var row = rows[i];
                if (row.IconGlyph != null)
                {
                    grid.Children.Add(
                        "image" + i,
                        new Image
                        {
                            Source = ImageSource.FromFont(row.IconGlyph, visuals.Colors.SystemGray2),
                            HeightRequest = 36,
                            WidthRequest = 36,
                            VerticalOptions = xf.LayoutOptions.Center
                        },
                        row: i + 1);
                }

                grid.Children.Add(
                    i,
                    new StyledEntry(visuals.Colors)
                    {
                        Text = row.Text,
                        Keyboard = xf.Keyboard.Text,
                        Placeholder = row.Placeholder,
                        TextChanged = e => row.Update(e.NewTextValue)
                    },
                    row: i + 1,
                    column: 1
                );
            }

            return grid;
        }

        public static VisualElement<xf.ContentPage> Page(Visuals visuals, Contact initial) => Element.WithContext(ctx => {
            var (state, setter) = ctx.UseLocalState(initial);
            
            return new ContentPage
            {
                Title = String.IsNullOrEmpty(state.Id) ? "New Contact" : "Edit Contact",
                BackgroundColor = visuals.Colors.WindowBackgroundColor,
                ToolbarItems =
                {
                    ["save"] = new ToolbarItem
                    {
                        IconImageSource = ImageSource.FromFont("\uf193", xf.Color.White),
                        Clicked = () => new SaveContact(state)
                    }
                },
                Content = new ScrollView {Content = new StackLayout
                {
                    Padding = 12,
                    ["name"] = Section(visuals, "Name",
                        (state.FirstName, text => setter(state.With(firstName: text)), "\uf004", "First name"),
                        (state.LastName, text => setter(state.With(lastName: text)), null, "Last name")
                    ),
                    ["employment"] = Section(visuals, "Employment",
                        (state.Company, text => setter(state.With(company: text)), "\uf990", "Company"),
                        (state.JobTitle, text => setter(state.With(jobTitle: text)), null, "Title")),
                    ["contact"] = Section(visuals, "Contact",
                        (state.Phone, text => setter(state.With(phone: text)), "\uf3f2", "Phone number"),
                        (state.Email, text => setter(state.With(email: text)), "\uf1ee", "Email address")),
                    ["address"] = Section(visuals, "Address",
                        (state.Street, text => setter(state.With(street: text)), "\uf34d", "Street"),
                        (state.City, text => setter(state.With(city: text)), null, "City"),
                        (state.State, text => setter(state.With(state: text)), null, "State"),
                        (state.PostalCode, text => setter(state.With(postalCode: text)), null, "Zip code")),
                }}
            };
        });
    }
}
