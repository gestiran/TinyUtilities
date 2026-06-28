#!/bin/bash

dotnet build -c Release

cp -f bin/Release/netstandard2.1/* ../Libraries