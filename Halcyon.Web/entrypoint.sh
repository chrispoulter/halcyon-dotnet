#!/bin/sh
set -e

find /usr/share/nginx/html/assets -type f -name "*.js" -print0 | xargs -0 sed -i \
	-e "s|__VITE_RUNTIME_VALUE_1__|${VITE_RUNTIME_VALUE_1}|g" \
	-e "s|__VITE_RUNTIME_VALUE_2__|${VITE_RUNTIME_VALUE_2}|g"
