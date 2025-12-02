#!/bin/sh
set -e

echo "Generating runtime-config.js..."

cat <<EOF > /usr/share/nginx/html/runtime-config.js
window.__ENV__ = {
  RUNTIME_VALUE_1: "${RUNTIME_VALUE_1}",
  RUNTIME_VALUE_2: "${RUNTIME_VALUE_2}",
};
EOF