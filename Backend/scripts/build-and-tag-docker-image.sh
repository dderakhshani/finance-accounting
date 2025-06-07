#!/bin/bash 

PROJECT_FOLDER_NAME=$1
PROJECT_NAME=$2

cd /tmp/$PROJECT_FOLDER_NAME

VERSION=$(grep -oPm1 "(?<=<Version>)[^<]+" ./Microservices/$PROJECT_NAME/Eefa.Admin.WebApi/Eefa.Admin.WebApi.csproj)

IMAGE_TAG=$(echo "$PROJECT_FOLDER_NAME-$PROJECT_NAME:$VERSION" | tr '[:upper:]' '[:lower:]')
REGISTRY_IMAGE_TAG=localhost:5000/eefa/$(echo $PROJECT_NAME | tr '[:upper:]' '[:lower:]'):$VERSION

docker build -f ./Microservices/Admin/Eefa.Admin.WebApi/Dockerfile --no-cache -t $IMAGE_TAG .  2>&1

docker tag $IMAGE_TAG localhost:5000/eefa/$(echo $PROJECT_NAME | tr '[:upper:]' '[:lower:]'):$VERSION

docker push $REGISTRY_IMAGE_TAG 2>&1

docker image prune -a -f