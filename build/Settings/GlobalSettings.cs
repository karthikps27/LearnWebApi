using System;
using System.Collections.Generic;
using System.Text;

namespace Settings
{
    public class GlobalSettings : EnvironmentSettings
    {
        public GlobalSettings()
        {
            AccountID = "342305230154";
            RepositoryName = "learnwebapi";
            ContainerRegistryAddress = AccountID + ".dkr.ecr.ap-southeast-2.amazonaws.com";
            ContainerRegistryAccessRole = "arn:aws:iam::" + AccountID + ":role/LearnWebApi-ContainerRegistryAccessRole";
            CidrIp = "120.159.99.103/32";
            SubnetIds = "subnet-f94946b0,subnet-da6ee382,subnet-bd1a08da";
            VpcId = "vpc-d45379b3";
        }        
    }
}
