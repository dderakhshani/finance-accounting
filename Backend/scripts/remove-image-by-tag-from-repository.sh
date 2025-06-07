 #!/bin/bash 

REPOSITORY_NAME=$(echo $1 | tr '[:upper:]' '[:lower:]')
TAG=$(echo $2 | tr '[:upper:]' '[:lower:]')
 
DIGEST=$(curl -sS -H 'Accept: application/vnd.docker.distribution.manifest.v2+json' -o /dev/null -w '%header{Docker-Content-Digest}'  localhost:5000/v2/$REPOSITORY_NAME/manifests/$TAG)
curl -sS -X DELETE  localhost:5000/v2/$REPOSITORY_NAME/manifests/$DIGEST


  docker run --rm \
  -v /home/root1/data/registry/registry_data:/var/lib/registry \
  registry:2 \
  garbage-collect /etc/docker/registry/config.yml >&2