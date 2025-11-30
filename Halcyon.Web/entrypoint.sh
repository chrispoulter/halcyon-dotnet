#!/bin/sh

echo "Replacing runtime environment variables..."

find /usr/share/nginx/html/assets -type f -name "*.js" -print0 | xargs -0 sed -i \
	-e "s|VITE_RUNTIME_VALUE_1:void 0|VITE_RUNTIME_VALUE_1: \"${VITE_RUNTIME_VALUE_1}\"|g" \
	-e "s|VITE_RUNTIME_VALUE_2:void 0|VITE_RUNTIME_VALUE_2: \"${VITE_RUNTIME_VALUE_2}\"|g"