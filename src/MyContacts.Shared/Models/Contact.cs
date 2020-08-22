using System;
using System.Text.Json.Serialization;
using Laconic.CodeGeneration;

namespace MyContacts.Shared.Models
{
    [Records]
    public interface Records
    {
       record Contact(string id, string firstName, string lastName, 
           string company, string jobTitle, 
           string phone, string email,
           string street, string city, string state, string postalCode,
           string photoUrl);
    }

    partial class Contact
    {
        public static Contact New() => new Contact(null, "", "", "", "", "", "", "", "", "", "", "https://raw.githubusercontent.com/jamesmontemagno/app-monkeys/master/douc.jpg");

        [JsonIgnore]
        public string SmallPhotoUrl => PhotoUrl;
        
        [JsonIgnore]
        public string AddressString => string.Format(
             "{0} {1} {2} {3}",
             Street,
             !string.IsNullOrWhiteSpace(City) ? City + "," : "",
             State,
             PostalCode);
         
        [JsonIgnore]
        public string DisplayName => ToString();
         
        [JsonIgnore]
        public string DisplayLastNameFirst => $"{LastName}, {FirstName}";
        
        [JsonIgnore]
        public string StatePostal => State + " " + PostalCode;
        
        public override string ToString() => $"{FirstName} {LastName}";
    }
}

