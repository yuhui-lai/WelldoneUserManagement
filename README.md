# WelldoneUserManagement
使用者權限管理後台

### appsettings.json
放資料庫連線等參數，分為以下3種版本
- appsettings.Production.json 正式
- appsettings.Staging.json 測試
- appsettings.Development.json 開發

### pubxml部署文件
- Production正式.pubxml: ***還為編寫，須將Staging改為Production***
- Staging測試.pubxml: 
1. 修改執行環境(web.config)，iis執行時會吃appsettings.Staging.json設定檔
```xml
<!--測試環境-->
<EnvironmentName>Staging</EnvironmentName>
```
2. 並執行npm install、npm build ***(不同環境不同指令)*** 將wum.client 發佈檔複製到WUM.Server/wwwroot

### Dockerfile
根目錄的Dockerfile，同pubxml部署文件，改為docker版本
```dockerfile
docker build --build-arg DOTNET_ENV=Staging --build-arg NODE_ENV=staging -t wum:staging .
docker build --build-arg DOTNET_ENV=Production --build-arg NODE_ENV=production -t wum:latest .
```

