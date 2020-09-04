using MyContacts.Interfaces;
using Xamarin.Forms;

[assembly: Dependency(typeof(MyContacts.iOS.Helpers.Environment))]
namespace MyContacts.iOS.Helpers
{
    public class Environment : IEnvironment
    {
        public void SetStatusBarColor(System.Drawing.Color color, bool darkStatusBarTint)
        {
        }
    }
}