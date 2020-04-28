using Docker.DotNet;
using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ICSharpCode.SharpZipLib.Tar;
using Docker.DotNet.Models;

namespace Framework
{
    public static class DockerUtility
    {
        public static async Task CreateDockerImage(string publishDirectory) 
        {
            /* DockerClient dockerClient = new DockerClientConfiguration(new Uri("npipe://./pipe/docker_engine")).CreateClient();
            new MemoryStream();

            using (var memoryStream = new MemoryStream())
            {
                Console.WriteLine("Memory stream length before: " + memoryStream.Length);
                using (var tarArchive = TarArchive.CreateOutputTarArchive(memoryStream))
                {
                    tarArchive.RootPath = publishDirectory;
                    AddFilesToTarArchive(tarArchive, publishDirectory);
                    Console.WriteLine("Memory stream length after: " + memoryStream.Length);
                    var task = dockerClient.Images.BuildImageFromDockerfileAsync(memoryStream, new ImageBuildParameters { Tags = new List<string> { "tag" } });
                    task.Wait();
                }
            } */

            using var tarball = CreateTarballForDockerfileDirectory(publishDirectory);
            using var dockerClientNew = new DockerClientConfiguration(new Uri("npipe://./pipe/docker_engine")).CreateClient();
            using var responseStream = await dockerClientNew.Images.BuildImageFromDockerfileAsync(tarball, new ImageBuildParameters { Tags = new List<string> { "tag" } });
            using var buildReader = new StreamReader(responseStream, new UTF8Encoding(false));            
        }

        public static void AddFilesToTarArchive(TarArchive tarArchive, string publishDirectory)
        {           
            var filesPaths = Directory.GetFiles(publishDirectory);
            foreach (var filepath in filesPaths)
            {
                var tarEntry = TarEntry.CreateEntryFromFile(filepath);
                tarArchive.WriteEntry(tarEntry, false);
            }

            var directories = Directory.GetDirectories(publishDirectory);
            foreach(var directory in directories)
            {
                AddFilesToTarArchive(tarArchive, directory);
            }
        }

        private static Stream CreateTarballForDockerfileDirectory(string directory)
        {
            var tarball = new MemoryStream();
            var files = Directory.GetFiles(directory, "*.*", SearchOption.AllDirectories);

            using var archive = new TarOutputStream(tarball)
            {
                //Prevent the TarOutputStream from closing the underlying memory stream when done
                IsStreamOwner = false
            };

            foreach (var file in files)
            {
                //Replacing slashes as KyleGobel suggested and removing leading /
                string tarName = file.Substring(directory.Length).Replace('\\', '/').TrimStart('/');

                //Let's create the entry header
                var entry = TarEntry.CreateTarEntry(tarName);
                using var fileStream = File.OpenRead(file);
                entry.Size = fileStream.Length;
                archive.PutNextEntry(entry);

                //Now write the bytes of data
                byte[] localBuffer = new byte[32 * 1024];
                while (true)
                {
                    int numRead = fileStream.Read(localBuffer, 0, localBuffer.Length);
                    if (numRead <= 0)
                        break;

                    archive.Write(localBuffer, 0, numRead);
                }

                //Nothing more to do with this entry
                archive.CloseEntry();
            }
            archive.Close();

            //Reset the stream and return it, so it can be used by the caller
            tarball.Position = 0;
            return tarball;
        }
    }
}
