using System;
using System.Linq;
using Laconic;
using MyContacts.Shared.Models;

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
                            Source = new FontImageSource {
                                FontFamily = IconFont.Name,
                                Glyph = row.IconGlyph,
                                Color = visuals.Colors.SystemGray2
                            },
                            HeightRequest = 36,
                            WidthRequest = 36,
                            VerticalOptions = LayoutOptions.Center
                        },
                        row: i + 1);
                }

                grid.Children.Add(
                    i,
                    new StyledEntry(visuals.Colors)
                    {
                        Text = row.Text,
                        Keyboard = Keyboard.Text,
                        Placeholder = row.Placeholder,
                        TextChanged = e => row.Update(e.NewTextValue)
                    },
                    row: i + 1,
                    column: 1
                );
            }

            return grid;
        }

        public static VisualElement<Xamarin.Forms.ContentPage> Page(Visuals visuals, Contact initial) => Element.WithContext( ctx => {
            var (contact, setter) = ctx.UseLocalState(initial);

            return new ContentPage
            {
                Title = String.IsNullOrEmpty(contact.Id) ? "New Contact" : "Edit Contact",
                BackgroundColor = visuals.Colors.WindowBackgroundColor,
                ToolbarItems =
                {
                    ["save"] = new ToolbarItem
                    {
                        IconImageSource = new FontImageSource
                        {
                            FontFamily = IconFont.Name,
                            Glyph = "\uf193",
                            Color = Color.White
                        },
                        Clicked = () => Proceed(contact)
                    }
                },
                Content = new ScrollView { Content = new StackLayout 
                {
                    Padding = 12,
                    ["name"] = Section(visuals, "Name",
                        (contact.FirstName, text => setter(contact.With(firstName: text)), "\uf004", "First name"),
                        (contact.LastName, text => setter(contact.With(lastName: text)), null, "Last name")
                    ),
                    ["employment"] = Section(visuals, "Employment",
                        (contact.Company, text => setter(contact.With(company: text)), "\uf990", "Company"),
                        (contact.JobTitle, text => setter(contact.With(jobTitle: text)), null, "Title")),
                    ["contact"] = Section(visuals, "Contact",
                        (contact.Phone, text => setter(contact.With(phone: text)), "\uf3f2", "Phone number"),
                        (contact.Email, text => setter(contact.With(email: text)), "\uf1ee", "Email address")),
                    ["address"] = Section(visuals, "Address",
                        (contact.Street, text => setter(contact.With(street: text)), "\uf34d", "Street"),
                        (contact.City, text => setter(contact.With(city: text)), null, "City"),
                        (contact.State, text => setter(contact.With(state: text)), null, "State"),
                        (contact.PostalCode, text => setter(contact.With(postalCode: text)), null, "Zip code")),
                }}
            };
        });

        static Signal Proceed(Contact contact)
        {
            if (new [] {contact.LastName, contact.FirstName}.Any(x => string.IsNullOrEmpty(x.Trim())))
                return new DisplayAlert("Invalid name!", "A Contact must have both a first and last name.");

            var validAddress = new[] {contact.Street, contact.City, contact.State}.All(x => !string.IsNullOrEmpty(x.Trim()))
                || !string.IsNullOrEmpty(contact.PostalCode);

            if (!validAddress)
                return new DisplayAlert(
                    "Invalid address!",
                    "You must enter either a street, city, and state combination, or a postal code.");

            return new SaveContact(contact);
        }
    }
}
