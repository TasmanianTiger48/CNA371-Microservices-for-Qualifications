# Running the system

This guide can be used to run the Qualifications system on a machine using Docker Hub. The machine pulls the published images from Docker Hub and runs them with Docker Compose or Kubernetes.

| Image | Contents |
|---|---|
| [`ryandewet577645/qualification-service`](https://hub.docker.com/r/ryandewet577645/qualification-service) | QualificationService API |
| [`ryandewet577645/assessment-service`](https://hub.docker.com/r/ryandewet577645/assessment-service) | AssessmentService API |
| [`ryandewet577645/qualification-interface`](https://hub.docker.com/r/ryandewet577645/qualification-interface) | Console client |
| `redis:7-alpine` | Official Redis image |

## 1. Prerequisites

- [Docker Desktop](https://www.docker.com/products/docker-desktop/) (Windows, macOS) or Docker Engine with the Compose plugin (Linux).
- Ports **5000**, **5054** and **6379** are free.
- For the Kubernetes option only: Kubernetes enabled in Docker Desktop (Settings → Kubernetes).

The images are built for `linux/amd64`.

## 2a. Run with Docker Compose

The repo's `docker-compose.yml` also builds from source, so the other machine uses a pull-only version instead. Create an empty folder and save this as `docker-compose.yml` in it:

```yaml
services:
  redis:
    image: redis:7-alpine
    ports:
      - "6379:6379"

  qualification-service:
    image: ryandewet577645/qualification-service:${TAG:-latest}
    ports:
      - "5054:8080"

  assessment-service:
    image: ryandewet577645/assessment-service:${TAG:-latest}
    ports:
      - "5000:8080"
    environment:
      ConnectionStrings__Redis: redis:6379
      Services__QualificationService: http://qualification-service:8080
    depends_on:
      - redis
      - qualification-service

  qualification-interface:
    image: ryandewet577645/qualification-interface:${TAG:-latest}
    stdin_open: true
    tty: true
    restart: unless-stopped
    environment:
      ASSESSMENT_SERVICE_URL: http://assessment-service:8080
    depends_on:
      - assessment-service
```

From that folder, start the system and attach to the client:

```
docker compose up -d
docker compose attach qualification-interface
```

The first `up` pulls the images, which takes a minute or two. The menu was printed before you attached, so the screen may start blank. If the menu is blank press Enter to refresh it.

**Update to the latest images:**

```
docker compose pull
docker compose up -d
```

**Stop and remove everything:** `docker compose down`. Redis data is not persisted, so all assessments and prospects are lost.

## 2b. Run with Kubernetes (Docker Desktop)

Kubernetes needs the manifests from the repo's `k8s/` folder, but not the rest of the source. Copy these five files to your machine, keeping them together in a `k8s` folder:

```
k8s/kustomization.yaml
k8s/namespace.yaml
k8s/redis.yaml
k8s/qualification-service.yaml
k8s/assessment-service.yaml
```

Run the follwoing to pull the images and deploy them to the cluster:

```
kubectl apply -k k8s
kubectl rollout status deployment -n qualifications --timeout=120s
```

**Start the client.** On any shell you can run the command directly:

```
kubectl run qualification-interface -n qualifications -it --rm --restart=Never \
    --image=ryandewet577645/qualification-interface:latest --image-pull-policy=Always \
    --env=ASSESSMENT_SERVICE_URL=http://assessment-service:5000
```

Choose **0** to exit the client, and its pod is removed. If the session is cut off without exiting, reconnect with the following command:

```
kubectl attach qualification-interface -c qualification-interface -n qualifications -i -t
```

**Update to the latest images:** `kubectl rollout restart deployment -n qualifications`. Because of `imagePullPolicy: Always`, the restarted pods pull the newest images.

**Remove everything:** `kubectl delete namespace qualifications`.

## 3. Check it's working

| | URL |
|---|---|
| AssessmentService API reference | http://localhost:5000/scalar/v1 |
| QualificationService API reference | http://localhost:5054/scalar/v1 |

A quick check from a terminal:

```
curl http://localhost:5054/qualifications
```

The response lists three qualifications: DIT, BIT and BCOMP.

## Troubleshooting

| Problem | Cause and fix |
|---|---|
| `Bind for 0.0.0.0:5000 failed: port is already allocated` | Another program (or the other run option) is using the port. Run only one option at a time. In the case of using macOS, turn off AirPlay Receiver as it is know to run on Port 5000. |
| `pull access denied` or `manifest unknown` | The image name or tag is wrong, or that tag was never pushed. Check the tags on Docker Hub. |
| The client prints `Could not reach the assessment service` | The AssessmentService isn't up yet. Wait a few seconds and try again. Check it with `docker compose ps` or `kubectl get pods -n qualifications`. |
| `pods "qualification-interface" already exists` | A previous client pod is still running. Reconnect to it, or remove it with `kubectl delete pod qualification-interface -n qualifications`. |