﻿using System;
using System.Collections.Generic;
using System.Text;
using MyContacts.Shared.Models;

namespace MyContacts.Shared.Utils
{
    public static class ContactsGenerator
    {

        public static List<Contact> GenerateContacts()
        {
            return new List<Contact>()
            {
                new Contact ( id : "00004363-F79A-44E7-BC32-6128E2EC8401", firstName : "Joseph", lastName : "Grimes", company : "GG Mechanical", jobTitle : "Vice President", email : "jgrimes@ggmechanical.com", phone : "414-367-4348", street : "2030 Judah St", city : "San Francisco", postalCode : "94144", state : "CA", photoUrl : $"{Constants.BaseImagePath}/josephgrimes.jpg" ),
                new Contact ( id : "c227bfd2-c6f6-49b5-93ec-afef9eb18d08", firstName : "Monica", lastName : "Green", company : "Calcom Logistics", jobTitle : "Director", email : "mgreen@calcomlogistics.com", phone : "925-353-8029", street : "230 3rd Ave", city : "San Francisco", postalCode : "94118", state : "CA", photoUrl : $"{Constants.BaseImagePath}/monicagreen.jpg" ),
                new Contact ( id : "31bf6fe5-18f1-4354-9571-2cdecb0c00af", firstName : "Joan", lastName : "Mancum", company : "Bay Unified School District", jobTitle : "Principal", email : "joan.mancum@busd.org", phone : "914-870-7670", street : "448 Grand Ave", city : "South San Francisco", postalCode : "94080", state : "CA", photoUrl : $"{Constants.BaseImagePath}/joanmancum.jpg" ),
                new Contact ( id : "45d2ddc0-a8e9-4aea-8b51-2860c708e30d", firstName : "Alvin", lastName : "Gray", company : "Pacific Cabinetry", jobTitle : "Office Manager", email : "agray@pacificcabinets.com", phone : "720-344-7823", street : "1773 Lincoln St", city : "Santa Clara", postalCode : "95050", state : "CA", photoUrl : $"{Constants.BaseImagePath}/alvingray.jpg" ),
                new Contact ( id : "c9ebe513-0db2-41d3-b595-20a49454a421", firstName : "Michelle", lastName : "Wilson", company : "Evergreen Mechanical", jobTitle : "Sales Manager", email : "mwilson@evergreenmech.com", phone : "917-245-7975", street : "208 Jackson St", city : "San Jose", postalCode : "95112", state : "CA", photoUrl : $"{Constants.BaseImagePath}/michellewilson.jpg" ),
                new Contact ( id : "e4029998-5d6e-4ed8-b802-e6f940f307a1", firstName : "Jennifer", lastName : "Gillespie", company : "Peninsula University", jobTitle : "Superintendent", email : "jgillespie@peninsula.org", phone : "831-427-6746", street : "10002 N De Anza Blvd", city : "Cupertino", postalCode : "95014", state : "CA", photoUrl : $"{Constants.BaseImagePath}/jennifergillespie.jpg" ),
                new Contact ( id : "2323e8b6-ed1c-44fe-9cff-90dcd97d3bb5", firstName : "Thomas", lastName : "White", company : "Creative Automotive Group", jobTitle : "Service Manager", email : "tom.white@creativeauto.com", phone : "214-865-0771", street : "1181 Linda Mar Blvd", city : "Pacifica", postalCode : "94044", state : "CA", photoUrl : $"{Constants.BaseImagePath}/thomaswhite.jpg" ),
                new Contact ( id : "00F8A566-2538-4AF7-AE10-997B61537DC0", firstName : "Leon", lastName : "Muks", company : "Spacey", jobTitle : "President", email : "leon.muks@spacey.io", phone : "310-586-0181", street : "2518 Durant Ave", city : "Berkeley", postalCode : "94704", state : "CA", photoUrl : $"{Constants.BaseImagePath}/leonmuks.jpg" ),
                new Contact ( id : "FEB64319-1222-4C76-A8E5-EDFF84838B43", firstName : "Floyd", lastName : "Bell", company : "Netcore", jobTitle : "Procurement", email : "floyd.bell@netcore.net", phone : "603-226-4115", street : "450 15th St", city : "Oakland", postalCode : "94612", state : "CA", photoUrl : $"{Constants.BaseImagePath}/floydbell.jpg" ),
                new Contact ( id : "4FB56717-D2CF-4A5F-894A-C87383A8239D", firstName : "Vanessa", lastName : "Thornton", company : "Total Sources", jobTitle : "Product Manager", email : "vanessa.thornton@totalsourcesinc.com", phone : "419-998-6611", street : "550 Quarry Rd", city : "San Carlos", postalCode : "94070", state : "CA", photoUrl : $"{Constants.BaseImagePath}/vanessathornton.jpg" ),
                new Contact ( id : "89149CDA-5B31-4B15-88D2-6011B028F7AE", firstName : "John", lastName : "Boone", company : "A. L. Price", jobTitle : "Executive Associate", email : "jboone@alpricellc.com", phone : "973-579-4610", street : "233 E Harris Ave", city : "South San Francisco", postalCode : "94080", state : "CA", photoUrl : $"{Constants.BaseImagePath}/johnboone.jpg" ),
                new Contact ( id : "BFD74C2A-7840-45DD-9C47-13F90DE01F8B", firstName : "Ann", lastName : "Temple", company : "Foxmoor", jobTitle : "Director", email : "ann.temple@foxmoorinc.com", phone : "608-821-7667", street : "1270 San Pablo Ave", city : "Berkeley", postalCode : "94706", state : "CA", photoUrl : $"{Constants.BaseImagePath}/anntemple.jpg" ),
                new Contact ( id : "CF568F71-C4B6-45A9-A922-C9E69EF17B49", firstName : "Joseph", lastName : "Meeks", company : "Rose Records", jobTitle : "Manager", email : "jmeeks@roserecordsllc.com", phone : "978-628-6826", street : "28 N 1st St", city : "San Jose", postalCode : "95113", state : "CA", photoUrl : $"{Constants.BaseImagePath}/josephmeeks.jpg" ),
                new Contact ( id : "D81687FD-FE84-4F0F-AFDD-A104B28EC1A7", firstName : "Michelle", lastName : "Herring", company : "Full Color", jobTitle : "Production Specialist", email : "michelle.herring@fullcolorus.com", phone : "201-319-9344", street : "213 2nd Ave", city : "San Mateo", postalCode : "94401", state : "CA", photoUrl : $"{Constants.BaseImagePath}/michelleherring.jpg" ),
                new Contact ( id : "FB580ADA-4381-43EE-ADA1-D072878F08CE", firstName : "Daniel", lastName : "Jones", company : "Flexus", jobTitle : "Quality Assurance Associate", email : "daniel.jones@flexusinc.com", phone : "228-432-8712", street : "850 Bush St", city : "San Francisco", postalCode : "94108", state : "CA", photoUrl : $"{Constants.BaseImagePath}/danieljones.jpg" ),
                new Contact ( id : "953d9588-e6be-49cf-881d-68431b8285c3", firstName : "Margaret", lastName : "Cargill", company : "Redwood City Medical Group", jobTitle : "Director", email : "mcargill@rcmg.org", phone : "208-816-9793", street : "1037 Middlefield Road", city : "Redwood City", postalCode : "94063", state : "CA", photoUrl : $"{Constants.BaseImagePath}/margaretcargill.jpg" ),
                new Contact ( id : "450fe593-433f-4bca-9f39-f2a0e4c64dc6", firstName : "Benjamin", lastName : "Jones", company : "JH Manufacturing", jobTitle : "Head of Manufacturing", email : "ben.jones@jh.com", phone : "505.562.3086", street : "2091 Cowper St", city : "Palo Alto", postalCode : "94306", state : "CA", photoUrl : $"{Constants.BaseImagePath}/benjaminjones.jpg" ),
                new Contact ( id : "5c957b8f-6e76-470c-941f-789d12f10a42", firstName : "Ivan", lastName : "Diaz", company : "XYZ Robotics", jobTitle : "CEO", email : "ivan.diaz@xyzrobotics.com", phone : "406-496-8774", street : "1960 Mandela Parkway", city : "Oakland", postalCode : "94607", state : "CA", photoUrl : $"{Constants.BaseImagePath}/ivandiaz.jpg" ),
                new Contact ( id : "6FEFF721-2A97-4C0F-AACB-30B1F521ABF6", firstName : "Eric", lastName : "Grant", company : "MMSRI, Inc.", jobTitle : "Senior Manager", email : "egrant@mmsri.com", phone : "360-693-2388", street : "2043 Martin Luther King Jr. Way", city : "Berkeley", postalCode : "94704", state : "CA", photoUrl : $"{Constants.BaseImagePath}/ericgrant.jpg" ),
                new Contact ( id : "CA0A6161-6898-421D-9F29-A51B60F36BEE", firstName : "Stacey", lastName : "Valdovinos", company : "Global Manufacturing", jobTitle : "CEO", email : "svaldovinos@globalmanuf.com", phone : "440-243-7987", street : "98 Udayakavi Lane", city : "Danville", postalCode : "94526", state : "CA", photoUrl : $"{Constants.BaseImagePath}/staceyvaldovinos.jpg" ),
                new Contact ( id : "9CD6310F-1439-4898-9F51-EEC96D032CD3", firstName : "Jesus", lastName : "Cardell", company : "Pacific Marine Supply", jobTitle : "Manager", email : "jcardella@pacificmarine.com", phone : "410-745-5521", street : "1008 Rachele Road", city : "Walnut Creek", postalCode : "94597", state : "CA", photoUrl : $"{Constants.BaseImagePath}/jesuscardell.jpg" ),
                new Contact ( id : "D5E85894-129F-4F39-A75D-893DAB128ECD", firstName : "Wilma", lastName : "Woolley", company : "Mission School District", jobTitle : "Superintendent", email : "wwoolley@missionsd.org", phone : "940-696-1852", street : "7277 Moeser Lane", city : "El Cerrito", postalCode : "94530", state : "CA", photoUrl : $"{Constants.BaseImagePath}/wilmawoolley.jpg" ),
                new Contact ( id : "6CF4DE3E-FE50-4860-8E5C-6DCF479D4737", firstName : "Evan", lastName : "Armstead", company : "City of Richmond", jobTitle : "Board Member", email : "evan.armstead@richmond.org", phone : "415-336-2228", street : "398 23rd St", city : "Richmond", postalCode : "94804", state : "CA", photoUrl : $"{Constants.BaseImagePath}/evanarmstead.jpg" ),
                new Contact ( id : "DAFB9C5C-54A3-4F18-BC01-10AD2491AEC7", firstName : "James", lastName : "Jones", company : "East Bay Commercial Bank", jobTitle : "Manager", email : "james.jones@eastbaybank.com", phone : "313-248-7644", street : "4501 Pleasanton Way", city : "Pleasanton", postalCode : "94556", state : "CA", photoUrl : $"{Constants.BaseImagePath}/jamesjones.jpg" ),
                new Contact ( id : "AB6F1601-94F3-4E32-A08A-089B5B52DA36", firstName : "Douglas", lastName : "Greenly", company : "Bay Tech Credit Union", jobTitle : "Vice President", email : "d.greenly@baytechcredit.com", phone : "201-929-0094", street : "2267 Alameda Ave", city : "Alameda", postalCode : "94501", state : "CA", photoUrl : $"{Constants.BaseImagePath}/douglasgreenly.jpg" ),
                new Contact ( id : "70EB3223-4ED2-4FE2-9AC1-F72B474FF05F", firstName : "Brent", lastName : "Mason", company : "Rockridge Hotel", jobTitle : "Concierge", email : "brent.mason@rockridgehotel.com", phone : "940-482-7759", street : "1960 Mandela Parkway", city : "Oakland", postalCode : "94607", state : "CA", photoUrl : $"{Constants.BaseImagePath}/brentmason.jpg" ),
                new Contact ( id : "A5A8F111-FE08-4478-A90B-222F4BA033DD", firstName : "Richard", lastName : "Hogan", company : "Marin Luxury Senior Living", jobTitle : "Customer Care", email : "rhogan@marinseniorliving.com", phone : "978-658-7545", street : "674 Tiburon Blvd", city : "Belvedere Tiburon", postalCode : "94920", state : "CA", photoUrl : $"{Constants.BaseImagePath}/richardhogan.jpg" ),
                new Contact ( id : "6348C5F4-2073-4868-959C-D1650FD8C186", firstName : "Daniel", lastName : "Granville", company : "Cityview Consulting", jobTitle : "Consultant", email : "dgranville@cityviewconsulting.com", phone : "330-616-7467", street : "300 Spencer Ave", city : "Sausalito", postalCode : "94965", state : "CA", photoUrl : $"{Constants.BaseImagePath}/danielgranville.jpg" ),
                new Contact ( id : "303A5E88-E91D-43ED-9391-FDE9F7C03A66", firstName : "Margaret", lastName : "Kidd", company : "Marin Cultural Center", jobTitle : "President", email : "mkidd@marincultural.org", phone : "406-784-0602", street : "106 Throckmorton Ave", city : "Mill Valley", postalCode : "94941", state : "CA", photoUrl : $"{Constants.BaseImagePath}/margaretkidd.jpg" ),
                new Contact ( id : "0782C981-F003-44A4-87D1-771D3C6EB6B3", firstName : "Leo", lastName : "Parson", company : "San Rafel Chamber of Commerce", jobTitle : "Board Member", email : "leo.parson@sanrafaelcoc.org", phone : "773-991-5214", street : "199 Clorinda Ave", city : "San Rafael", postalCode : "94901", state : "CA", photoUrl : $"{Constants.BaseImagePath}/leoparson.jpg" ),
            };
        }
    }
}
