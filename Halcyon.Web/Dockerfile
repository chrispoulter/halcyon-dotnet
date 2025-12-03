FROM node:24-alpine AS build

WORKDIR /app

COPY package.json package.json
COPY package-lock.json package-lock.json

RUN npm ci

COPY . .

RUN npm run build

FROM nginx:alpine AS final

# COPY --from=build /app/default.conf.template /etc/nginx/templates/default.conf.template
COPY --from=build /app/dist /usr/share/nginx/html
# COPY --from=build /app/entrypoint.sh /docker-entrypoint.d/entrypoint.sh
COPY --from=build /app/entrypoint.sh /entrypoint.sh

# RUN chmod +x /docker-entrypoint.d/entrypoint.sh
RUN chmod +x /entrypoint.sh

EXPOSE 80

ENTRYPOINT ["/entrypoint.sh"]
CMD ["nginx", "-g", "daemon off;"]