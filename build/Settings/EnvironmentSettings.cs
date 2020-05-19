using System;

namespace Settings
{
    public abstract class EnvironmentSettings
    {
        public string AccountID { get; set; }
        public string RepositoryName { get; set; }
        public string ContainerRegistryAddress { get; set; }
        public string ContainerRegistryAccessRole { get; set; }
        public string CidrIp { get; set; }
        public string SubnetIds { get; set; }
        public string VpcId { get; set; }

        public static EnvironmentSettings CreateSettingsInstance()
        {
            const string objectToInstantiate = "Settings.GlobalSettings, Build";
            var objectType = Type.GetType(objectToInstantiate);
            return (EnvironmentSettings)Activator.CreateInstance(objectType);
        }
    }
}
