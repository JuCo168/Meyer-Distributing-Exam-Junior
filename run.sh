#!/bin/bash

docker stop interview-test-container sqlserver-container

docker rm interview-test-container sqlserver-container

docker rm interview-test sqlserver

docker build -t sqlserver -f Dockerfile-sql .

docker run -e 'ACCEPT_EULA=Y' -e 'SA_PASSWORD=StrongPassw0rd' -e 'MSSQL_ENCRYPT=DISABLED' -p 1433:1433 --name sqlserver-container -d sqlserver 

# docker build -t interview-test -f ./Dockerfile .

echo "Wait for database creation"

sleep 10s

cd InterviewTest

dotnet build

dotnet run

# docker run -d -p 8080:80 --name interview-test-container interview-test
