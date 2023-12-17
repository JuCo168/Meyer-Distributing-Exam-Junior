# Wait for SQL Server to start
sleep 15s

# Create the InterviewTest database if it doesn't exist
/opt/mssql-tools/bin/sqlcmd -S localhost -U SA -P StrongPassw0rd -Q "IF NOT EXISTS (SELECT 1 FROM sys.databases WHERE name = 'InterviewTest') BEGIN CREATE DATABASE InterviewTest; END;"

# Run sqlcmd to execute init.sql and log output
/opt/mssql-tools/bin/sqlcmd -S localhost -U SA -P StrongPassw0rd -d InterviewTest -i init.sql

# Keep the script running to keep the container running
tail -f /dev/null