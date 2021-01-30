using System;
using System.Linq;
using System.Threading.Tasks;
using Laconic;
using MyContacts.Interfaces;
using MyContacts.Laconic;
using MyContacts.Services;
using MyContacts.Shared.Models;
using MyContacts.Views;
using Xamarin.Essentials;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using xf = Xamarin.Forms;

namespace MyContacts
{
    enum SelectedTheme
    {
        Phone,
        Light,
        Dark
    }
    
    public class App : xf.Application
    {
        public static bool UseLocalDataSource = true;

        static SelectedTheme[] GetAvailableThemeOptions()
        {
            var minDefaultVersion = new Version(13, 0);
            if (DeviceInfo.Platform == DevicePlatform.UWP)
                minDefaultVersion = new Version(10, 0, 17763, 1);
            else if (DeviceInfo.Platform == DevicePlatform.Android)
                minDefaultVersion = new Version(10, 0);

            return DeviceInfo.Version >= minDefaultVersion 
                ? new[] {SelectedTheme.Phone, SelectedTheme.Light, SelectedTheme.Dark} 
                : new[] {SelectedTheme.Light, SelectedTheme.Dark};
        }
        
        static NamedSizes GetNamedSizes() => new NamedSizes(
            xf.Device.GetNamedSize(xf.NamedSize.Large, typeof(Label)),
            xf.Device.GetNamedSize(xf.NamedSize.Medium, typeof(Label)),
            xf.Device.GetNamedSize(xf.NamedSize.Small, typeof(Label)),
            xf.Device.GetNamedSize(xf.NamedSize.Micro, typeof(Label))
        );

        readonly Binder<State> _binder;
        
        public App()
        {
            IconFont.Name = Xamarin.Forms.Device.RuntimePlatform == Xamarin.Forms.Device.iOS
                ? "Material Design Icons"
                : "materialdesignicons-webfont.ttf#Material Design Icons";
            
            xf.DependencyService.Register<FileDataSource>();

            var deviceTheme = AppInfo.RequestedTheme;
            var selectedTheme = (SelectedTheme)Enum.Parse(
                typeof(SelectedTheme), 
                Preferences.Get(nameof(SelectedTheme), nameof(SelectedTheme.Phone)), 
                true
            );

            _binder = Binder.Create(
                State.Initial(GetNamedSizes(), GetAvailableThemeOptions(), deviceTheme, selectedTheme),
                State.MainReducer
            );

            var listPage = _binder.CreateElement(ContactList.Page);
            
            xf.NavigationPage.SetBackButtonTitle(listPage, "List");
            var navPage = new xf.NavigationPage(listPage)
            {
                BarTextColor = xf.Color.White,
                BarBackgroundColor = _binder.State.Visuals.Colors.PrimaryColor.ToXamarinFormsColor()
            };

            // Navigation
            _binder.UseMiddleware((context, next) =>
            {
                if (!(context.Signal is INavigationSignal nav))
                    return next(context);
                
                Action action = nav switch
                {
                    ShowSettings _ => () =>
                    {
                        var p = _binder.CreateElement(SettingsEditor.Page);
                        p.On<xf.PlatformConfiguration.iOS>().SetModalPresentationStyle(UIModalPresentationStyle.FormSheet);
                        navPage.Navigation.PushModalAsync(p);
                    },
                    CloseSettings _ => () =>
                        navPage.Navigation.PopModalAsync(),
                    ShowAddContact _ => () =>
                        navPage.Navigation.PushAsync(_binder.CreateElement(s => ContactEditor.Page(Contact.New(), s.Visuals))),
                    ShowDetails d => () =>
                    {
                        // Must use current state instead of captured 'c' variable:
                        var p = _binder.CreateElement(s => ContactDetails.Page(s.Contacts.First(x => x.Id == d.Payload.Id), s.Visuals));
                        xf.NavigationPage.SetBackButtonTitle(p, "");
                        navPage.Navigation.PushAsync(p);
                    },
                    ShowContactEditor e => () =>
                        navPage.Navigation.PushAsync(_binder.CreateElement(s => ContactEditor.Page(e.Payload, s.Visuals))),
                    CloseContactEditor _ => () => navPage.Navigation.PopAsync(),
                    _ => () => { }
                };
                // Middleware runs on background thread. Since we're creating and pushing new Xamarin.Forms ContentPages
                // we must dispatch to the main thread:
                xf.Device.BeginInvokeOnMainThread(action);
                return next(context);
            });
            
            // Save selected theme
            _binder.UseMiddleware((context, next) =>
            {
                if (context.Signal is SetTheme t)
                    Preferences.Set(nameof(SelectedTheme), t.Payload.ToString());
                
                return next(context);
            });

            // Show validation messages
            _binder.UseMiddleware((context, next) =>
            {
                if (context.Signal is DisplayAlert a)
                    xf.Device.BeginInvokeOnMainThread(() => { navPage.DisplayAlert(a.Title, a.Message, "OK"); });
                
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
                        var success = string.IsNullOrEmpty(save.Payload.Id)
                            ? await svc.AddItem(save.Payload)
                            : await svc.UpdateItem(save.Payload);

                        if (success)
                        {
                            _binder.Send(new CloseContactEditor());
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

            _binder.UseMiddleware(ExternalAppsMiddleware);

            MainPage = navPage;
            
            _binder.Send(new DataRequested());
        }

        MiddlewareContext<State> ExternalAppsMiddleware(
            MiddlewareContext<State> context, 
            Func<MiddlewareContext<State>, MiddlewareContext<State>> next)
        {
            if (!(context.Signal is IExternalAppRequestSignal ext))
                return next(context);
            
            static string Sanitize(string source) => new string(source.ToCharArray().Where(char.IsDigit).ToArray());

            xf.Device.BeginInvokeOnMainThread(async () =>
            {
                var errorMessage = "";
                try
                {
                    switch (ext)
                    {
                        case ShowDirections d:
                            errorMessage = "Unable to open a map application on the device.";
                            await Map.OpenAsync(new Placemark
                            {
                                AdminArea = d.Payload.State,
                                Locality = d.Payload.City,
                                PostalCode = d.Payload.PostalCode,
                                Thoroughfare = d.Payload.AddressString
                            });
                            break;
                        case SendTextMessage m:
                            errorMessage = "Sms is not supported on this device.";
                            await Sms.ComposeAsync(new SmsMessage(string.Empty, Sanitize(m.Payload)));
                            break;
                        case DialNumber d:
                            errorMessage = "Phone calls are not supported on this device.";
                            PhoneDialer.Open(Sanitize(d.Payload));
                            break;
                        case SendEmail e:
                            errorMessage = "Email is not supported on this device.";
                            await Email.ComposeAsync(string.Empty, string.Empty, e.Payload);
                            break;
                    }
                }
                catch (FeatureNotSupportedException)
                {
                    _binder.Send(new DisplayAlert("Not Supported", errorMessage));
                }
            });
            
            return next(context);
        }

        protected override void OnStart()
        {
            base.OnStart();
            _binder.Send(new DeviceSettingsChanged(AppInfo.RequestedTheme, GetNamedSizes()));
        }
        
        protected override void OnResume()
        {
            base.OnResume();
            _binder.Send(new DeviceSettingsChanged(AppInfo.RequestedTheme, GetNamedSizes()));
        }
    }
}