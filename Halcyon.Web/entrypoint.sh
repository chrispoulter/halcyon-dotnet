#!/bin/sh

echo "Replacing runtime environment variables..."

find /usr/share/nginx/html/assets -type f -name "*.js" -print0 | xargs -0 sed -i \
	-e "s|VITE_API_URL:void 0|VITE_API_URL: \"${VITE_API_URL}\"|g"
