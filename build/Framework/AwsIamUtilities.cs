using Amazon.Runtime;
using Amazon.SecurityToken;
using Amazon.SecurityToken.Model;
using System.Threading.Tasks;

namespace Framework
{
    public static class AwsIamUtilities
    {
        public static async Task<AssumeRoleResponse> AssumeRole(string roleArn, string sessionName, AWSCredentials credentials = null)
        {
            AmazonSecurityTokenServiceClient amazonSecurityTokenServiceClient = 
                credentials != null ? new AmazonSecurityTokenServiceClient(credentials) : new AmazonSecurityTokenServiceClient();

            return await amazonSecurityTokenServiceClient.AssumeRoleAsync(new AssumeRoleRequest
            {
                RoleArn = roleArn,
                RoleSessionName = sessionName
            });
        }
    }
}
