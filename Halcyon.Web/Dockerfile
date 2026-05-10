FROM node:24-alpine AS build

WORKDIR /app

COPY package.json package.json
COPY package-lock.json package-lock.json

RUN npm ci

COPY . .

RUN npm run build

FROM nginx:alpine AS final

COPY --from=build /app/default.conf.template /etc/nginx/templates/default.conf.template
COPY --from=build /app/dist /usr/share/nginx/html
COPY --from=build /app/20-generate-env-config.sh /docker-entrypoint.d/20-generate-env-config.sh

RUN chmod +x /docker-entrypoint.d/20-generate-env-config.sh

EXPOSE 80

CMD ["nginx", "-g", "daemon off;"]