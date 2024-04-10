using Microsoft.Extensions.Configuration;


namespace Protyo.Utilities.Configuration.Contracts
{
    public interface IConfigurationSetting
    {
        public IConfiguration appSettings { get; set; }
    }
}
