# CONFIG
$composeFile = "docker-compose.prod.yml"
$serviceName = "razorapp"
$imageTag = "fadisami/razorapp:prod"

# 1. Build the production image
Write-Host "🧱 Building image using $composeFile..."
docker-compose -f $composeFile build $serviceName

if ($LASTEXITCODE -ne 0) {
    Write-Host "❌ Build failed." -ForegroundColor Red
    exit 1
}

# 2. Push the image to Docker Hub
Write-Host "☁️ Pushing $imageTag to Docker Hub..."
docker push $imageTag

if ($LASTEXITCODE -ne 0) {
    Write-Host "❌ Push failed." -ForegroundColor Red
    exit 1
}

# 3. Stop and remove any existing container running this image
Write-Host "🧼 Stopping existing container (if any)..."
$existingContainer = docker ps -aq --filter ancestor=$imageTag
if ($existingContainer) {
    docker stop $existingContainer | Out-Null
    docker rm $existingContainer | Out-Null
    Write-Host "✅ Old container removed."
}

# 4. Run new container
Write-Host "🚀 Running new container from $imageTag..."
# docker run -d -p 8000:5000 --name razorapp-prod $imageTag

docker-compose -f $composeFile up -d $serviceName

if ($LASTEXITCODE -eq 0) {
    Write-Host "✅ Razor app running at: http://localhost:8000" -ForegroundColor Green
} else {
    Write-Host "❌ Failed to run container." -ForegroundColor Red
}
