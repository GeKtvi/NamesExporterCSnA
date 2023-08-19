namespace NamesExporterCSnA.Data.UpdateLog
{
    public struct UpdateFail
    {
        public UpdateFailType Type { get; set; } = UpdateFailType.Error;
        public string Message { get; set; } = "{NotSet}";
        public string SchemeName { get; set; } = "{NotSet}";
        public string WireName { get; set; } = "{NotSet}";
        public string Source { get; set; } = "{NotSet}";
        public UpdateFail() { }
    }
}
