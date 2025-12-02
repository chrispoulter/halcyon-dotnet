#!/bin/sh
set -e

echo "Generating runtime-config.js..."

cat <<EOF > /usr/share/nginx/html/runtime-config.js
window.__ENV__ = {
  VITE_RUNTIME_VALUE_1: "${VITE_RUNTIME_VALUE_1}",
  VITE_RUNTIME_VALUE_2: "${VITE_RUNTIME_VALUE_2}",
};
EOF