﻿namespace NamesExporterCSnA.Data.Cables
{
    internal class PurchasedCable : ICable
    {
        public string FullName => CableType;

        public string CableType { get; set; } = "{NotSet}";
        public double Length { get; set; }
        public string SchemeName { get; set; } = "{NotSet}";

        public bool HasFixedLength => true;
        public int WireCount => 0;
        public int WirePairs => 0;
        public double WireSection => 0;
    }
}
