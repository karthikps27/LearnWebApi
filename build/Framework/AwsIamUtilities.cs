using Amazon.Runtime;
using Amazon.SecurityToken;
using Amazon.SecurityToken.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Framework
{
    public static class AwsIamUtilities
    {
        public static async Task<AssumeRoleResponse> AssumeRole(string roleArn, string sessionName, AWSCredentials credentials = null)
        {
            AmazonSecurityTokenServiceClient amazonSecurityTokenServiceClient = new AmazonSecurityTokenServiceClient(credentials);
            return await amazonSecurityTokenServiceClient.AssumeRoleAsync(new AssumeRoleRequest
            {
                RoleArn = roleArn,
                RoleSessionName = sessionName
            });
        }
    }
}
