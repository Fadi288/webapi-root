# CONFIG
$imageTag = "fadisami/consoleapp:prod"
$serviceName = "consoleapp"
$composeFile = "docker-compose.prod.yml"

# 1. Build image
Write-Host "🧱 Building console app image..."
docker-compose -f $composeFile build $serviceName

if ($LASTEXITCODE -ne 0) {
    Write-Host "❌ Build failed." -ForegroundColor Red
    exit 1
}

# 2. Push image
Write-Host "☁️ Pushing $imageTag to Docker Hub..."
docker push $imageTag

if ($LASTEXITCODE -ne 0) {
    Write-Host "❌ Push failed." -ForegroundColor Red
    exit 1
}

# 3. Remove existing container if any
Write-Host "🧼 Removing existing container (if any)..."
$existingContainer = docker ps -aq --filter ancestor=$imageTag
if ($existingContainer) {
    docker stop $existingContainer | Out-Null
    docker rm $existingContainer | Out-Null
    Write-Host "✅ Old container removed."
}

# 4. Run container (detached)
Write-Host "🚀 Running console app container..."
docker-compose -f $composeFile up -d $serviceName

if ($LASTEXITCODE -eq 0) {
    Write-Host "✅ Console app container started." -ForegroundColor Green
} else {
    Write-Host "❌ Failed to run container." -ForegroundColor Red
}
