using Amazon;
using Amazon.SimpleSystemsManagement;
using Amazon.SimpleSystemsManagement.Model;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Framework
{
    public static class AwsParameterStoreUtils
    {
        private static readonly ILogger _logger;
        public static async Task<string> GetParameterValueAsync(string key, bool decryption)
        {
            string parameterValue = null;
            try
            {
                var parameter = await new AmazonSimpleSystemsManagementClient(RegionEndpoint.APSoutheast2)
                .GetParameterAsync(new GetParameterRequest
                {
                    Name = key,
                    WithDecryption = decryption
                });
                parameterValue = parameter?.Parameter?.Value;
            }
            catch (ParameterNotFoundException)
            {
                _logger.LogWarning($"{key} not found in parameter store");
            }
            return parameterValue;
        }
    }
}
