FROM node:22.12.0 AS builder

WORKDIR /app

COPY ["src/Server/ui/", "./"]

ARG VITE_API_URL
ENV VITE_API_URL=$VITE_API_URL

RUN npm install

RUN npm run build

FROM node:22.12.0

WORKDIR /app

RUN npm install -g serve

COPY --from=builder /app/dist ./dist

EXPOSE 3000

CMD ["serve", "-s", "dist", "-l", "3000"]
