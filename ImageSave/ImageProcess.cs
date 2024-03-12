using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageSave
{
    public class ImageProcess
    {
        public Int64 Id { get; set; }
        public string SourcePath { get; set; } = default!;
        public string DestinationPath { get; set; } = default!;
        public DateTime? SyncDateTime { get; set; }
        public bool Active { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
