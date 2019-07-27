namespace UntisExportService.Core.Settings
{
    public interface IEndpointSettings
    {
        bool UseLegacyStrategy { get; set; }

        string SubstitutionsUrl { get; set; }

        string InfotextsUrl { get; set; }

        string ApiKey { get; set; }
    }
}

