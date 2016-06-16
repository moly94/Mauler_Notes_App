using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Views;
using MyLittleNotesApp.Models;
using MyLittleNotesApp.Services;
using MyLittleNotesApp.Views;

namespace MyLittleNotesApp.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public readonly NavigationService _navigationService;

                public Note SelectedNote{ get; set; }

        public List<Note> Notes
        {
           /* get
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
            }*/
            get; set; }

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
                    if (Ascending==true)
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

        public MainViewModel()
        {
            ContentShownInRead = 10;
            Notes = new List<Note>();
            //ShownNotes = new ObservableCollection<Note>();
            SearchTerm = "";
            Ascending = true;
            storageService= new LocalStorageService();
            

            _navigationService = new NavigationService();
            _navigationService.Configure("New Note", typeof(NewNoteView));
            _navigationService.Configure("Read Notes", typeof(ReadNotesView));
            _navigationService.Configure("Search Notes", typeof(SearchNotesView));
            _navigationService.Configure("Settings", typeof(SettignsView));
        
        }

        public void NavigateBack()
        {
            _navigationService.GoBack();
        }

        public async void AddNote()
        {
            var dialog = new MessageDialog("Input Field is empty");
            if (string.IsNullOrEmpty(NewNoteContent))
            {
               await dialog.ShowAsync();
            }
            else
            {
                Notes.Add(new Note(DateTime.Now+": "+NewNoteContent, DateTime.Now));
                ShownNotes.Add(new Note(DateTime.Now+ ": " + NewNoteContent, DateTime.Now));
                NewNoteContent = "";
                NavigateBack();
            }
            
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
                Notes.Add(new Note(DateTime.Now+ ": " + NewNoteContent, DateTime.Now));
                ShownNotes.Add(new Note(DateTime.Now+ ": " + NewNoteContent, DateTime.Now));
                NewNoteContent = "";
                NavigateBack();
            }

        }

        public async void DeleteNote()
        {
            var dialog = new MessageDialog("Are you sure?");
            dialog.Title = "Delete Note";
            dialog.Commands.Add(new UICommand { Label = "Ok", Id = 0 });
            dialog.Commands.Add(new UICommand { Label = "Cancel", Id = 1 });
            var res = await dialog.ShowAsync();

            if ((int)res.Id == 0)
           {
                Notes.Remove(SelectedNote);
                ShownNotes.Remove(SelectedNote);
                NavigateBack();
            }
        }

        public async void Cancel()
        {
            var dialog = new MessageDialog("Are you sure?");
            dialog.Title = "Cancel Writing Note";
            dialog.Commands.Add(new UICommand { Label = "Ok", Id = 0 });
            dialog.Commands.Add(new UICommand { Label = "Cancel", Id = 1 });
            var res = await dialog.ShowAsync();

            if ((int)res.Id == 0)
            {
                NavigateBack();
            }
        }

        public void Save()
        {
            storageService.Write(nameof(ContentShownInRead), ContentShownInRead);
            storageService.Write(nameof(Ascending), Ascending);
        }

        public void Load()
        {
            ContentShownInRead = storageService.Read<int>(nameof(ContentShownInRead), 5);
            Ascending = storageService.Read<bool>(nameof(Ascending), true);
        }

    }
}
