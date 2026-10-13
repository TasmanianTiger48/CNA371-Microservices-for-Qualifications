function Invoke-Native {
    & $args[0] $args[1..($args.Count - 1)]
    if ($LASTEXITCODE -ne 0) { throw "Command failed: $args" }
}

$compose = Join-Path (Split-Path $PSScriptRoot -Parent) 'docker-compose.yml'

Invoke-Native docker compose -f $compose build
Invoke-Native docker compose -f $compose push

Invoke-Native kubectl apply -k $PSScriptRoot
Invoke-Native kubectl rollout restart deployment -n qualifications
Invoke-Native kubectl rollout status deployment -n qualifications --timeout=120s
