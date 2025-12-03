#!/bin/sh
set -e

echo "Generating runtime-config.js..."

cat <<EOF > /usr/share/nginx/html/runtime-config.js
window.__ENV__ = {
  VITE_RUNTIME_VALUE_1: "${VITE_RUNTIME_VALUE_1}",
  VITE_RUNTIME_VALUE_2: "${VITE_RUNTIME_VALUE_2}",
};
EOF

echo "Generating default.conf..."

cat <<EOF > /etc/nginx/conf.d/default.conf
server {
    listen       ${VITE_PORT};
    listen  [::]:${VITE_PORT};
    server_name  localhost;

    access_log  /var/log/nginx/server.access.log  main;

    location / {
        root /usr/share/nginx/html;
        try_files \$uri \$uri/ /index.html;
    }

    location /api/ {
        proxy_pass ${API_HTTP};
        proxy_http_version 1.1;
        proxy_ssl_server_name on;
        proxy_set_header X-Forwarded-For \$proxy_add_x_forwarded_for;
        rewrite ^/api(/.*)$ \$1 break;
    }
}
EOF

exec "$@"