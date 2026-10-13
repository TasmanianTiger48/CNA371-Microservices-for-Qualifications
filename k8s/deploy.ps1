# Builds the images, loads them into the Docker Desktop (kind) cluster and applies the manifests.

function Invoke-Native {
    & $args[0] $args[1..($args.Count - 1)]
    if ($LASTEXITCODE -ne 0) { throw "Command failed: $args" }
}

$root = Split-Path $PSScriptRoot -Parent
$node = 'desktop-control-plane'
$images = 'qualification-service', 'assessment-service', 'qualification-interface'

Invoke-Native docker compose -f "$root\docker-compose.yml" build

foreach ($image in $images) {
    Write-Host "Loading $image into the cluster..."
    $archive = Join-Path $env:TEMP "$image.tar"
    Invoke-Native docker save -o $archive "${image}:latest"
    Invoke-Native docker cp $archive "${node}:/var/tmp/$image.tar"
    Invoke-Native docker exec $node ctr --namespace=k8s.io images import --digests --snapshotter=overlayfs "/var/tmp/$image.tar"
    Invoke-Native docker exec $node rm "/var/tmp/$image.tar"
    Remove-Item $archive
}

Invoke-Native kubectl apply -k $PSScriptRoot
Invoke-Native kubectl rollout restart deployment -n qualifications
Invoke-Native kubectl rollout status deployment -n qualifications --timeout=120s
