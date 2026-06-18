using System;
using System.Collections.Generic;
using System.Text;

namespace WarehouseApp.Domain.Core.Configurations
{
    public class FileSettings
    {
        public long MaxFileSizeMB { get; set; }
        public string[] AllowedExtensions { get; set; }

        public long MaxFileSizeBytes => MaxFileSizeMB * 1024 * 1024;
    }
}
