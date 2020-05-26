# My Contacts - Laconic

> A rewrite of [My Contacts sample app](https://github.com/xamarin/app-contacts) by Xamarin using [Laconic library](https://github.com/shirshov/laconic)
> Do `git submodule init` after cloning this solution

## Step 1 - Settings Page

More details for the step here: https://omnitalented.com/converting-my-contacts-step-1/

---
> Original README below:
---
# My Contacts

A Xamarin.Forms app named *My Contacts*. The app is a simple list of contacts, each of which can be viewed in a detail screen and modified in an edit screen. It runs on iOS, Android, and UWP. It is powered by an ASP.NET Core web API backend.

![Screenshots of My Contact app](/art/mycontacts.png)
    
##Integrations

Includes integrations such as:
* getting directions
* making calls
* sending text messages
* email composition


## Google Maps API key (Android)
For Android, you'll need to obtain a Google Maps API key, read through the [documentation](https://docs.microsoft.com/xamarin/android/platform/maps-and-location/maps/obtaining-a-google-maps-api-key).

Insert it in the Android project: `~/Properties/AndroidManifest.xml`:

```
    <application ...>
      ...
      <meta-data android:name="com.google.android.geo.API_KEY" android:value="GOOGLE_MAPS_API_KEY" />
      ...
    </application>
```

## UWP Maps

See [documentation](https://docs.microsoft.com/bingmaps/getting-started/bing-maps-dev-center-help/getting-a-bing-maps-key) on how to register for an API key. Set it in `MyContacts/Utils/Settings.cs`. A development key is provided.


## Screens



The app has three main screens:
* a list screen
* a read-only detail screen
* an editable detail screen

## People

All images of people in the app come from [UIFaces.com](http://uifaces.com/authorized). In accordance with the guidelines, fictitious names have been provided. 


## License
Under MIT
