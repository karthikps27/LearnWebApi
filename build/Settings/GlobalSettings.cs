namespace Settings
{
    public class GlobalSettings : EnvironmentSettings
    {
        public GlobalSettings()
        {
            AccountID = "342305230154";
            RepositoryName = "learnwebapi";
            ApplicationStackName = $"{RepositoryName}-Application";
            ApplicationSecurityGroupStackName = $"{RepositoryName}-ApplicationSecurityGroup";

            ContainerRegistryAddress = AccountID + ".dkr.ecr.ap-southeast-2.amazonaws.com";
            ContainerRegistryAccessRole = "arn:aws:iam::" + AccountID + ":role/LearnWebApi-ContainerRegistryAccessRole";

            CidrIp = "120.159.99.103/32";

            DBServerStackName = $"{RepositoryName}-PostgresDB";
            DBServerInstanceClass = "db.t2.micro";
            PostgresDBName = "bookdata";
            DbUsernameParameterPath = "/bookData/database/username";
            DbPasswordParameterPath = "/bookData/database/password";

            S3BucketStackName = $"{RepositoryName}-S3Bucket";
            S3BucketName = "book-data-resources";
            S3ArchiveBucketName = "book-data-archive";

            SubnetIds = "subnet-f94946b0,subnet-da6ee382,subnet-bd1a08da";
            VpcId = "vpc-d45379b3";
        }        
    }
}
