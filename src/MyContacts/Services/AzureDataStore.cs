#define LOCALWRITES

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using MyContacts.Extensions;
using MyContacts.Interfaces;
using MyContacts.Shared.Models;
using MyContacts.Util;
using MyContacts.Utils;
using Xamarin.Essentials;

namespace MyContacts.Services
{
    public class AzureDataStore : IDataSource<Contact>
    {
        HttpClient client;
        List<Contact> contacts;
        public static string BackendUrl = "https://mycontactsapi20200107080452.azurewebsites.net";

        //public static string BackendUrl = 
        //    DeviceInfo.Platform == DevicePlatform.Android ? "http://10.0.2.2:5000" : "http://localhost:5000";


        public AzureDataStore()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri($"{BackendUrl}/");

            contacts = new List<Contact>();
        }

        bool IsConnected => Connectivity.NetworkAccess == NetworkAccess.Internet;
        public async Task<IEnumerable<Contact>> GetItems()
        {

            Settings.LastUpdate = DateTime.UtcNow;

#if LOCALWRITES
            if (contacts.Any())
                return contacts;
#endif
            if (IsConnected)
            {
                var json = await client.GetStringAsync($"api/Contacts");
                contacts = await Task.Run(() => JsonSerializer.Deserialize<List<Contact>>(json));
            }
            else
            {
                await OfflineAlert();
            }


            return contacts;
        }

        public async Task<Contact> GetItem(string id)
        {

            Settings.LastUpdate = DateTime.UtcNow;

#if LOCALWRITES
            var c = contacts.FirstOrDefault(c => c.Id == id);
            if (c != null)
                return c;
#endif

            if (!IsConnected)
            {
                await OfflineAlert();
                return null;
            }

            if (id != null)
            {
                var json = await client.GetStringAsync($"api/Contacts/{id}");
                return await Task.Run(() => JsonSerializer.Deserialize<Contact>(json));
            }


            return null;
        }

        public async Task<bool> AddItem(Contact contact)
        {
            if (contact == null)
                return false;

            Settings.LastUpdate = DateTime.UtcNow;

            var serializedContact = JsonSerializer.Serialize(contact);

#if LOCALWRITES
            contacts.Add(contact.With(id: Guid.NewGuid().ToString()));

            await Dialogs.Alert(new AlertInfo
            {
                Cancel = "OK",
                Title = "Local Only Mode",
                Message = "Currently running in local write mode. Data will not be sent to the server."
            });

            return true;
#endif

            if (!IsConnected)
            {
                await OfflineAlert();
                return false;
            }

            var response = await client.PostAsync($"api/Contacts", new StringContent(serializedContact, Encoding.UTF8, "application/json"));            
            if(response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var c = JsonSerializer.Deserialize<Contact>(json);
                contact = contact.With(id: c.Id);
                return true;
            }

            return false;
        }

        public async Task<bool> UpdateItem(Contact contact)
        {
            if (contact == null || contact.Id == null)
                return false;

            Settings.LastUpdate = DateTime.UtcNow;

#if LOCALWRITES

            var existing = contacts.FirstOrDefault(c => c.Id == contact.Id);
            if (existing == null)
                return false;

            await LocalOnlyModeAlert();

            return true;
#endif

            if(!IsConnected)
            {
                await OfflineAlert();
                return false;
            }

            var serializedContact = JsonSerializer.Serialize(contact);

            var response = await client.PutAsync($"api/Contacts", new StringContent(serializedContact, Encoding.UTF8, "application/json"));


            return response.IsSuccessStatusCode;
        }


        public async Task<bool> RemoveItem(Contact contact)
        {
            var id = contact?.Id;

            if (string.IsNullOrEmpty(id) && !IsConnected)
                return false;

            Settings.LastUpdate = DateTime.UtcNow;

#if LOCALWRITES

            contacts.Add(contact);

            await Dialogs.Alert(new AlertInfo
            {
                Cancel = "OK",
                Title = "Local Only Mode",
                Message = "Currently running in local write mode. Data will not be sent to the server."
            });

            return true;
#endif

            var response = await client.DeleteAsync($"api/Contacts/{id}");


            return response.IsSuccessStatusCode;
        }  
        
        Task LocalOnlyModeAlert() => Dialogs.Alert(new AlertInfo
        {
            Cancel = "OK",
            Title = "Local Only Mode",
            Message = "Currently running in local write mode. Data will not be sent to the server."
        });

        Task OfflineAlert() => Dialogs.Alert(new AlertInfo
        {
            Cancel = "OK",
            Title = "Offline",
            Message = "Currently offline, please check internet connection."
        });

    }
}
