# Use the official .NET SDK image as a base image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /app

# Copy csproj and restore dependencies
COPY InterviewTest.sln ./

# Copy the entire project and build
COPY . ./
RUN dotnet restore

RUN dotnet publish -c Release -o out

# Use the official .NET runtime image as the final image
FROM mcr.microsoft.com/dotnet/runtime:8.0 AS runtime

WORKDIR /app

# Copy the published output from the build image
COPY --from=build /app/out ./

# Set the entry point
ENTRYPOINT ["dotnet", "InterviewTest.dll"]
