FROM ubuntu:20.04
RUN apt-get update -y
RUN apt-get install -y nginx
COPY openssl/localhost.key ./etc/ssl/localhost.key
COPY openssl/localhost.crt ./etc/ssl/localhost.crt
COPY nginx/nginx-debug.conf ./etc/nginx/nginx.conf
COPY scripts/nginx-entrypoint-debug.sh .
RUN chmod +x ./nginx-entrypoint-debug.sh
ENTRYPOINT ["./nginx-entrypoint-debug.sh"]
