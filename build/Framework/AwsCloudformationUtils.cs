using Amazon.CloudFormation;
using Amazon.CloudFormation.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Framework
{
    public static class AwsCloudformationUtils
    {
        public static readonly List<string> DefaultCapabilities = new List<string> { Capability.CAPABILITY_NAMED_IAM, Capability.CAPABILITY_AUTO_EXPAND};
        public static async Task CreateOrUpdateStack(string stackName, string templatePath, List<Parameter> parameters, bool disableRollBack = false)
        {
            if (await StackExists(stackName))
            {
                await UpdateCloudformationStack(stackName, templatePath, parameters);
            }
            else
            {
                await CreateCloudformationStack(stackName, templatePath, parameters, disableRollBack);
            }
        }

        public static async Task CreateCloudformationStack(string stackName, string templatePath, List<Parameter> parameters, bool disableRollBack = false)
        {
            var client = new AmazonCloudFormationClient();
            await client.CreateStackAsync(new CreateStackRequest
            {
                StackName = stackName,
                TemplateBody = File.ReadAllText(templatePath),
                Parameters = parameters,
                Capabilities = DefaultCapabilities,
                DisableRollback = disableRollBack
            });

            await WaitForStackStatus(stackName, StackStatus.CREATE_COMPLETE);
        }        


        public static async Task<StackStatus> GetStackStatus(string stackName)
        {
            var client = new AmazonCloudFormationClient();
            DescribeStacksResponse describeStacksResponse = await client.DescribeStacksAsync(new DescribeStacksRequest { StackName = stackName });
            return describeStacksResponse.Stacks.First().StackStatus;
        }

        public static async Task<bool> StackExists(string stackName)
        {
            try
            {
                await GetStackStatus(stackName);
                return true;
            }
            catch(AmazonCloudFormationException exception) when (exception.Message.Contains("does not exist"))
            {                
            }
            return false;
        }

        public static async Task WaitForStackStatus(string stackName, StackStatus finalStatus)
        {
            StackStatus currentStatus = await GetStackStatus(stackName);
            
            while(currentStatus != finalStatus)
            {
                if(!currentStatus.ToString().Contains("IN_PROGRESS"))
                {
                    throw new Exception($"Error while creation or updating stack: {currentStatus}");
                }
                Console.WriteLine(currentStatus);
                await Task.Delay(20000);
                currentStatus = await GetStackStatus(stackName);
            }
            Console.WriteLine(currentStatus);
        }

        public static async Task UpdateCloudformationStack(string stackName, string templatePath, List<Parameter> parameters)
        {
            var client = new AmazonCloudFormationClient();
            try
            {
                await client.UpdateStackAsync(new UpdateStackRequest
                {
                    StackName = stackName,
                    TemplateBody = File.ReadAllText(templatePath),
                    Parameters = parameters,
                    Capabilities = DefaultCapabilities,
                });
            }
            catch(AmazonCloudFormationException exc) when (exc.Message.Contains("No updates are to be performed"))
            {
                Console.WriteLine("Cloud stack already up to date");
            }            
        }
    }
}
