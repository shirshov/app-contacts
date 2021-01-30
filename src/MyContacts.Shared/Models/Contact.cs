using System;
using Newtonsoft.Json;

namespace MyContacts.Shared.Models
{
   public partial record Contact(string Id, string FirstName, string LastName, 
	   string Company, string JobTitle, string Phone, string Email,
   string Street, string City, string State, string PostalCode, string PhotoUrl);

    partial record Contact
    {
        public static Contact New() => new(null, "", "", "", "", "", "", "", "", "", "", "https://raw.githubusercontent.com/jamesmontemagno/app-monkeys/master/douc.jpg");

        [JsonIgnore]
        public string SmallPhotoUrl => PhotoUrl;
        
        [JsonIgnore]
        public string AddressString =>
            $"{Street} {(!string.IsNullOrWhiteSpace(City) ? City + "," : "")} {State} {PostalCode}";
         
        [JsonIgnore]
        public string DisplayName => ToString();
         
        [JsonIgnore]
        public string DisplayLastNameFirst => $"{LastName}, {FirstName}";
        
        [JsonIgnore]
        public string StatePostal => State + " " + PostalCode;
        
        public override string ToString() => $"{FirstName} {LastName}";
    }
}

// Without this dummy class C#9's records won't compile
namespace System.Runtime.CompilerServices
{
    public static class IsExternalInit
    {
    }
}

