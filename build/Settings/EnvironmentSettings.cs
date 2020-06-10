using System;

namespace Settings
{
    public abstract class EnvironmentSettings
    {
        public string AccountID { get; set; }
        public string ApplicationSecurityGroupStackName { get; set; }
        public string ApplicationStackName { get; set; }        
        public string ContainerRegistryAddress { get; set; }
        public string ContainerRegistryAccessRole { get; set; }
        public string CidrIp { get; set; }
        public string DbUsernameParameterPath { get; set; }
        public string DbPasswordParameterPath { get; set; }        
        public string PostgresDBName { get; set; }
        public string PostgresDBStackName { get; set; }
        public string PostgresDBInstanceClass { get; set; }
        public string RepositoryName { get; set; }
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
