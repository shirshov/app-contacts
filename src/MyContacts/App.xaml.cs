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

        readonly Binder<State> _binder;

		static (double, double, double, double) GetNamedSizes() => (
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

            var listPage = _binder.CreatePage(ContactList.Page);
            xf.NavigationPage.SetBackButtonTitle(listPage, "List");
            var navPage = new xf.NavigationPage(listPage) { BarTextColor = xf.Color.White };
            navPage.SetDynamicResource(xf.NavigationPage.BarBackgroundColorProperty, "PrimaryColor");

            // Navigation
			_binder.UseMiddleware((context, next) => {
			    Action action = context.Signal switch {
                    ("showSettings", _) => () => navPage.Navigation.PushModalAsync(new SettingsPage(_binder)),
                    ("closeSettings", _) => () => navPage.Navigation.PopModalAsync(),
				    ("addContact", _) => () => navPage.Navigation.PushAsync(new EditPage()),
				    ("showDetails", Contact c) => () => navPage.Navigation.PushAsync(new DetailPage(c)),
					_ => () => {}
				};
                action();
                return next(context);
			});
            
            // Data retrieval
            _binder.UseMiddleware((context, next) => 
            {
                if (context.Signal is DataRequested & !context.State.IsFetchingData)
                {
                    Task.Run(async () =>
                    {
                        var svc = xf.DependencyService.Get<IDataSource<Contact>>();
                        await Task.Delay(1000);
                        var items = await svc.GetItems();
                        xf.Device.BeginInvokeOnMainThread(() => _binder.Dispatch(new DataReceived(items)));
                    });
                }
                return next(context);
            });

            // Handle change theme request:
            _binder.UseMiddleware((context, next) =>
            {
                context = next(context);

                if (context.Signal is SetThemeSignal t)
                {
                     var newTheme = context.State.Visuals.Themes.Length == 3 ? t.Payload : t.Payload + 1;
                     Settings.ThemeOption = newTheme;
                    ThemeHelper.ChangeTheme(t.Payload);

                    return context.WithState(
                        new State(
							context.State.IsFetchingData,
                            context.State.Contacts,
                            new Visuals(new Colors(Resources), GetNamedSizes(), context.State.Visuals.Themes, newTheme)
                        )
                    );
                }

                return context;
            });

            MainPage = navPage;
			_binder.Dispatch(new DataRequested());
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