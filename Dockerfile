FROM mcr.microsoft.com/dotnet/core/aspnet:3.1

WORKDIR /app

#Adding some data to make some difference in file size.
#Doing this as there no file difference seen with and without the Docker file

COPY ./* ./
RUN chmod +x BookMaster*

ENTRYPOINT ["dotnet","BookMaster.dll"]