#!/bin/bash 

PROJECT_FOLDER_NAME=$1
REPOSITORY_ADDRESS=$2

if [ -z "$PROJECT_FOLDER_NAME" ] || [ -z "$REPOSITORY_ADDRESS" ]; then
  echo "Usage: $0 <PROJECT_FOLDER_NAME> <REPOSITORY_ADDRESS>"
  exit 1
fi

TARGET_FOLDER="/tmp/$PROJECT_FOLDER_NAME"

if [ -d "$TARGET_FOLDER" ]; then
  echo "Folder exists: $TARGET_FOLDER. Pulling latest changes..."
  cd "$TARGET_FOLDER" || { echo "Failed to navigate to $TARGET_FOLDER."; exit 1; }
  if git pull --quiet; then
    echo "Successfully pulled the latest changes."
  else
    echo "Error occurred while pulling changes." >&2
    exit 1
  fi
else
  echo "Folder does not exist: $TARGET_FOLDER. Cloning repository..."
  if git clone "$REPOSITORY_ADDRESS" "$TARGET_FOLDER"; then
    echo "Repository cloned successfully into $TARGET_FOLDER."
  else
    echo "Error occurred while cloning the repository." >&2
    exit 1
  fi
fi

echo "Script execution completed successfully."
exit 0