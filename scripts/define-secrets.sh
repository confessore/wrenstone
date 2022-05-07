#!/bin/sh

read -p "Please enter the sql server address (example - 'mariadb' for docker-compose): " sqlServer
read -p "Please enter the sql username (example - 'root'): " sqlUsername
read -p "Please enter the sql password: " sqlPassword
read -p "Please enter the sql database name (example - 'justchooseanydbname'): " sqlDatabase
read -p "Please enter the smtp host address (example -'mail.privateemail.com' for namecheap): " smtpHost
read -p "Please enter the smtp port (example - '465' for namecheap): " smtpPort
read -p "Please enter the smtp username (example 'noreply@example.com'): " smtpUsername
read -p "Please enter the smtp password: " smtpPassword
read -p "Please enter the smtp from name (example - 'Nigerian Prince'): " smtpFromName
read -p "Please enter the smtp from address (example - 'noreply@example.com'): " smtpFromAddress
read -p "please enter the discord bot token " discordBotToken
parent=$(dirname $(dirname "$0"))
cd "$parent"/secrets
echo "generating secrets..."
echo "$sqlServer" > sql-server
echo "$sqlUsername" > sql-username
echo "$sqlPassword" > sql-password
echo "$sqlDatabase" > sql-database
echo "$smtpHost" > smtp-host
echo "$smtpPort" > smtp-port
echo "$smtpUsername" > smtp-username
echo "$smtpPassword" > smtp-password
echo "$smtpFromName" > smtp-fromname
echo "$smtpFromAddress" > smtp-fromaddress
echo "$discordBotToken" > discordBotToken
echo "secrets generated!"
