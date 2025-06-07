FROM node:latest as builder

RUN yes | apt-get update && apt-get -y upgrade
RUN yes | apt-get install g++ make python

WORKDIR /app

COPY package*.json ./
RUN npm install -g npm@8.3.0
RUN npm install

COPY . .

#RUN npm run build

FROM node:lts-alpine
#RUN npm install -g http-server
WORKDIR /app
COPY --from=builder /app .
CMD [ "npm","start" ]
#CMD [ "http-server", "dist", "-p 80" ]
