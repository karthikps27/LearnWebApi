using Docker.DotNet;
using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ICSharpCode.SharpZipLib.Tar;
using Docker.DotNet.Models;
using Newtonsoft.Json;
using Amazon.Runtime;
using Amazon.ECR;
using Amazon.ECR.Model;
using System.Linq;
using System.Runtime.InteropServices;

namespace Framework
{
    public static class DockerUtility
    {
        public static async Task CreateDockerImage(string publishDirectory, string repositoryName, string tag) 
        {            
            string line;
            string imageId;
            const string successfullyBuilt = "Successfully built ";            

            using var tarball = CreateTarballForDockerfileDirectory(publishDirectory);
            using var dockerClient = GetDockerClient();
            using var responseStream = await dockerClient.Images.BuildImageFromDockerfileAsync(tarball, new ImageBuildParameters { Tags = new List<string> { $"{repositoryName}:{tag}" } });
            using var buildReader = new StreamReader(responseStream, new UTF8Encoding(false));            
            while ((line = await buildReader.ReadLineAsync()) != null)
            {
                JSONMessage jsonMessage = JsonConvert.DeserializeObject<JSONMessage>(line);
                Console.WriteLine(jsonMessage.Stream);
                if(jsonMessage.Stream != null && jsonMessage.Stream.Contains(successfullyBuilt))
                {
                    imageId = jsonMessage.Stream.Substring(successfullyBuilt.Length).Trim();
                    Console.WriteLine(imageId);
                }
            }                        
        }

        public static async Task PushDockerImageToEcr(string accountID, string registryAddress, AWSCredentials credentials, string repositoryName, string tag)
        {           
            var amazonECRClient = new AmazonECRClient(credentials);
            var response = await amazonECRClient.GetAuthorizationTokenAsync(new GetAuthorizationTokenRequest
            {
                RegistryIds = new List<string>{ accountID }
            });

            string[] tokens = Encoding.UTF8.GetString(Convert.FromBase64String(response.AuthorizationData.First().AuthorizationToken)).Split(":");

            var authconfig = new AuthConfig
            {
                Username = tokens[0],
                Password = tokens[1],
            };

            using (DockerClient dockerClientNew = GetDockerClient())
            {
                await dockerClientNew.Images.TagImageAsync($"{repositoryName}:{tag}", 
                    new ImageTagParameters { RepositoryName = $"{registryAddress}/{repositoryName}", Tag = tag });

                await dockerClientNew.Images.PushImageAsync($"{registryAddress}/{repositoryName}:{tag}",
                    new ImagePushParameters(), 
                    authconfig, 
                    new Progress<JSONMessage>(LogJsonMessage));
            }
        }

        public static DockerClient GetDockerClient()
        {
            string dockerEndpoint = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "npipe://./pipe/docker_engine" : "unix://var/run/docker.sock";
            return new DockerClientConfiguration(new Uri(dockerEndpoint)).CreateClient();
        }

        public static void LogJsonMessage(JSONMessage message)
        {
            if(message.Error != null)
            {
                Console.WriteLine($"Message: {message.Error.Message} {message.ErrorMessage}");
            }

            if(message.ProgressMessage != null)
            {
                Console.WriteLine($"{message.Status} {message.ID} {message.ProgressMessage}");
            }
        }

        private static Stream CreateTarballForDockerfileDirectory(string publishDirectory)
        {
            var tarball = new MemoryStream();
            var files = Directory.GetFiles(publishDirectory, "*.*", SearchOption.AllDirectories);

            using var archive = new TarOutputStream(tarball)
            {
                IsStreamOwner = false
            };

            foreach (var file in files)
            {
                string tarName = file.Substring(publishDirectory.Length).Replace('\\', '/').TrimStart('/');

                var entry = TarEntry.CreateTarEntry(tarName);
                using var fileStream = File.OpenRead(file);
                entry.Size = fileStream.Length;
                archive.PutNextEntry(entry);

                byte[] localBuffer = new byte[32 * 1024];
                while (true)
                {
                    int numRead = fileStream.Read(localBuffer, 0, localBuffer.Length);
                    if (numRead <= 0)
                        break;

                    archive.Write(localBuffer, 0, numRead);
                }

                archive.CloseEntry();
            }
            archive.Close();

            tarball.Position = 0;
            return tarball;
        }
    }
}
