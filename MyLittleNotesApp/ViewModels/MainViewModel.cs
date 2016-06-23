using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Maps;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Views;
using MyLittleNotesApp.Models;
using MyLittleNotesApp.Services;
using MyLittleNotesApp.Views;
using GalaSoft.MvvmLight.Messaging;
using YANApp.PCL.Services;

namespace MyLittleNotesApp.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public readonly NavigationService _navigationService;

        public Note SelectedNote { get; set; }

        public List<Note> Notes { get; set; }

        public List<Note> ShownNotes
        {
            get
            {
                List<Note> notes = new List<Note>();
                if (Ascending == true)
                    foreach (
                        var nt in
                            Notes.Where(n => n.Content.ToUpper().Contains(SearchTerm.ToUpper()))
                                .OrderBy(n => n.Date).Take(ContentShownInRead)
                                .ToList())
                    {
                        notes.Add(nt);
                    }
                else
                {
                    foreach (
                        var nt in
                            Notes.Where(n => n.Content.ToUpper().Contains(SearchTerm.ToUpper()))
                                .OrderByDescending(n => n.Date).Take(ContentShownInRead)
                                .ToList())
                    {
                        notes.Add(nt);
                    }
                }
                return notes;
            }
        }

        public bool Ascending { get; set; }

        public List<Note> SearchedNotes
        {
            get
            {
                var notes = new List<Note>();
                if (SearchTerm != "")
                {
                    if (Ascending == true)
                        foreach (
                            var nt in
                                Notes.Where(n => n.Content.ToUpper().Contains(SearchTerm.ToUpper()))
                                    .OrderBy(n => n.Date)
                                    .ToList())
                        {
                            notes.Add(nt);
                        }
                    else
                    {
                        foreach (
                            var nt in
                                Notes.Where(n => n.Content.ToUpper().Contains(SearchTerm.ToUpper()))
                                    .OrderByDescending(n => n.Date)
                                    .ToList())
                        {
                            notes.Add(nt);
                        }
                    }
                }

                return notes;
            }


        }


        public string NewNoteContent { get; set; }

        public int ContentShownInRead { get; set; }

        public string SearchTerm { get; set; }

        private readonly IStorageService storageService;

        private Geolocator locator;

        public Geopoint Center { get; set; } = new Geopoint(new BasicGeoposition() {Latitude = 20.0, Longitude = 10.0});

        public double Zoomlevel { get; set; } = 5;

        public double Pitch { get; set; }
        public double Orientation { get; set; }

        public MapStyle MapStyle => (MapStyle) Enum.Parse(typeof(MapStyle), MapStyleName);

        public string MapStyleName { get; set; } = "Road";

        public CloudDataService cs { get; set; }
        public string TenantId { get; set; } = "S1510237022";

       
        public MainViewModel()
        {
            ContentShownInRead = 10;
            //Notes = new List<Note>();
            //ShownNotes = new ObservableCollection<Note>();
            SearchTerm = "";
            Ascending = true;
            storageService = new LocalStorageService();

            locator = new Geolocator();

            cs = new CloudDataService(TenantId);
            _navigationService = new NavigationService();
            _navigationService.Configure("New Note", typeof(NewNoteView));
            _navigationService.Configure("Read Notes", typeof(ReadNotesView));
            _navigationService.Configure("Search Notes", typeof(SearchNotesView));
            _navigationService.Configure("Settings", typeof(SettignsView));

            LoadData();

        }

        public void NavigateBack()
        {
            _navigationService.GoBack();
        }

        public async void EditNote()
        {


            var dialog = new MessageDialog("Input Field is empty");
            if (string.IsNullOrEmpty(SelectedNote.Content))
            {
                await dialog.ShowAsync();
            }
            else
            {
                Notes.Remove(SelectedNote);
                ShownNotes.Remove(SelectedNote);
                //Notes.Add(new Note(DateTime.Now + ": " + NewNoteContent, DateTime.Now));
                //ShownNotes.Add(new Note(DateTime.Now + ": " + NewNoteContent, DateTime.Now));
                AddNote();
                NewNoteContent = "";
                NavigateBack();
            }

        }

        public async void DeleteNote()
        {
            var dialog = new MessageDialog("Are you sure?");
            dialog.Title = "Delete Note";
            dialog.Commands.Add(new UICommand {Label = "Ok", Id = 0});
            dialog.Commands.Add(new UICommand {Label = "Cancel", Id = 1});
            var res = await dialog.ShowAsync();

            if ((int) res.Id == 0)
            {
                Notes.Remove(SelectedNote);
                ShownNotes.Remove(SelectedNote);
                cs.DeleteNote(SelectedNote);
                NavigateBack();
            }
        }

        public async void Cancel()
        {
            var dialog = new MessageDialog("Are you sure?");
            dialog.Title = "Cancel Writing Note";
            dialog.Commands.Add(new UICommand {Label = "Ok", Id = 0});
            dialog.Commands.Add(new UICommand {Label = "Cancel", Id = 1});
            var res = await dialog.ShowAsync();

            if ((int) res.Id == 0)
            {
                NavigateBack();
            }
        }

        public void Save()
        {
            storageService.Write(nameof(ContentShownInRead), ContentShownInRead);
            storageService.Write(nameof(Ascending), Ascending);
            storageService.Write(nameof(TenantId), TenantId);
        }

        public void Load()
        {
            ContentShownInRead = storageService.Read<int>(nameof(ContentShownInRead), 5);
            Ascending = storageService.Read<bool>(nameof(Ascending), true);
            TenantId = storageService.Read<string>(nameof(TenantId), "S1510237022");
        }

        private Random random = new Random();

        public async void AddNote()
        {

        var dialog = new MessageDialog("Input Field is empty");
        if (string.IsNullOrEmpty(NewNoteContent))
        {
            await dialog.ShowAsync();
        }
        else
        {
            var access = await Geolocator.RequestAccessAsync();

            switch (access)
            {
                case GeolocationAccessStatus.Allowed:

                    var geolocator = new Geolocator();
                    var geopositon = await geolocator.GetGeopositionAsync();
                    var geopoint = geopositon.Coordinate.Point;
                    Notes.Add(new Note(DateTime.Now + ": " + NewNoteContent, DateTime.Now, geopoint, geopositon.Coordinate.Latitude, geopositon.Coordinate.Longitude));
                    ShownNotes.Add(new Note(DateTime.Now + ": " + NewNoteContent, DateTime.Now, geopoint, geopositon.Coordinate.Latitude, geopositon.Coordinate.Longitude));

                    await cs.AddNote(new Note(DateTime.Now + ": " + NewNoteContent, DateTime.Now, geopoint, geopositon.Coordinate.Latitude, geopositon.Coordinate.Longitude));
                  

                    ZoomToFit();
                    Center = geopoint;
                    Zoomlevel = 15;
                    break;
                    case GeolocationAccessStatus.Unspecified:
                        var dialogError = new ContentDialog
                        {
                            Title = "Unspecified Error",
                            Content = "There was an unspecified error while retrieving your location!",
                            PrimaryButtonText = "Ok",
                        };

                        break;
                    case GeolocationAccessStatus.Denied:
                        var dialogDenied = new ContentDialog()
                        {
                            Title = "Access denied",
                            Content = "You need to allow to track your location!",
                            PrimaryButtonText = "Ok",
                        };

                        await dialogDenied.ShowAsync();
                        break;
                }
                NewNoteContent = "";
                NavigateBack();
            }
        }

        public void ZoomToFit()
        {
            Messenger.Default.Send("zoom");
        }
        public async void LoadData()
        {
            Notes = new List<Note>(await cs.GetAllNotes());

            //if (!string.IsNullOrEmpty(SearchTerm)) notes = notes.Where(n => n.Title.ToLower().Contains(SearchTerm) || n.Content.ToLower().Contains(SearchTerm));
            //if (FromDate.HasValue)                 notes = notes.Where(n => n.CreatedAt >= FromDate.Value.Date);  
            //if (ToDate.HasValue)                   notes = notes.Where(n => n.CreatedAt < ToDate.Value.Date.AddDays(1));

            /*notes = notes.Where(n => (!FromDate.HasValue || n.CreatedAt >= FromDate.Value.Date)
                                  && (!ToDate.HasValue || n.CreatedAt < ToDate.Value.Date.AddDays(1))
                                  && (string.IsNullOrEmpty(SearchTerm) || n.Title.ToLower().Contains(SearchTerm) || n.Description.ToLower().Contains(SearchTerm)));

            notes = settings.IsSortAscending
                        ? notes.OrderBy(n => n.CreatedAt)
                        : notes.OrderByDescending(n => n.CreatedAt);


            notes = notes.Take(settings.NumberOfNotes);
            Notes = new ObservableCollection<Note>(notes);
            */
            //Messenger.Default.Register<DeleteMessage>(this, DeleteNote);
        }
    }
}

