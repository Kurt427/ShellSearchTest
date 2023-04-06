using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ShellSearchTest
{
    public class TestItem
    {
        public string Name { get; set; }
    }

    public static class TestItems
    {
        public static IList<TestItem> Items { get; private set; }

        static TestItems()
        {
            Items = new List<TestItem>
            {
                new TestItem { Name = "Item 1" },
                new TestItem { Name = "Item 2" },
                new TestItem { Name = "String 1" },
                new TestItem { Name = "String 2" },
                new TestItem { Name = "Last item" }
            };
        }
    }

    public class AboutSearchHandler : SearchHandler
    {
        protected override void OnQueryChanged(string oldValue, string newValue)
        {
            base.OnQueryChanged(oldValue, newValue);

            if (string.IsNullOrWhiteSpace(newValue) || TestItems.Items.Count == 0)
            {
                ItemsSource = null;
            }
            else
            {
                List<TestItem> items = TestItems.Items
                    .Where(i => i.Name.ToLower().Contains(newValue.ToLower()))
                    .Take(20)
                    .ToList();
                // TOOD: When also set ShowsResults="True" in XAML, causes crash on iOS after OnQueryChanged() returns; fine on Windows.
                //ItemsSource = items;

                // TODO: Due to above, doing auto selection (disabled for now)
                //if (items.Count == 1 && items[0].Name.ToLower() == newValue.ToLower())
                //{
                //    OnItemSelected(items[0]);
                //}
            }
        }

        // TODO: Called only when search suggestions are enabled and the user chooses a selection, or in iOS
        //       simulator when Return button typed, or on an iOS device when "Search" tapped on virtual keyboard.
        protected override async void OnQueryConfirmed()
        {
            base.OnQueryConfirmed();

            Debug.WriteLine("AboutSearchHandler.OnQueryConfirmed().");

            await Task.Delay(100);  // Delay a bit to give a change for IsFocused to change to 'False'.

            await Shell.Current.CurrentPage.DisplayAlert("About", $"AboutSearchHandler.OnQueryConfirmed(): Query: '{Query}' IsFocused: '{IsFocused}'.", "OK");
        }

        protected override void OnItemSelected(object item)
        {
            Debug.WriteLine("AboutSearchHandler.OnItemSelected().");

            base.OnItemSelected(item);

            // TODO: delay for animation?
            //await Task.Delay(1000);

            if (item is TestItem ti)
            {
                Debug.WriteLine($"AboutSearchHandler.OnItemSelected(): Processing item '{ti.Name}'.");

                // Nothing done with item for this test
            }
        }

        protected override void OnFocused()
        {
            base.OnFocused();
            Debug.WriteLine("AboutSearchHandler.OnFocused().");
        }

        protected override async void OnUnfocus()
        {
            base.OnUnfocus();
            Debug.WriteLine("AboutSearchHandler.OnUnfocus().");

            //await Shell.Current.CurrentPage.DisplayAlert("About", $"AboutSearchHandler.OnUnfocus(): IsFocused: '{IsFocused}'.", "OK");
        }
    }

    public partial class AboutViewModel : ObservableObject
    {
        [ObservableProperty]
        string title;


        public AboutViewModel()
        {
            Title = "About";
        }
    }
}
