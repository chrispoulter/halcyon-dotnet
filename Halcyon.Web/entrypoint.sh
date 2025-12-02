#!/bin/sh
set -e

echo "Replacing runtime environment variables..."
envsubst < /usr/share/nginx/html/index.html > /usr/share/nginx/html/index.tmp.html
mv /usr/share/nginx/html/index.tmp.html /usr/share/nginx/html/index.html