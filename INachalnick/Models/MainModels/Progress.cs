using ProgramUtilities.Mongo;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace INachalnick.Models
{
    public class Progress : MongoBaseDocument
    {

        public DateTime StartedDraw { get; set; }
        public string DrawerName { get; set; }
        public List<string> Reviewes { get; set; }
        public DateTime FilesCreatedDate { get; set; }
        public DateTime PrintedDate { get; set; }
        public Progress(DateTime startedDraw, string drawerName, List<string> reviewes, DateTime filesCreatedDate, DateTime printedDate)
        {
            StartedDraw = startedDraw;
            DrawerName = drawerName;
            Reviewes = reviewes;
            FilesCreatedDate = filesCreatedDate;
            PrintedDate = printedDate;
        }

    }
}