#!/bin/sh

if [ "$(dirname $0)" = "." ]
then
    cd ../src/wrenstone
else
    parent=$(dirname $(dirname "$0"))
    cd "$parent"/src/wrenstone
fi

date=$(date '+%H-%M-%S_%d-%m-%Y')

dotnet ef migrations add _"$date" --context DefaultDbContext
