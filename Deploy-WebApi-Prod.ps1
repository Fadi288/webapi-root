# CONFIG
$composeFile = "docker-compose.prod.yml"
$serviceName = "webapi"
$imageTag = "fadisami/webapiapp:prod"

# 1. Build the production image
Write-Host "🧱 Building image using $composeFile..."
docker-compose -f $composeFile build

if ($LASTEXITCODE -ne 0) {
    Write-Host "❌ Build failed." -ForegroundColor Red
    exit 1
}

# 2. Tagging is already handled by the compose file (via `image:` key)

# 3. Push the image to Docker Hub
Write-Host "☁️ Pushing $imageTag to Docker Hub..."
docker push $imageTag

if ($LASTEXITCODE -ne 0) {
    Write-Host "❌ Push failed." -ForegroundColor Red
    exit 1
}

# 4. Clean up any old container running this image
Write-Host "🧼 Stopping existing container (if any)..."
$existingContainer = docker ps -aq --filter ancestor=$imageTag
if ($existingContainer) {
    docker stop $existingContainer | Out-Null
    docker rm $existingContainer | Out-Null
    Write-Host "✅ Old container removed."
}

# 5. Run new container from pushed image
Write-Host "🚀 Running new container from $imageTag..."
docker run -d -p 7000:5000 --name webapi-prod $imageTag

if ($LASTEXITCODE -eq 0) {
    Write-Host "✅ Web API running at: http://localhost:7000" -ForegroundColor Green
} else {
    Write-Host "❌ Failed to run container." -ForegroundColor Red
}
