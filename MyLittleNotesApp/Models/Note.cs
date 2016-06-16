using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;

namespace MyLittleNotesApp.Models
{
    public class Note:ObservableObject
    {
        public Note(string content, DateTime date)
        {
            Content = content;
            Date = date;
        }

        public string Content { get; set; }
        public DateTime Date { get; set; }

        public override string ToString()
        {
            return Content;
        }
    }
}
