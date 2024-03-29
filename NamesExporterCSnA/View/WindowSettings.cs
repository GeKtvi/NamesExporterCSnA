﻿using GeKtviWpfToolkit;
using System;
using System.Windows;
using System.Xml.Serialization;

namespace NamesExporterCSnA.View
{
    [XmlInclude(typeof(GridLength))]
    public class WindowSettings : DefaultWindowSettings
    {
        public GridLength FirstColumn
        {
            // Create the "GridLength" from the separate properties
            get => new GridLength(this.FirstColumnValue, (GridUnitType)Enum.Parse(typeof(GridUnitType), this.FirstColumnType));
            set
            {
                // store the "GridLength" properties in separate properties
                this.FirstColumnType = value.GridUnitType.ToString();
                this.FirstColumnValue = value.Value;
            }
        }

        public string FirstColumnType { get; set; } = "Star";
        public double FirstColumnValue { get; set; } = 1;


        public GridLength SecondColumn
        {
            // Create the "GridLength" from the separate properties
            get => new GridLength(this.SecondColumnValue, (GridUnitType)Enum.Parse(typeof(GridUnitType), this.SecondColumnType));
            set
            {
                // store the "GridLength" properties in separate properties
                this.SecondColumnType = value.GridUnitType.ToString();
                this.SecondColumnValue = value.Value;
            }
        }

        public string SecondColumnType { get; set; } = "Star";
        public double SecondColumnValue { get; set; } = 2;

        public WindowSettings()
        {
            Top = 250;
            Left = 300;
            Height = 600;
            Width = 1100;
        }
    }
}
