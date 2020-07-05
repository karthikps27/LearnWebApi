using Amazon;
using Amazon.SimpleSystemsManagement;
using Amazon.SimpleSystemsManagement.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BookMaster.Security.Framework
{
    public class AwsParameterStore
    {
        public async Task<string> GetParameterValueAsync(string key, bool decryption)
        {
            try
            {
                using var client = new AmazonSimpleSystemsManagementClient(RegionEndpoint.APSoutheast2);
                var parameter = await client.GetParameterAsync(new GetParameterRequest
                {
                    Name = key,
                    WithDecryption = decryption
                });
                return parameter?.Parameter?.Value;
            }
            catch (Exception exception)
            {
                throw new AmazonSimpleSystemsManagementException($"Parameter not found in parameter store: {key}", exception);
            }
        }
    }
}