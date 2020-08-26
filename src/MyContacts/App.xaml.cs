using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Laconic;
using MyContacts.Interfaces;
using MyContacts.Laconic;
using MyContacts.Models;
using MyContacts.Services;
using MyContacts.Shared.Models;
using MyContacts.Styles;
using MyContacts.Util;
using MyContacts.Views;
using xf = Xamarin.Forms;

[assembly: xf.Xaml.XamlCompilation(xf.Xaml.XamlCompilationOptions.Compile)]

namespace MyContacts
{
    public partial class App : xf.Application
    {
        public static bool UseLocalDataSource = true;

        static (string[], Theme) GetThemeList()
        {
            var list = new List<string> {"Light", "Dark"};

            if (Settings.HasDefaultThemeOption)
                list.Insert(0, "Device Default");

            var savedThemeIndex = (int) Settings.ThemeOption;
            return (list.ToArray(), (Theme) (list.Count == 3 ? savedThemeIndex : savedThemeIndex - 1));
        }

        internal readonly Binder<State> _binder;

        static NamedSizes GetNamedSizes() => new NamedSizes(
            xf.Device.GetNamedSize(xf.NamedSize.Large, typeof(Label)),
            xf.Device.GetNamedSize(xf.NamedSize.Medium, typeof(Label)),
            xf.Device.GetNamedSize(xf.NamedSize.Small, typeof(Label)),
            xf.Device.GetNamedSize(xf.NamedSize.Micro, typeof(Label))
        );
        
        public App()
        {
            InitializeComponent();

            ThemeHelper.ChangeTheme(Settings.ThemeOption, true);

            if (UseLocalDataSource)
                xf.DependencyService.Register<FileDataSource>();
            else
                xf.DependencyService.Register<AzureDataStore>();

            var (themes, selectedTheme) = GetThemeList();
            _binder = Binder.Create(
                new State(
                    false,
                    new Contact[0],
                    new Visuals(new Colors(Resources), GetNamedSizes(), themes, selectedTheme)
                ),
                State.MainReducer
            );

            var listPage = _binder.CreateElement(ContactList.Page);
            xf.NavigationPage.SetBackButtonTitle(listPage, "List");
            var navPage = new xf.NavigationPage(listPage) {BarTextColor = xf.Color.White};
            navPage.SetDynamicResource(xf.NavigationPage.BarBackgroundColorProperty, "PrimaryColor");

            // Navigation
            _binder.UseMiddleware((context, next) =>
            {
                Action action = context.Signal switch
                {
                    ("showSettings", _) => () =>
                        navPage.Navigation.PushModalAsync(_binder.CreateElement(s => SettingsEditor.Page(s.Visuals))),
                    ("closeSettings", _) => () =>
                        navPage.Navigation.PopModalAsync(),
                    ("showAddContact", _) => () =>
                        navPage.Navigation.PushAsync(_binder.CreateElement(s => ContactEditor.Page(s.Visuals, Contact.New()))),
                    ("showDetails", Contact c) => () =>
                        navPage.Navigation.PushAsync(new DetailPage(c)),
                    ("closeContactEditor", _) => () => navPage.Navigation.PopAsync(),
                    _ => () => { }
                };
                // Middleware runs on background thread. Since we're creating and pushing new Xamarin.Forms ContentPages
                // we must dispatch to the main thread:
                xf.Device.BeginInvokeOnMainThread(action);
                return next(context);
            });
            
            // Show validation messages
            _binder.UseMiddleware((context, next) =>
            {
                if (context.Signal is DisplayAlert a)
                {
                    xf.Device.BeginInvokeOnMainThread(() => { navPage.DisplayAlert(a.Title, a.Message, "OK"); });
                    return context;
                }
                return next(context);
            });
            
            // Data retrieval and update
            _binder.UseMiddleware((context, next) =>
            {
                var svc = xf.DependencyService.Get<IDataSource<Contact>>();
                if (context.Signal is DataRequested & !context.State.IsFetchingData)
                {
                    Task.Run(async () =>
                    {
                        await Task.Delay(1000);
                        var items = await svc.GetItems();
                        _binder.Send(new DataReceived(items));
                    });
                }
                else if (context.Signal is SaveContact save)
                {
                    Task.Run(async () =>
                    {
                        var success = string.IsNullOrEmpty(save.Contact.Id)
                            ? await svc.AddItem(save.Contact)
                            : await svc.UpdateItem(save.Contact);

                        if (success)
                        {
                            _binder.Send(new Signal("closeContactEditor"));
                            _binder.Send(new DataRequested());
                        }
                        else
                        {
                            // This signal is not used anywhere, it's here purely for demonstration purposes
                            new Signal("SaveError", save.Payload);
                        }
                    });
                }

                return next(context);
            });

            // Handle change theme request:
            _binder.UseMiddleware((context, next) =>
            {
                context = next(context);

                if (context.Signal is SetTheme t)
                {
                    xf.Device.BeginInvokeOnMainThread(() =>
                    {
                        var newTheme = context.State.Visuals.Themes.Length == 3 ? t.Theme : t.Theme + 1;
                        Settings.ThemeOption = newTheme;
                        ThemeHelper.ChangeTheme(newTheme);
                        var colors = new Colors(Resources);
                        System.Diagnostics.Debug.WriteLine($"THEME UPDATED: {colors.FrameBackgroundColor}");
                        _binder.Send(new ThemeUpdated(newTheme, new Colors(Resources), GetNamedSizes()));
                    });
                }

                return context;
            });

            MainPage = navPage;
            _binder.Send(new DataRequested());
        }

        protected override void OnStart()
        {
            base.OnStart();
            ThemeHelper.ChangeTheme(Settings.ThemeOption, true);
        }

        protected override void OnResume()
        {
            base.OnResume();
            ThemeHelper.ChangeTheme(Settings.ThemeOption, true);
        }
    }
}