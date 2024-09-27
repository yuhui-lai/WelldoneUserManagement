# 使用官方 Node.js 圖像作為基礎圖像
FROM node:alpine

# 設置工作目錄
WORKDIR /app

# 複製 package.json 和 package-lock.json
COPY package*.json ./

# 安裝依賴
RUN npm install

# 設置 NODE_ENV 參數
ARG NODE_ENV
ENV NODE_ENV=$NODE_ENV

# 複製所有文件到工作目錄
COPY . .

# 根據環境變數運行不同的構建命令
RUN if [ "$NODE_ENV" = "production" ]; then \
    echo "Running production build" && npm run build; \
    elif [ "$NODE_ENV" = "staging" ]; then \
    echo "Running staging build" && npm run staging; \
    else \
    echo "Running development build" && npm run dev; \
    fi

# 暴露應用運行的端口
EXPOSE 5173

# 運行應用（根據環境使用不同的啟動命令）
CMD ["npm", "run", "serve"]

# Development
# docker build --build-arg NODE_ENV=development -t wum.client:dev .
# docker run -p 8703:5173 wum.client:dev

# Staging
# docker build --build-arg NODE_ENV=staging -t wum.client:staging .
# docker run -p 8703:5173 wum.client:staging

# Production
# docker build --build-arg NODE_ENV=production -t wum.client:prod .
# docker run -p 8703:5173 wum.client:prod