#!/bin/bash

rm ./coverage_report/ -rf
find -name coverage.info -delete
find -name coverage.cobertura.xml -delete
find -name coverage.opencover.xml -delete
find -name coverage.json -delete
dotnet clean
dotnet tool update dotnet-reportgenerator-globaltool
dotnet build
dotnet test --no-build --collect:"XPlat Code Coverage" --logger "trx" /p:CollectCoverage=true /p:CoverletOutput="../" /p:MergeWith="../coverage.json" /p:CoverletOutputFormat=json%2copencover%2clcov%2ccobertura
dotnet reportgenerator -reports:./__tests__/coverage.opencover.xml -targetdir:coverage_report
dotnet reportgenerator -reports:./__tests__/coverage.info -targetdir:coverage_report -reporttypes:"lcov"
npx http-server -o coverage_report
