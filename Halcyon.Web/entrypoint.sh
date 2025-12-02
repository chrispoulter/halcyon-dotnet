#!/bin/sh
set -e

echo "Generating runtime config..."
envsubst < /etc/nginx/conf.d/default.conf > /etc/nginx/conf.d/default.tmp.conf
mv /etc/nginx/conf.d/default.tmp.conf /etc/nginx/conf.d/default.conf

envsubst < /usr/share/nginx/html/index.html > /usr/share/nginx/html/index.tmp.html
mv /usr/share/nginx/html/index.tmp.html /usr/share/nginx/html/index.html

echo "Starting Nginx... $@"
exec "$@"