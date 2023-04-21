# Use the official .NET 6 SDK image as the build environment
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

# Set the working directory
WORKDIR /app

# Copy the project files into the container
COPY . ./

# Build the application
RUN dotnet restore
RUN dotnet publish -c Release -o out

# Use the official .NET 6 runtime image for the final image
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS runtime

# Set the working directory
WORKDIR /app

# Copy the published output from the build image
COPY --from=build /app/out ./

# Expose the desired port for the application to listen on
EXPOSE 5000

# Run the application
CMD ["dotnet", "AiTelegramChannel.ServerHost.dll"]
