#!/bin/bash
set -x

if [[ $(git status -s) ]]; then
    echo "You have uncommitted files. Commit and push them before running this script."
    exit 1
fi

git fetch --tags

# get latest git tag and increase by one (see https://stackoverflow.com/questions/4485399/how-can-i-bump-a-version-number-using-bash)
VERSION=`git describe --abbrev=0 | awk -F. '/[0-9]+\./{$NF++;print}' OFS=.`

echo "setting version to $VERSION"

function setVersion_Nupkg {
  sed -i '' "s/\(<version>\).*\(<\/version>\)/\1$VERSION\2/" $1
}

function packNuGet {
	setVersion_Nupkg $1
	nuget pack $1 || exit 1
}

function publishNuGet {
  nuget push -Source https://www.nuget.org/api/v2/package $1 || exit 1
}

function createTag {
  git tag -a $VERSION -m ''  || exit 1
  git push --tags || exit 1
}

dotnet restore QuickTest.sln || exit 1

dotnet build --configuration Release QuickTest/QuickTest.csproj || exit 1
dotnet build --configuration Release Tests/Tests.csproj || exit 1

dotnet test Tests/Tests.csproj

createTag

packNuGet Zauberzeug.QuickTest.Maui.nuspec

if [[ $SKIP_DEPLOYMENT == True ]]; then
  echo "Skipping deployment"
  exit 0
fi

publishNuGet Zauberzeug.QuickTest.Maui.$VERSION.nupkg
