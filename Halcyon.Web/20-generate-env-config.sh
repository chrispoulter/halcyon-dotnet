#!/bin/sh
set -e

{
  echo "window.__ENV__ ||= {"
  echo "  VITE_API_URL: \"${VITE_API_URL}\","
  [ -n "${VITE_RUNTIME_VALUE_1}" ] && echo "  VITE_RUNTIME_VALUE_1: \"${VITE_RUNTIME_VALUE_1}\","
  [ -n "${VITE_RUNTIME_VALUE_2}" ] && echo "  VITE_RUNTIME_VALUE_2: \"${VITE_RUNTIME_VALUE_2}\","
  echo "};"
} > /usr/share/nginx/html/env.js