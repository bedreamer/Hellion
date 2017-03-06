#!/usr/bin/env bash

dotnet restore
dotnet build src/Hellion.Core/project.json -f netstandard1.6
dotnet build src/Hellion.Database/project.json -f netstandard1.6
dotnet build src/Hellion.Login/project.json -f netcoreapp1.0
dotnet build src/Hellion.Cluster/project.json -f netcoreapp1.0
dotnet build src/Hellion.World/project.json -f netcoreapp1.0