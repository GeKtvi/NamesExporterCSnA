﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NamesExporterCSnA.Model.Data.Marks.Exceptions
{
    class VendorsDataNotFoundException : Exception
    {
        public VendorsDataNotFoundException(string message) : base(message) { }
    }
}
