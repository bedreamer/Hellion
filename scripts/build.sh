#!/usr/bin/env bash

dotnet restore
dotnet build src/Hellion.Core/ -f netstandard1.6
dotnet build src/Hellion.Database/ -f netstandard1.6
dotnet build src/Hellion.Login/ -f netcoreapp1.1
dotnet build src/Hellion.Cluster/ -f netcoreapp1.1
dotnet build src/Hellion.World/ -f netcoreapp1.1